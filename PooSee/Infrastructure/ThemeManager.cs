using System.Windows;

namespace PooSee.Infrastructure;

public enum AppTheme { Dark, Light, System }

public static class ThemeManager
{
    public static void Apply(AppTheme mode)
    {
        var actual = mode == AppTheme.System ? DetectSystem() : mode;

        var dicts = System.Windows.Application.Current.Resources.MergedDictionaries;

        for (int i = dicts.Count - 1; i >= 0; i--)
        {
            var src = dicts[i].Source?.OriginalString ?? "";
            if (src.Contains("Colors.Light.xaml"))
                dicts.RemoveAt(i);
        }

        if (actual == AppTheme.Light)
        {
            try
            {
                var light = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/Resources/Colors.Light.xaml", UriKind.Absolute)
                };
                dicts.Add(light);
            }
            catch
            {
                // Colors.Light.xaml doesn't exist yet — safe to ignore.
            }
        }
    }

    private static AppTheme DetectSystem()
    {
        try
        {
            using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            var v = key?.GetValue("AppsUseLightTheme");
            return v is int i && i == 1 ? AppTheme.Light : AppTheme.Dark;
        }
        catch { return AppTheme.Dark; }
    }
}