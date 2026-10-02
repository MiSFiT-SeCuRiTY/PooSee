using PooSee.Infrastructure;
using PooSee.Models;

namespace PooSee.Services;

public sealed class PresetManager : IPresetManager
{
    private readonly AppPaths _paths;
    private readonly ILogService _log;
    private PresetCollection _collection = new();

    public PresetManager(AppPaths paths, ILogService log)
    {
        _paths = paths;
        _log = log;
    }

    public async Task LoadAsync(CancellationToken ct = default)
    {
        try
        {
            var loaded = await JsonFileStore.ReadAsync<PresetCollection>(_paths.PresetsFile, ct);
            if (loaded is not null) _collection = loaded;
        }
        catch (Exception ex)
        {
            _log.Warn($"Failed to load presets: {ex.Message}");
        }
    }

    public IReadOnlyList<Preset> GetAll() => _collection.Items;

    public async Task<Preset> CreateAsync(string name, IEnumerable<UrlJob> jobs, int sessionCount, CancellationToken ct = default)
    {
        var preset = new Preset
        {
            Name = name,
            Jobs = jobs.Select(j => j.Clone()).ToList(),
            SessionCount = sessionCount
        };
        _collection.Items.Add(preset);
        await SaveAsync(ct);
        return preset;
    }

    public async Task RenameAsync(Guid id, string newName, CancellationToken ct = default)
    {
        var preset = _collection.Items.FirstOrDefault(p => p.Id == id);
        if (preset is null) return;
        preset.Name = newName;
        preset.ModifiedUtc = DateTime.UtcNow;
        await SaveAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var preset = _collection.Items.FirstOrDefault(p => p.Id == id);
        if (preset is null) return;
        _collection.Items.Remove(preset);
        await SaveAsync(ct);
    }

    public async Task SaveAsync(CancellationToken ct = default)
    {
        try
        {
            await JsonFileStore.WriteAsync(_paths.PresetsFile, _collection, ct);
        }
        catch (Exception ex)
        {
            _log.Warn($"Failed to save presets: {ex.Message}");
        }
    }
}