using PooSee.Models;

namespace PooSee.Services;

public interface IBrowserDetector
{
    IReadOnlyList<BrowserInfo> DetectAll();
    BrowserInfo? DetectPreferred(string? preferredChannel = null, string? explicitPath = null);
}