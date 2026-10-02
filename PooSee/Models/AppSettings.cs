namespace PooSee.Models;

public sealed class AppSettings
{
    // Browser
    public bool AutoSelectBrowser { get; set; } = true;
    public string? PreferredBrowserChannel { get; set; }
    public string? PreferredBrowserExecutablePath { get; set; }

    // Profile location
    public string? ProfileLocation { get; set; }
    public bool CleanupProfilesAfterRun { get; set; } = true;
    public bool PersistentProfiles { get; set; } = false;

    // Default sessions
    public bool AutoRecommendSessions { get; set; } = true;
    public int ManualDefaultSessionCount { get; set; } = 4;
    public int MaximumSessionCount { get; set; } = 20;

    // Startup behavior
    public bool StartMinimized { get; set; } = false;
    public bool StartInTray { get; set; } = false;
    public bool RestorePreviousConfiguration { get; set; } = true;
    public bool RestorePreviousWindowState { get; set; } = true;

    // Appearance
    public string Theme { get; set; } = "System";
    public bool AlwaysOnTop { get; set; } = false;
    public bool MinimizeToTray { get; set; } = true;

    // Window state
    public double WindowWidth { get; set; } = 720;
    public double WindowHeight { get; set; } = 780;
    public double? WindowLeft { get; set; }
    public double? WindowTop { get; set; }
    public bool WindowMaximized { get; set; }

    // Last used config
    public List<UrlJob> LastJobs { get; set; } = new();
    public int LastSessionCount { get; set; } = 0;
}