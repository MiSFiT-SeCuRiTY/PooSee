using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PooSee.Services;
using PooSee.ViewModels;

namespace PooSee.Views;

public partial class MainWindow : Window
{
    private readonly MainViewModel _vm;
    private readonly IServiceProvider _services;
    private readonly ISettingsService _settings;

    public bool ForceClose { get; set; }

    public MainWindow(MainViewModel vm, IServiceProvider services, ISettingsService settings)
    {
        InitializeComponent();
        _vm = vm;
        _services = services;
        _settings = settings;

        DataContext = vm;

        vm.RequestEditUrl = ShowUrlEditor;
        vm.RequestOpenSettings = ShowSettings;
        vm.RequestOpenPresets = ShowPresets;
        vm.RequestOpenAbout = ShowAbout;

        Loaded += MainWindow_Loaded;
        Closing += MainWindow_Closing;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            await _vm.InitializeAsync();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Initialization failed: {ex.Message}",
                "PooSee", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            ToggleMaximize();
            return;
        }
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    private void Minimize_Click(object sender, RoutedEventArgs e)
        => WindowState = WindowState.Minimized;

    private void Maximize_Click(object sender, RoutedEventArgs e)
        => ToggleMaximize();

    private void ToggleMaximize()
        => WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;

    private void Close_Click(object sender, RoutedEventArgs e) => Close();

    private void AlwaysOnTop_Click(object sender, RoutedEventArgs e)
    {
        Topmost = AlwaysOnTopToggle.IsChecked == true;
    }

    private void MainWindow_Closing(object? sender, CancelEventArgs e)
    {
        if (!ForceClose && _settings.Current.MinimizeToTray)
        {
            e.Cancel = true;
            Hide();

            try
            {
                var tray = (ITrayService?)_services.GetService(typeof(ITrayService));
                tray?.Show();
                tray?.SetTooltip("PooSee — running");
            }
            catch { }

            return;
        }

        try
        {
            if (_settings.Current.RestorePreviousWindowState)
            {
                _settings.Current.WindowMaximized = WindowState == WindowState.Maximized;
                if (WindowState == WindowState.Normal)
                {
                    _settings.Current.WindowWidth = Width;
                    _settings.Current.WindowHeight = Height;
                    _settings.Current.WindowLeft = Left;
                    _settings.Current.WindowTop = Top;
                }
            }
            _settings.Current.AlwaysOnTop = Topmost;
            _ = _settings.SaveAsync();
        }
        catch { }
    }

    private void ShowUrlEditor(UrlJobViewModel? existing)
    {
        var dlg = new UrlJobWindow(existing?.Model) { Owner = this };
        if (dlg.ShowDialog() == true && dlg.Result is not null)
        {
            if (existing is null)
            {
                _vm.Jobs.Add(new UrlJobViewModel(dlg.Result));
            }
            else
            {
                var idx = _vm.Jobs.IndexOf(existing);
                if (idx >= 0)
                    _vm.Jobs[idx] = new UrlJobViewModel(dlg.Result);
            }
        }
    }

    private void ShowSettings()
    {
        var settingsVm = _services.GetRequiredService<SettingsViewModel>();
        var dlg = new SettingsWindow(settingsVm) { Owner = this };
        dlg.ShowDialog();
        _vm.DetectBrowser();
    }

    private void ShowPresets()
    {
        var presetVm = _services.GetRequiredService<PresetViewModel>();

        presetVm.CurrentJobsProvider = () => _vm.Jobs.Select(j => j.Model);
        presetVm.CurrentSessionCountProvider = () => _vm.SessionCount;

        presetVm.RequestLoadPreset = (jobs, sessionCount) =>
        {
            _vm.Jobs.Clear();
            foreach (var j in jobs)
                _vm.Jobs.Add(new UrlJobViewModel(j.Clone()));
            if (sessionCount > 0)
                _vm.SessionCount = sessionCount;
        };

        var dlg = new PresetWindow(presetVm) { Owner = this };
        dlg.ShowDialog();
    }

    private void ShowAbout()
    {
        var dlg = new AboutWindow { Owner = this };
        dlg.ShowDialog();
    }
}