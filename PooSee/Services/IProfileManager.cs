namespace PooSee.Services;

public interface IProfileManager
{
    string GetBaseDirectory();
    void SetBaseDirectory(string dir);
    string CreateProfile(int sessionId, bool persistent);
    string GetProfilePath(int sessionId, bool persistent);
    void DeleteProfile(int sessionId, bool persistent);
    void DeleteAllTemporaryProfiles();
    void CleanupAfterSession(int sessionId, bool persistent, bool cleanup);
}