using PooSee.Infrastructure;
using PooSee.Models;

namespace PooSee.ViewModels;

public sealed class UrlJobViewModel : ObservableObject
{
    private UrlJob _model;

    public UrlJobViewModel(UrlJob model) => _model = model;

    public UrlJob Model => _model;

    public Guid Id => _model.Id;

    public string Name
    {
        get => _model.Name;
        set { _model.Name = value; OnPropertyChanged(); }
    }

    public string Url
    {
        get => _model.Url;
        set { _model.Url = value; OnPropertyChanged(); }
    }

    public int MinimumIntervalSeconds
    {
        get => _model.MinimumIntervalSeconds;
        set { _model.MinimumIntervalSeconds = value; OnPropertyChanged(); }
    }

    public int MaximumIntervalSeconds
    {
        get => _model.MaximumIntervalSeconds;
        set { _model.MaximumIntervalSeconds = value; OnPropertyChanged(); }
    }

    public bool Enabled
    {
        get => _model.Enabled;
        set { _model.Enabled = value; OnPropertyChanged(); }
    }

    public string DisplayTitle =>
        string.IsNullOrWhiteSpace(Name) ? _model.Url : $"{Name} — {_model.Url}";

    public string IntervalText => $"{MinimumIntervalSeconds}–{MaximumIntervalSeconds}s";

    public void Refresh() => OnPropertyChanged(string.Empty);
}