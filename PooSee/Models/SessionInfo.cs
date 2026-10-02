using PooSee.Infrastructure;

namespace PooSee.Models;

public enum SessionStatus { Idle, Starting, Running, Failed, Stopped }

public sealed class SessionInfo : ObservableObject
{
    private int _sessionId;
    private string _url = "";
    private SessionStatus _status = SessionStatus.Idle;
    private DateTime? _lastRefresh;
    private DateTime? _nextRefresh;
    private int _refreshCount;
    private string? _error;
    private string _browserName = "";

    public int SessionId { get => _sessionId; set => SetProperty(ref _sessionId, value); }
    public string Url { get => _url; set => SetProperty(ref _url, value); }

    public SessionStatus Status
    {
        get => _status;
        set { if (SetProperty(ref _status, value)) OnPropertyChanged(nameof(StatusText)); }
    }

    public DateTime? LastRefresh { get => _lastRefresh; set => SetProperty(ref _lastRefresh, value); }

    public DateTime? NextRefresh
    {
        get => _nextRefresh;
        set { if (SetProperty(ref _nextRefresh, value)) OnPropertyChanged(nameof(CountdownText)); }
    }

    public int RefreshCount { get => _refreshCount; set => SetProperty(ref _refreshCount, value); }
    public string? Error { get => _error; set => SetProperty(ref _error, value); }
    public string BrowserName { get => _browserName; set => SetProperty(ref _browserName, value); }

    public string StatusText => Status switch
    {
        SessionStatus.Idle => "Idle",
        SessionStatus.Starting => "Starting",
        SessionStatus.Running => "Running",
        SessionStatus.Failed => "Failed",
        SessionStatus.Stopped => "Stopped",
        _ => "?"
    };

    public string CountdownText
    {
        get
        {
            if (NextRefresh is null) return "";
            var remaining = NextRefresh.Value - DateTime.Now;
            if (remaining.TotalSeconds <= 0) return "0s";
            return $"{(int)remaining.TotalSeconds}s";
        }
    }

    public void Tick() => OnPropertyChanged(nameof(CountdownText));
}