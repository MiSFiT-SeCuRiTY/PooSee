using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace PooSee.Views;

public partial class AboutWindow : Window
{
    public AboutWindow() => InitializeComponent();

    private void Close_Click(object sender, RoutedEventArgs e) => Close();

    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        try
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
        catch { }
    }
}