using PooSee.Models;

namespace PooSee.Services;

/// <summary>
/// Deterministic round-robin distribution.
/// Session 0 → jobs[0], Session 1 → jobs[1], ... Session N → jobs[N % count].
/// This gives a stable, understandable 1-per-job spread when sessions >= jobs.
/// </summary>
public sealed class RoundRobinStrategy : ISessionDistributionStrategy
{
    public IReadOnlyList<UrlJob> Distribute(int sessionCount, IReadOnlyList<UrlJob> jobs)
    {
        if (sessionCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(sessionCount));

        var enabled = jobs.Where(j => j.Enabled).ToList();
        if (enabled.Count == 0)
            throw new InvalidOperationException("No enabled URL jobs to distribute.");

        var result = new List<UrlJob>(sessionCount);
        for (int i = 0; i < sessionCount; i++)
            result.Add(enabled[i % enabled.Count]);
        return result;
    }
}