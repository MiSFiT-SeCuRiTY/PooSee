using Microsoft.Playwright;
using PooSee.Models;

namespace PooSee.Services;

public interface IBrowserService : IAsyncDisposable
{
    bool IsRunning { get; }

    /// <summary>
    /// Launch the persistent (shared) browser process. Playwright contexts
    /// opened later will each have isolated cookies/storage.
    /// </summary>
    Task StartAsync(BrowserInfo browserInfo, CancellationToken ct = default);

    /// <summary>
    /// Create an isolated incognito-like context with its own user data dir.
    /// </summary>
    Task<IBrowserContext> CreateContextAsync(string profileDirectory, CancellationToken ct = default);

    Task StopAsync();
}