using System.Collections.ObjectModel;
using System.Windows.Threading;
using PooSee.Infrastructure;
using PooSee.Models;
using PooSee.Services;

namespace PooSee.ViewModels;

public sealed class MainViewModel : ObservableObject
{
    private readonly ILogService _log;
    private readonly IBrowserDetector _browserDetector;
    private readonly ISessionManager _sessionManager;
    private readonly ISettingsService _settings;
    private readonly IPresetManager _presets;
    private readonly IHistoryService _history;
    private readonly ISystemResourceService _resources;
    private readonly Func<SessionConfiguration> _configProvider;
    private readonly DispatcherTimer _tickTimer;

    private string _browserName = "Detecting…";
    private string _browserStatus = "";
    private int _sessionCount = 1;
    private string _statusText = "Idle";
    private bool _isRunning;
    private bool _hasErrors;

    public ObservableCollection<UrlJobViewModel> Jobs { get; } = new();
    public ObservableCollection<SessionInfo> Sessions { get; } = new();

    public string BrowserName { get => _browserName; set => SetProperty(ref _browserName, value); }
    public string BrowserStatus { get => _browserStatus; set => SetProperty(ref _browserStatus, value); }

    public int SessionCount
    {
        get => _sessionCount;
        set
        {
            if (value < 1) value = 1;
            if (value > _settings.Current.MaximumSessionCount) value = _settings.Current.MaximumSessionCount;
            SetProperty(ref _sessionCount, value);
        }
    }

    public string StatusText { get => _statusText; set => SetProperty(ref _statusText, value); }

    public bool IsRunning
    {
        get => _isRunning;
        set { if (SetProperty(ref _isRunning, value)) OnPropertyChanged(nameof(IsIdle)); }
    }

    public bool IsIdle => !IsRunning;
    public bool HasErrors { get => _hasErrors; set => SetProperty(ref _hasErrors, value); }

    public int RecommendedSessionCount { get; private set; }

    public RelayCommand AddUrlCommand { get; }
    public RelayCommand<UrlJobViewModel> EditUrlCommand { get; }
    public RelayCommand<UrlJobViewModel> RemoveUrlCommand { get; }
    public RelayCommand<UrlJobViewModel> ToggleEnabledCommand { get; }
    public AsyncRelayCommand StartCommand { get; }
    public AsyncRelayCommand StopCommand { get; }
    public RelayCommand OpenSettingsCommand { get; }
    public RelayCommand OpenPresetsCommand { get; }
    public RelayCommand OpenAboutCommand { get; }

    public Action<UrlJobViewModel?>? RequestEditUrl { get; set; }
    public Action? RequestOpenSettings { get; set; }
    public Action? RequestOpenPresets { get; set; }
    public Action? RequestOpenAbout { get; set; }

    public MainViewModel(
        ILogService log,
        IBrowserDetector browserDetector,
        ISessionManager sessionManager,
        ISettingsService settings,
        IPresetManager presets,
        IHistoryService history,
        ISystemResourceService resources,
        Func<SessionConfiguration> configProvider)
    {
        _log = log;
        _browserDetector = browserDetector;
        _sessionManager = sessionManager;
        _settings = settings;
        _presets = presets;
        _history = history;
        _resources = resources;
        _configProvider = configProvider;

        AddUrlCommand = new RelayCommand(() => RequestEditUrl?.Invoke(null));
        EditUrlCommand = new RelayCommand<UrlJobViewModel>(vm => { if (vm is not null) RequestEditUrl?.Invoke(vm); });
        RemoveUrlCommand = new RelayCommand<UrlJobViewModel>(vm => { if (vm is not null) Jobs.Remove(vm); });
        ToggleEnabledCommand = new RelayCommand<UrlJobViewModel>(vm => { if (vm is not null) vm.Enabled = !vm.Enabled; });

        StartCommand = new AsyncRelayCommand(StartAsync, () => !IsRunning && Jobs.Count > 0);
        StopCommand = new AsyncRelayCommand(StopAsync, () => IsRunning);

        OpenSettingsCommand = new RelayCommand(() => RequestOpenSettings?.Invoke());
        OpenPresetsCommand = new RelayCommand(() => RequestOpenPresets?.Invoke());
        OpenAboutCommand = new RelayCommand(() => RequestOpenAbout?.Invoke());

        _sessionManager.SessionsChanged += (_, _) =>
            System.Windows.Application.Current.Dispatcher.Invoke(SyncSessions);

        _tickTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _tickTimer.Tick += (_, _) =>
        {
            foreach (var s in Sessions) s.Tick();
        };
        _tickTimer.Start();
    }

    public async Task InitializeAsync()
    {
        await _settings.LoadAsync();
        await _presets.LoadAsync();
        await _history.LoadAsync();

        RecommendedSessionCount = _resources.GetRecommendedSessionCount();
        OnPropertyChanged(nameof(RecommendedSessionCount));

        if (_settings.Current.RestorePreviousConfiguration && _settings.Current.LastJobs.Count > 0)
        {
            Jobs.Clear();
            foreach (var j in _settings.Current.LastJobs)
                Jobs.Add(new UrlJobViewModel(j));

            SessionCount = _settings.Current.LastSessionCount > 0
                ? _settings.Current.LastSessionCount
                : (_settings.Current.AutoRecommendSessions ? RecommendedSessionCount : _settings.Current.ManualDefaultSessionCount);
        }
        else
        {
            SessionCount = _settings.Current.AutoRecommendSessions
                ? RecommendedSessionCount
                : _settings.Current.ManualDefaultSessionCount;
        }

        DetectBrowser();
    }

    public void DetectBrowser()
    {
        try
        {
            var preferred = _browserDetector.DetectPreferred(
                _settings.Current.PreferredBrowserChannel,
                _settings.Current.PreferredBrowserExecutablePath);

            if (preferred is null || !preferred.IsAvailable)
            {
                BrowserName = "No browser detected";
                BrowserStatus = "Install Chrome, Edge or Chromium.";
                HasErrors = true;
                return;
            }

            BrowserName = preferred.Name;
            BrowserStatus = $"{preferred.Version ?? ""}  ·  {preferred.ExecutablePath}";
            HasErrors = false;
        }
        catch (Exception ex)
        {
            _log.Error("Browser detection failed", ex);
            BrowserName = "Detection error";
            BrowserStatus = ex.Message;
            HasErrors = true;
        }
    }

    private async Task StartAsync()
    {
        HasErrors = false;

        var errors = Validate();
        if (errors.Count > 0)
        {
            HasErrors = true;
            System.Windows.MessageBox.Show(
                string.Join(Environment.NewLine, errors),
                "PooSee — Please fix the following",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Warning);
            return;
        }

        _settings.Current.LastJobs = Jobs.Select(j => j.Model.Clone()).ToList();
        _settings.Current.LastSessionCount = SessionCount;
        await _settings.SaveAsync();

        var config = _configProvider();
        config.SessionCount = SessionCount;
        config.Jobs = Jobs.Select(j => j.Model).ToList();

        try
        {
            StatusText = "Starting…";
            await _sessionManager.StartAsync(config);
            IsRunning = true;
            StatusText = $"{_sessionManager.Sessions.Count} session(s) running";
        }
        catch (Exception ex)
        {
            _log.Error("Start failed", ex);
            System.Windows.MessageBox.Show(
                $"Failed to start sessions:{Environment.NewLine}{ex.Message}",
                "PooSee — Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);
            StatusText = "Idle";
            IsRunning = false;
        }
    }

    private async Task StopAsync()
    {
        try
        {
            StatusText = "Stopping…";
            var entries = _sessionManager.Sessions.ToList();
            var startedAt = DateTime.UtcNow;
            await _sessionManager.StopAllAsync();

            var hist = new SessionHistoryEntry
            {
                StartedUtc = startedAt,
                EndedUtc = DateTime.UtcNow,
                SessionCount = entries.Count,
                Urls = entries.Select(s => s.Url).Distinct().ToList(),
                TotalRefreshes = entries.Sum(s => s.RefreshCount),
                SuccessfulSessions = entries.Count(s => s.Status == SessionStatus.Stopped || s.Status == SessionStatus.Running),
                FailedSessions = entries.Count(s => s.Status == SessionStatus.Failed)
            };
            await _history.RecordAsync(hist);
        }
        catch (Exception ex)
        {
            _log.Warn($"Stop error: {ex.Message}");
        }
        finally
        {
            IsRunning = false;
            StatusText = "Idle";
            SyncSessions();
        }
    }

    private List<string> Validate()
    {
        var errors = new List<string>();

        if (Jobs.Count == 0)
            errors.Add("• Add at least one URL job.");

        var enabled = Jobs.Where(j => j.Enabled).ToList();
        if (enabled.Count == 0 && Jobs.Count > 0)
            errors.Add("• At least one URL job must be enabled.");

        foreach (var j in Jobs)
        {
            if (!Uri.TryCreate(j.Url, UriKind.Absolute, out var u) ||
                (u.Scheme != Uri.UriSchemeHttp && u.Scheme != Uri.UriSchemeHttps))
            {
                errors.Add($"• '{j.Url}' is not a valid HTTP or HTTPS URL.");
            }

            if (j.MinimumIntervalSeconds < 1)
                errors.Add($"• '{j.Url}' minimum interval must be at least 1 second.");
            if (j.MaximumIntervalSeconds < 1)
                errors.Add($"• '{j.Url}' maximum interval must be at least 1 second.");
            if (j.MinimumIntervalSeconds > j.MaximumIntervalSeconds)
                errors.Add($"• '{j.Url}' minimum interval must be ≤ maximum interval.");
        }

        if (_browserDetector.DetectPreferred(
                _settings.Current.PreferredBrowserChannel,
                _settings.Current.PreferredBrowserExecutablePath) is null)
        {
            errors.Add("• No compatible browser detected on this system.");
        }

        return errors;
    }

    private void SyncSessions()
    {
        Sessions.Clear();
        foreach (var s in _sessionManager.Sessions)
            Sessions.Add(s);
    }
}