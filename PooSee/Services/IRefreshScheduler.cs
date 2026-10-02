namespace PooSee.Services;

public interface IRefreshScheduler : IAsyncDisposable
{
    /// <summary>
    /// Start a scheduler loop for one session. The loop:
    /// 1. picks a random delay between min and max
    /// 2. waits with CancellationToken
    /// 3. invokes <paramref name="onRefresh"/> (which reloads the page)
    /// 4. repeats until cancellation.
    /// </summary>
    Task RunAsync(
        int sessionId,
        int minSeconds,
        int maxSeconds,
        Func<CancellationToken, Task> onRefresh,
        Action<DateTime> onNextRefreshScheduled,
        CancellationToken ct);
}