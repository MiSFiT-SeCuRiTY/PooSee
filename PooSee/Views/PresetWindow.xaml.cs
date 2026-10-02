using System.Windows;
using PooSee.ViewModels;

namespace PooSee.Views;

public partial class PresetWindow : Window
{
    public PresetWindow(PresetViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;

        vm.RequestClose = () => Close();
        vm.RequestNewName = ShowNameDialog;
    }

    private string? ShowNameDialog(string prompt)
    {
        var dlg = new NamePromptWindow(prompt) { Owner = this };
        return dlg.ShowDialog() == true ? dlg.EnteredName : null;
    }

    private void Close_Click(object sender, RoutedEventArgs e) => Close();
}