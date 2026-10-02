using System.Collections.ObjectModel;
using PooSee.Infrastructure;
using PooSee.Models;
using PooSee.Services;

namespace PooSee.ViewModels;

public sealed class SettingsViewModel : ObservableObject
{
    private readonly ISettingsService _settings;
    private readonly IBrowserDetector _detector;

    public AppSettings Model => _settings.Current;

    public ObservableCollection<BrowserInfo> DetectedBrowsers { get; } = new();

    public RelayCommand SaveCommand { get; }
    public RelayCommand BrowseProfileCommand { get; }

    public Action? RequestClose { get; set; }
    public Action? RequestBrowseProfile { get; set; }

    public SettingsViewModel(ISettingsService settings, IBrowserDetector detector)
    {
        _settings = settings;
        _detector = detector;

        foreach (var b in _detector.DetectAll())
            DetectedBrowsers.Add(b);

        SaveCommand = new RelayCommand(async () => await SaveAsync());
        BrowseProfileCommand = new RelayCommand(() => RequestBrowseProfile?.Invoke());
    }

    public async Task SaveAsync()
    {
        await _settings.SaveAsync();
        ThemeManager.Apply(ParseTheme(_settings.Current.Theme));
        RequestClose?.Invoke();
    }

    private static AppTheme ParseTheme(string s) => s switch
    {
        "Dark" => AppTheme.Dark,
        "Light" => AppTheme.Light,
        _ => AppTheme.System
    };
}