using PooSee.Models;

namespace PooSee.Services;

public interface IHistoryService
{
    Task LoadAsync(CancellationToken ct = default);
    Task RecordAsync(SessionHistoryEntry entry, CancellationToken ct = default);
    IReadOnlyList<SessionHistoryEntry> GetAll();
}