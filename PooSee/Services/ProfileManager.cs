using System.IO;
using PooSee.Infrastructure;

namespace PooSee.Services;

public sealed class ProfileManager : IProfileManager
{
    private readonly AppPaths _paths;
    private readonly ILogService _log;
    private string _baseProfilesDir;

    public ProfileManager(AppPaths paths, ILogService log)
    {
        _paths = paths;
        _log = log;
        _baseProfilesDir = paths.Profiles;
    }

    public string GetBaseDirectory() => _baseProfilesDir;

    public void SetBaseDirectory(string dir)
    {
        if (!string.IsNullOrWhiteSpace(dir))
        {
            Directory.CreateDirectory(dir);
            _baseProfilesDir = dir;
        }
    }

    private string GetRoot(bool persistent)
        => Path.Combine(_baseProfilesDir, persistent ? "persistent" : "temp");

    public string GetProfilePath(int sessionId, bool persistent)
        => Path.Combine(GetRoot(persistent), $"session-{sessionId:D3}");

    public string CreateProfile(int sessionId, bool persistent)
    {
        var path = GetProfilePath(sessionId, persistent);
        Directory.CreateDirectory(path);
        _log.Debug($"Profile created: {path}");
        return path;
    }

    public void DeleteProfile(int sessionId, bool persistent)
    {
        var path = GetProfilePath(sessionId, persistent);
        TryDeleteDirectory(path);
    }

    public void DeleteAllTemporaryProfiles()
    {
        TryDeleteDirectory(GetRoot(persistent: false));
    }

    public void CleanupAfterSession(int sessionId, bool persistent, bool cleanup)
    {
        if (!cleanup) return;
        if (persistent) return;
        DeleteProfile(sessionId, persistent: false);
    }

    private void TryDeleteDirectory(string dir)
    {
        if (!Directory.Exists(dir)) return;

        var full = Path.GetFullPath(dir);
        var root = Path.GetFullPath(_baseProfilesDir);
        if (!full.StartsWith(root, StringComparison.OrdinalIgnoreCase))
        {
            _log.Warn($"Refusing to delete outside profile root: {full}");
            return;
        }

        for (int attempt = 0; attempt < 4; attempt++)
        {
            try { Directory.Delete(dir, recursive: true); return; }
            catch (IOException) { Thread.Sleep(150); }
            catch (UnauthorizedAccessException) { Thread.Sleep(150); }
        }
        _log.Warn($"Failed to remove profile directory: {dir}");
    }
}