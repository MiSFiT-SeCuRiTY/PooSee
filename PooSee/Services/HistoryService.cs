using PooSee.Infrastructure;
using PooSee.Models;

namespace PooSee.Services;

public sealed class HistoryService : IHistoryService
{
    private readonly AppPaths _paths;
    private readonly ILogService _log;
    private SessionHistoryCollection _collection = new();

    public HistoryService(AppPaths paths, ILogService log)
    {
        _paths = paths;
        _log = log;
    }

    public async Task LoadAsync(CancellationToken ct = default)
    {
        try
        {
            var loaded = await JsonFileStore.ReadAsync<SessionHistoryCollection>(_paths.HistoryFile, ct);
            if (loaded is not null) _collection = loaded;
        }
        catch (Exception ex)
        {
            _log.Warn($"Failed to load history: {ex.Message}");
        }
    }

    public IReadOnlyList<SessionHistoryEntry> GetAll() => _collection.Entries;

    public async Task RecordAsync(SessionHistoryEntry entry, CancellationToken ct = default)
    {
        _collection.Entries.Add(entry);

        // Keep last 200 entries.
        const int max = 200;
        if (_collection.Entries.Count > max)
            _collection.Entries.RemoveRange(0, _collection.Entries.Count - max);

        try
        {
            await JsonFileStore.WriteAsync(_paths.HistoryFile, _collection, ct);
        }
        catch (Exception ex)
        {
            _log.Warn($"Failed to save history: {ex.Message}");
        }
    }
}