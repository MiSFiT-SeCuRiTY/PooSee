using PooSee.Models;

namespace PooSee.Services;

public interface ISessionDistributionStrategy
{
    /// <summary>
    /// Assign each session to a URL job.
    /// Returns an array of length sessionCount where each element is the
    /// UrlJob that session should load (never null).
    /// </summary>
    IReadOnlyList<UrlJob> Distribute(int sessionCount, IReadOnlyList<UrlJob> jobs);
}