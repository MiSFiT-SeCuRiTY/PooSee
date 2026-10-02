namespace PooSee.Services;

public sealed class RefreshScheduler : IRefreshScheduler
{
    private readonly ILogService _log;
    private readonly Random _rng = new();

    public RefreshScheduler(ILogService log) => _log = log;

    public async Task RunAsync(
        int sessionId,
        int minSeconds,
        int maxSeconds,
        Func<CancellationToken, Task> onRefresh,
        Action<DateTime> onNextRefreshScheduled,
        CancellationToken ct)
    {
        if (minSeconds < 1) minSeconds = 1;
        if (maxSeconds < minSeconds) maxSeconds = minSeconds;

        try
        {
            // Small stagger so N sessions don't all fire simultaneously at t=0.
            var firstDelay = TimeSpan.FromSeconds(1 + _rng.NextDouble() * 2);

            while (!ct.IsCancellationRequested)
            {
                var delaySeconds = minSeconds == maxSeconds
                    ? minSeconds
                    : minSeconds + _rng.NextDouble() * (maxSeconds - minSeconds);

                var fireAt = DateTime.Now.AddSeconds(delaySeconds);
                onNextRefreshScheduled(fireAt);

                var wait = TimeSpan.FromSeconds(delaySeconds) + (firstDelay - TimeSpan.FromSeconds(1));

                await Task.Delay(wait, ct).ConfigureAwait(false);
                firstDelay = TimeSpan.Zero;

                if (ct.IsCancellationRequested) break;

                try
                {
                    await onRefresh(ct).ConfigureAwait(false);
                }
                catch (OperationCanceledException) { break; }
                catch (Exception ex)
                {
                    _log.Warn($"Session {sessionId:D2} refresh error: {ex.Message}");
                }
            }
        }
        catch (OperationCanceledException) { /* normal */ }
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}