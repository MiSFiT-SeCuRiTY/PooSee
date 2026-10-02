using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PooSee.Infrastructure;
using PooSee.Models;
using PooSee.Services;
using PooSee.ViewModels;

namespace PooSee;

public partial class App : System.Windows.Application
{
    public static IServiceProvider Services { get; private set; } = default!;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var paths = new AppPaths();
        var log = new LogService(paths.LogFile);
        log.Info("Application started");

        var services = new ServiceCollection();

        // Infra
        services.AddSingleton(paths);
        services.AddSingleton<ILogService>(log);

        // Services
        services.AddSingleton<ISystemResourceService, SystemResourceService>();
        services.AddSingleton<IBrowserDetector, BrowserDetector>();
        services.AddSingleton<IProfileManager, ProfileManager>();
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<IPresetManager, PresetManager>();
        services.AddSingleton<IHistoryService, HistoryService>();
        services.AddSingleton<ISessionDistributionStrategy, RoundRobinStrategy>();
        services.AddSingleton<IRefreshScheduler, RefreshScheduler>();
        services.AddSingleton<IBrowserService, BrowserService>();
        services.AddSingleton<ISessionManager, SessionManager>();
        services.AddSingleton<ITrayService, TrayService>();

        // Config factory
        services.AddSingleton<Func<SessionConfiguration>>(sp => () =>
        {
            var settings = sp.GetRequiredService<ISettingsService>().Current;
            var detector = sp.GetRequiredService<IBrowserDetector>();
            var browser = detector.DetectPreferred(
                settings.PreferredBrowserChannel,
                settings.PreferredBrowserExecutablePath);

            return new SessionConfiguration
            {
                BrowserExecutablePath = browser?.ExecutablePath,
                BrowserName = browser?.Name,
                ProfileLocation = settings.ProfileLocation,
                CleanupProfiles = settings.CleanupProfilesAfterRun,
                RandomIntervals = true
            };
        });

        // ViewModels
        services.AddSingleton<MainViewModel>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<PresetViewModel>();

        // Main window
        services.AddSingleton<Views.MainWindow>();

        Services = services.BuildServiceProvider();

        var settingsService = Services.GetRequiredService<ISettingsService>();
        await settingsService.LoadAsync();
        ThemeManager.Apply(ParseTheme(settingsService.Current.Theme));

        if (!string.IsNullOrWhiteSpace(settingsService.Current.ProfileLocation))
            Services.GetRequiredService<IProfileManager>().SetBaseDirectory(settingsService.Current.ProfileLocation);

        try
        {
            Services.GetRequiredService<IProfileManager>().DeleteAllTemporaryProfiles();
        }
        catch (Exception ex)
        {
            log.Warn($"Startup profile cleanup failed: {ex.Message}");
        }

        var main = Services.GetRequiredService<Views.MainWindow>();
        MainWindow = main;

        var tray = Services.GetRequiredService<ITrayService>();

        tray.OnShowRequested = () => main.Dispatcher.Invoke(() =>
        {
            main.Show();
            if (main.WindowState == WindowState.Minimized)
                main.WindowState = WindowState.Normal;
            main.Activate();
            tray.Hide();
        });

        tray.OnStartRequested = () => main.Dispatcher.Invoke(() =>
        {
            if (main.DataContext is MainViewModel vm && vm.StartCommand.CanExecute(null))
                vm.StartCommand.Execute(null);
        });

        tray.OnStopRequested = () => main.Dispatcher.Invoke(() =>
        {
            if (main.DataContext is MainViewModel vm && vm.StopCommand.CanExecute(null))
                vm.StopCommand.Execute(null);
        });

        tray.OnSettingsRequested = () => main.Dispatcher.Invoke(() =>
        {
            if (main.DataContext is MainViewModel vm && vm.OpenSettingsCommand.CanExecute(null))
                vm.OpenSettingsCommand.Execute(null);
        });

        tray.OnExitRequested = () => main.Dispatcher.Invoke(() =>
        {
            main.ForceClose = true;
            main.Close();
            Current.Shutdown();
        });

        if (settingsService.Current.StartInTray)
        {
            tray.Show();
        }
        else
        {
            main.Show();

            if (settingsService.Current.StartMinimized)
            {
                main.WindowState = WindowState.Minimized;
                tray.Show();
            }
        }
    }

    protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
    {
        try
        {
            var tray = Services.GetService(typeof(ITrayService)) as ITrayService;
            tray?.Dispose();
        }
        catch { }

        base.OnSessionEnding(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        try
        {
            var sessionMgr = Services.GetService(typeof(ISessionManager)) as ISessionManager;
            sessionMgr?.StopAllAsync().GetAwaiter().GetResult();
        }
        catch { }

        try
        {
            var profileMgr = Services.GetService(typeof(IProfileManager)) as IProfileManager;
            profileMgr?.DeleteAllTemporaryProfiles();
        }
        catch { }

        try
        {
            var tray = Services.GetService(typeof(ITrayService)) as ITrayService;
            tray?.Dispose();
        }
        catch { }

        try
        {
            var log = Services.GetService(typeof(ILogService)) as LogService;
            log?.Info("Application exiting");
            log?.Dispose();
        }
        catch { }

        base.OnExit(e);
    }

    private static AppTheme ParseTheme(string s) => s switch
    {
        "Dark" => AppTheme.Dark,
        "Light" => AppTheme.Light,
        _ => AppTheme.System
    };
}