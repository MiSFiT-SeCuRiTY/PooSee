namespace PooSee.Services;

public interface ITrayService : IDisposable
{
    void Show();
    void Hide();
    void SetTooltip(string text);

    Action? OnShowRequested { get; set; }
    Action? OnStartRequested { get; set; }
    Action? OnStopRequested { get; set; }
    Action? OnSettingsRequested { get; set; }
    Action? OnExitRequested { get; set; }
}