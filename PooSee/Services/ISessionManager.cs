using PooSee.Models;

namespace PooSee.Services;

public interface ISessionManager : IAsyncDisposable
{
    bool IsRunning { get; }
    IReadOnlyList<SessionInfo> Sessions { get; }
    event EventHandler? SessionsChanged;

    Task StartAsync(SessionConfiguration configuration, CancellationToken ct = default);
    Task StopAllAsync();
}