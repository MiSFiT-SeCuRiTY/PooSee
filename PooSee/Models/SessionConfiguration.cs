namespace PooSee.Models;

public sealed class SessionConfiguration
{
    public int SessionCount { get; set; }
    public List<UrlJob> Jobs { get; set; } = new();
    public string? BrowserExecutablePath { get; set; }
    public string? BrowserName { get; set; }
    public string? ProfileLocation { get; set; }
    public bool CleanupProfiles { get; set; } = true;
    public bool RandomIntervals { get; set; } = true;
}