using System.IO;

namespace PooSee.Services;

public sealed class TrayService : ITrayService
{
    private readonly System.Windows.Forms.NotifyIcon _icon;
    private readonly ILogService _log;

    public Action? OnShowRequested { get; set; }
    public Action? OnStartRequested { get; set; }
    public Action? OnStopRequested { get; set; }
    public Action? OnSettingsRequested { get; set; }
    public Action? OnExitRequested { get; set; }

    public TrayService(ILogService log)
    {
        _log = log;

        var menu = new System.Windows.Forms.ContextMenuStrip();
        menu.Items.Add("Show", null, (_, _) => OnShowRequested?.Invoke());
        menu.Items.Add(new System.Windows.Forms.ToolStripSeparator());
        menu.Items.Add("Start", null, (_, _) => OnStartRequested?.Invoke());
        menu.Items.Add("Stop All", null, (_, _) => OnStopRequested?.Invoke());
        menu.Items.Add(new System.Windows.Forms.ToolStripSeparator());
        menu.Items.Add("Settings", null, (_, _) => OnSettingsRequested?.Invoke());
        menu.Items.Add("Exit", null, (_, _) => OnExitRequested?.Invoke());

        _icon = new System.Windows.Forms.NotifyIcon
        {
            Text = "PooSee",
            Visible = false,
            ContextMenuStrip = menu,
            Icon = LoadAppIcon()
        };

        _icon.DoubleClick += (_, _) => OnShowRequested?.Invoke();
    }

    private static System.Drawing.Icon LoadAppIcon()
    {
        try
        {
            // In single-file publish, Assembly.Location is empty.
            // Process path is reliable in both modes.
            var exe = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;

            if (string.IsNullOrEmpty(exe) || !File.Exists(exe))
            {
                var name = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name ?? "PooSee";
                exe = Path.Combine(AppContext.BaseDirectory, name + ".exe");
            }

            if (File.Exists(exe))
            {
                var extracted = System.Drawing.Icon.ExtractAssociatedIcon(exe);
                if (extracted is not null) return extracted;
            }
        }
        catch { }

        return System.Drawing.SystemIcons.Application;
    }

    public void Show()
    {
        _icon.Visible = true;
        _log.Debug("Tray icon shown");
    }

    public void Hide()
    {
        _icon.Visible = false;
        _log.Debug("Tray icon hidden");
    }

    public void SetTooltip(string text)
    {
        if (string.IsNullOrEmpty(text)) text = "PooSee";
        if (text.Length > 62) text = text[..62];
        _icon.Text = text;
    }

    public void Dispose()
    {
        try
        {
            _icon.Visible = false;
            _icon.Dispose();
        }
        catch { }
    }
}