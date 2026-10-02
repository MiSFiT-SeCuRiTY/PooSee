using System.IO;
using Microsoft.Playwright;
using PooSee.Models;

namespace PooSee.Services;

public sealed class BrowserService : IBrowserService
{
    private readonly ILogService _log;
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private BrowserInfo? _browserInfo;

    public bool IsRunning => _browser is not null && _browser.IsConnected;

    public BrowserService(ILogService log) => _log = log;

    public async Task StartAsync(BrowserInfo browserInfo, CancellationToken ct = default)
    {
        if (IsRunning) return;

        _browserInfo = browserInfo;
        _log.Info($"Starting Playwright. Browser: {browserInfo.Name} ({browserInfo.ExecutablePath})");

        // Point Playwright at the correct driver + browser cache locations.
        ConfigurePlaywrightEnvironment();

        _playwright = await Playwright.CreateAsync();

        var launchOptions = new BrowserTypeLaunchOptions
        {
            Headless = false,
            ExecutablePath = browserInfo.ExecutablePath,
            Args = new[]
            {
                "--no-default-browser-check",
                "--no-first-run"
            }
        };

        _browser = await _playwright.Chromium.LaunchAsync(launchOptions);
        _log.Info("Browser launched.");
    }

    private void ConfigurePlaywrightEnvironment()
    {
        try
        {
            var baseDir = AppContext.BaseDirectory;

            // 1. Playwright driver location (node + playwright.cmd inside .playwright)
            var driverDir = Path.Combine(baseDir, ".playwright");
            if (Directory.Exists(driverDir))
            {
                _log.Debug($"Playwright driver dir: {driverDir}");
                Environment.SetEnvironmentVariable("PLAYWRIGHT_DRIVER_PATH", driverDir);
            }
            else
            {
                _log.Warn($"Playwright driver directory not found at {driverDir}");
            }

            // 2. Playwright browser cache location (ms-playwright folder)
            // Default is %USERPROFILE%\AppData\Local\ms-playwright, which already works,
            // but we set it explicitly to avoid inherited env issues.
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PLAYWRIGHT_BROWSERS_PATH")))
            {
                var cache = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "ms-playwright");
                Environment.SetEnvironmentVariable("PLAYWRIGHT_BROWSERS_PATH", cache);
                _log.Debug($"Playwright browsers path: {cache}");
            }
        }
        catch (Exception ex)
        {
            _log.Warn($"Failed to configure Playwright env: {ex.Message}");
        }
    }

    public async Task<IBrowserContext> CreateContextAsync(string profileDirectory, CancellationToken ct = default)
    {
        if (_browser is null)
            throw new InvalidOperationException("Browser is not running.");

        var ctx = await _browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
            ViewportSize = new ViewportSize { Width = 1280, Height = 800 }
        });

        _log.Debug($"Context created for profile {profileDirectory}");
        return ctx;
    }

    public async Task StopAsync()
    {
        try
        {
            if (_browser is not null)
            {
                await _browser.CloseAsync();
                _browser = null;
            }
        }
        catch (Exception ex)
        {
            _log.Warn($"Browser close error: {ex.Message}");
        }
        finally
        {
            _browser = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await StopAsync();
        try { _playwright?.Dispose(); } catch { }
        _playwright = null;
        _browserInfo = null;
    }
}