using System.Windows;

namespace PooSee.Views;

public partial class NamePromptWindow : Window
{
    public string? EnteredName { get; private set; }

    public NamePromptWindow(string prompt)
    {
        InitializeComponent();
        PromptText.Text = prompt;

        Loaded += (_, _) =>
        {
            InputBox.Focus();
            InputBox.SelectAll();
        };
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void Ok_Click(object sender, RoutedEventArgs e)
    {
        var t = InputBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(t))
        {
            DialogResult = false;
            Close();
            return;
        }

        EnteredName = t;
        DialogResult = true;
        Close();
    }
}