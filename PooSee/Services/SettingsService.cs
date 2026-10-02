using PooSee.Infrastructure;
using PooSee.Models;

namespace PooSee.Services;

public sealed class SettingsService : ISettingsService
{
    private readonly AppPaths _paths;
    private readonly ILogService _log;

    public AppSettings Current { get; private set; } = new();

    public SettingsService(AppPaths paths, ILogService log)
    {
        _paths = paths;
        _log = log;
    }

    public async Task LoadAsync(CancellationToken ct = default)
    {
        try
        {
            var loaded = await JsonFileStore.ReadAsync<AppSettings>(_paths.SettingsFile, ct);
            if (loaded is not null) Current = loaded;
        }
        catch (Exception ex)
        {
            _log.Warn($"Failed to load settings: {ex.Message}");
        }
    }

    public async Task SaveAsync(CancellationToken ct = default)
    {
        try
        {
            await JsonFileStore.WriteAsync(_paths.SettingsFile, Current, ct);
        }
        catch (Exception ex)
        {
            _log.Warn($"Failed to save settings: {ex.Message}");
        }
    }
}