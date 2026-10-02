using System.Windows;
using PooSee.Models;

namespace PooSee.Views;

public partial class UrlJobWindow : Window
{
    public UrlJob? Result { get; private set; }

    private Guid? _existingId;

    public UrlJobWindow(UrlJob? existing = null)
    {
        InitializeComponent();

        if (existing is not null)
        {
            HeaderText.Text = "Edit URL Job";
            UrlBox.Text = existing.Url;
            NameBox.Text = existing.Name;
            MinBox.Text = existing.MinimumIntervalSeconds.ToString();
            MaxBox.Text = existing.MaximumIntervalSeconds.ToString();
            EnabledBox.IsChecked = existing.Enabled;
            _existingId = existing.Id;
        }

        Loaded += (_, _) =>
        {
            UrlBox.Focus();
            UrlBox.SelectAll();
        };
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var errors = new List<string>();

        var url = UrlBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(url) ||
            !Uri.TryCreate(url, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            errors.Add("Please enter a valid HTTP or HTTPS URL.");
        }

        if (!int.TryParse(MinBox.Text, out var min) || min < 1)
            errors.Add("Minimum interval must be a whole number of seconds (1 or more).");

        if (!int.TryParse(MaxBox.Text, out var max) || max < 1)
            errors.Add("Maximum interval must be a whole number of seconds (1 or more).");

        if (errors.Count == 0 && min > max)
            errors.Add("Minimum interval must be less than or equal to maximum interval.");

        if (errors.Count > 0)
        {
            ErrorText.Text = string.Join(Environment.NewLine, errors);
            ErrorText.Visibility = Visibility.Visible;
            return;
        }

        Result = new UrlJob
        {
            Id = _existingId ?? Guid.NewGuid(),
            Url = url,
            Name = NameBox.Text.Trim(),
            MinimumIntervalSeconds = min,
            MaximumIntervalSeconds = max,
            Enabled = EnabledBox.IsChecked == true
        };

        DialogResult = true;
        Close();
    }
}