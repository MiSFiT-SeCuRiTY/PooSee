using Microsoft.Playwright;
using PooSee.Models;

namespace PooSee.Services;

public sealed class SessionManager : ISessionManager
{
    private readonly ILogService _log;
    private readonly IBrowserService _browserService;
    private readonly IProfileManager _profileManager;
    private readonly IRefreshScheduler _scheduler;
    private readonly ISessionDistributionStrategy _strategy;

    private readonly List<SessionInfo> _sessions = new();
    private readonly List<SessionRuntime> _runtimes = new();
    private CancellationTokenSource? _cts;
    private DateTime _startedAt;

    public event EventHandler? SessionsChanged;

    public bool IsRunning { get; private set; }
    public IReadOnlyList<SessionInfo> Sessions => _sessions;

    public SessionManager(
        ILogService log,
        IBrowserService browserService,
        IProfileManager profileManager,
        IRefreshScheduler scheduler,
        ISessionDistributionStrategy strategy)
    {
        _log = log;
        _browserService = browserService;
        _profileManager = profileManager;
        _scheduler = scheduler;
        _strategy = strategy;
    }

    public async Task StartAsync(SessionConfiguration configuration, CancellationToken ct = default)
    {
        if (IsRunning) return;

        _sessions.Clear();
        _runtimes.Clear();
        _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        _startedAt = DateTime.Now;
        IsRunning = true;

        var jobs = configuration.Jobs.Where(j => j.Enabled).ToList();
        if (jobs.Count == 0)
            throw new InvalidOperationException("No enabled URL jobs.");

        // 1. Detect and start the browser.
        var browser = configuration.BrowserExecutablePath is null
            ? throw new InvalidOperationException("No browser selected.")
            : new BrowserInfo
            {
                Name = configuration.BrowserName ?? "Browser",
                ExecutablePath = configuration.BrowserExecutablePath,
                IsAvailable = true,
                Channel = "chrome"
            };

        await _browserService.StartAsync(browser, _cts.Token);

        // 2. Distribute sessions across jobs.
        var assigned = _strategy.Distribute(configuration.SessionCount, jobs);

        // 3. Create each session.
        for (int i = 0; i < assigned.Count; i++)
        {
            var sessionId = i + 1;
            var job = assigned[i];

            var info = new SessionInfo
            {
                SessionId = sessionId,
                Url = job.Url,
                Status = SessionStatus.Starting,
                BrowserName = browser.Name
            };
            _sessions.Add(info);

            try
            {
                var profileDir = _profileManager.CreateProfile(sessionId, persistent: false);
                var context = await _browserService.CreateContextAsync(profileDir, _cts.Token);
                var page = await context.NewPageAsync();

                var runtime = new SessionRuntime
                {
                    Info = info,
                    Job = job,
                    Context = context,
                    Page = page,
                    ProfileDirectory = profileDir
                };
                _runtimes.Add(runtime);

                // Navigate to the target URL once at start.
                try
                {
                    await page.GotoAsync(job.Url, new PageGotoOptions
                    {
                        WaitUntil = WaitUntilState.DOMContentLoaded,
                        Timeout = 30_000
                    });
                    info.Status = SessionStatus.Running;
                    info.LastRefresh = DateTime.Now;
                    _log.Info($"Session {sessionId:D2} started → {job.Url}");
                }
                catch (Exception ex)
                {
                    info.Status = SessionStatus.Failed;
                    info.Error = ex.Message;
                    _log.Warn($"Session {sessionId:D2} navigation failed: {ex.Message}");
                    continue;
                }

                // Kick off this session's refresh loop.
                var sId = sessionId;
                var min = job.MinimumIntervalSeconds;
                var max = job.MaximumIntervalSeconds;
                runtime.LoopTask = Task.Run(async () =>
                {
                    await _scheduler.RunAsync(
                        sId, min, max,
                        async token => await ReloadAsync(runtime, token),
                        fireAt => runtime.Info.NextRefresh = fireAt,
                        _cts.Token);
                }, _cts.Token);
            }
            catch (Exception ex)
            {
                info.Status = SessionStatus.Failed;
                info.Error = ex.Message;
                _log.Error($"Session {sessionId:D2} setup failed", ex);
            }
        }

        RaiseChanged();
    }

    private async Task ReloadAsync(SessionRuntime runtime, CancellationToken token)
    {
        try
        {
            if (runtime.Page is null) return;
            await runtime.Page.ReloadAsync(new PageReloadOptions
            {
                WaitUntil = WaitUntilState.DOMContentLoaded,
                Timeout = 30_000
            });
            runtime.Info.LastRefresh = DateTime.Now;
            runtime.Info.RefreshCount++;
            _log.Debug($"Session {runtime.Info.SessionId:D2} refreshed (#{runtime.Info.RefreshCount})");
        }
        catch (Exception ex)
        {
            runtime.Info.Status = SessionStatus.Failed;
            runtime.Info.Error = ex.Message;
            _log.Warn($"Session {runtime.Info.SessionId:D2} reload error: {ex.Message}");
        }
    }

    public async Task StopAllAsync()
    {
        if (!IsRunning) return;

        try { _cts?.Cancel(); } catch { }

        // Give loops a moment.
        await Task.Delay(150);

        foreach (var rt in _runtimes)
        {
            try { if (rt.LoopTask is not null) await rt.LoopTask; }
            catch { }

            try { if (rt.Page is not null) await rt.Page.CloseAsync(); }
            catch { }
            try { if (rt.Context is not null) await rt.Context.CloseAsync(); }
            catch { }

            rt.Info.Status = SessionStatus.Stopped;

            try { _profileManager.CleanupAfterSession(rt.Info.SessionId, false, cleanup: true); }
            catch { }
        }

        try { await _browserService.StopAsync(); } catch { }

        _runtimes.Clear();
        _cts?.Dispose();
        _cts = null;
        IsRunning = false;
        RaiseChanged();

        _log.Info("All sessions stopped.");
    }

    private void RaiseChanged() => SessionsChanged?.Invoke(this, EventArgs.Empty);

    public async ValueTask DisposeAsync()
    {
        await StopAllAsync();
    }

    private sealed class SessionRuntime
    {
        public SessionInfo Info { get; set; } = default!;
        public UrlJob Job { get; set; } = default!;
        public IBrowserContext? Context { get; set; }
        public IPage? Page { get; set; }
        public string? ProfileDirectory { get; set; }
        public Task? LoopTask { get; set; }
    }
}