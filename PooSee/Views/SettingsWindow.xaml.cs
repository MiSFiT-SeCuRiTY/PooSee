using System.Windows;
using PooSee.ViewModels;

namespace PooSee.Views;

public partial class SettingsWindow : Window
{
    public SettingsWindow(SettingsViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
        vm.RequestClose = () => { DialogResult = true; Close(); };
        vm.RequestBrowseProfile = BrowseProfile;
    }

    private void BrowseProfile()
    {
        var dlg = new Microsoft.Win32.OpenFolderDialog();
        if (dlg.ShowDialog() == true)
        {
            ((SettingsViewModel)DataContext).Model.ProfileLocation = dlg.FolderName;
        }
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}