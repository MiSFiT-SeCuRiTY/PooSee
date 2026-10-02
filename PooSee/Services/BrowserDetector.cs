using System.IO;
using Microsoft.Win32;
using PooSee.Models;

namespace PooSee.Services;

public sealed class BrowserDetector : IBrowserDetector
{
    private readonly ILogService _log;
    public BrowserDetector(ILogService log) => _log = log;

    public IReadOnlyList<BrowserInfo> DetectAll()
    {
        return new List<BrowserInfo>
        {
            DetectChrome(),
            DetectEdge(),
            DetectChromium()
        };
    }

    public BrowserInfo? DetectPreferred(string? preferredChannel = null, string? explicitPath = null)
    {
        if (!string.IsNullOrWhiteSpace(explicitPath) && File.Exists(explicitPath))
            return new BrowserInfo
            {
                Name = "Custom",
                ExecutablePath = explicitPath,
                IsAvailable = true,
                Channel = "chrome"
            };

        var all = DetectAll();

        if (!string.IsNullOrWhiteSpace(preferredChannel))
        {
            var match = all.FirstOrDefault(b => b.IsAvailable &&
                string.Equals(b.Channel, preferredChannel, StringComparison.OrdinalIgnoreCase));
            if (match is not null) return match;
        }

        return all.FirstOrDefault(b => b.IsAvailable);
    }

    private BrowserInfo DetectChrome()
    {
        var candidates = new[]
        {
            ReadRegistryPath(RegistryHive.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe"),
            ReadRegistryPath(RegistryHive.CurrentUser, @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Google", "Chrome", "Application", "chrome.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Google", "Chrome", "Application", "chrome.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google", "Chrome", "Application", "chrome.exe")
        };
        return Build("Google Chrome", "chrome", candidates);
    }

    private BrowserInfo DetectEdge()
    {
        var candidates = new[]
        {
            ReadRegistryPath(RegistryHive.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\msedge.exe"),
            ReadRegistryPath(RegistryHive.CurrentUser, @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\msedge.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft", "Edge", "Application", "msedge.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft", "Edge", "Application", "msedge.exe")
        };
        return Build("Microsoft Edge", "msedge", candidates);
    }

    private BrowserInfo DetectChromium()
    {
        var candidates = new[]
        {
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Chromium", "Application", "chrome.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Chromium", "Application", "chrome.exe")
        };
        return Build("Chromium", "chromium", candidates);
    }

    private static BrowserInfo Build(string name, string channel, IEnumerable<string?> candidates)
    {
        foreach (var c in candidates)
        {
            if (string.IsNullOrWhiteSpace(c) || !File.Exists(c)) continue;
            try
            {
                var version = System.Diagnostics.FileVersionInfo.GetVersionInfo(c).FileVersion;
                return new BrowserInfo
                {
                    Name = name,
                    ExecutablePath = c!,
                    Version = version,
                    IsAvailable = true,
                    Channel = channel
                };
            }
            catch { }
        }
        return new BrowserInfo { Name = name, Channel = channel, IsAvailable = false };
    }

    private static string? ReadRegistryPath(RegistryHive hive, string subKey)
    {
        try
        {
            using var baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default);
            using var key = baseKey.OpenSubKey(subKey);
            return key?.GetValue(null) as string;
        }
        catch { return null; }
    }
}