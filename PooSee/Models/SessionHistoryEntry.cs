namespace PooSee.Models;

public sealed class SessionHistoryEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime StartedUtc { get; set; }
    public DateTime EndedUtc { get; set; }
    public int SessionCount { get; set; }
    public List<string> Urls { get; set; } = new();
    public int TotalRefreshes { get; set; }
    public int SuccessfulSessions { get; set; }
    public int FailedSessions { get; set; }
    public string? Notes { get; set; }
}

public sealed class SessionHistoryCollection
{
    public List<SessionHistoryEntry> Entries { get; set; } = new();
}