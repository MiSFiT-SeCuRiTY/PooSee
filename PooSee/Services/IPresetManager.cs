using PooSee.Models;

namespace PooSee.Services;

public interface IPresetManager
{
    Task LoadAsync(CancellationToken ct = default);
    IReadOnlyList<Preset> GetAll();
    Task<Preset> CreateAsync(string name, IEnumerable<UrlJob> jobs, int sessionCount, CancellationToken ct = default);
    Task RenameAsync(Guid id, string newName, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
    Task SaveAsync(CancellationToken ct = default);
}