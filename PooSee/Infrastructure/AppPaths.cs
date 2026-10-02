using System.IO;

namespace PooSee.Infrastructure;

public sealed class AppPaths
{
    public string Root { get; }
    public string Profiles { get; }
    public string Presets { get; }
    public string History { get; }
    public string Logs { get; }
    public string Settings { get; }

    public AppPaths(string? rootOverride = null)
    {
        Root = rootOverride ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "PooSee");

        Profiles = Path.Combine(Root, "Profiles");
        Presets = Path.Combine(Root, "Presets");
        History = Path.Combine(Root, "History");
        Logs = Path.Combine(Root, "Logs");
        Settings = Path.Combine(Root, "Settings");

        foreach (var dir in new[] { Root, Profiles, Presets, History, Logs, Settings })
            Directory.CreateDirectory(dir);
    }

    public string SettingsFile => Path.Combine(Settings, "appsettings.json");
    public string PresetsFile => Path.Combine(Presets, "presets.json");
    public string HistoryFile => Path.Combine(History, "history.json");
    public string LogFile => Path.Combine(Logs, $"poosee-{DateTime.Now:yyyy-MM-dd}.log");
}