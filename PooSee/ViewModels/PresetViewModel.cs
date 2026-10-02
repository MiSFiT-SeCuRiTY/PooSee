using System.Collections.ObjectModel;
using PooSee.Infrastructure;
using PooSee.Models;
using PooSee.Services;

namespace PooSee.ViewModels;

public sealed class PresetViewModel : ObservableObject
{
    private readonly IPresetManager _presets;

    private Preset? _selected;
    private string _newName = "";

    public ObservableCollection<Preset> Presets { get; } = new();

    public Preset? Selected
    {
        get => _selected;
        set { if (SetProperty(ref _selected, value)) OnPropertyChanged(nameof(HasSelection)); }
    }

    public bool HasSelection => _selected is not null;

    public string NewName { get => _newName; set => SetProperty(ref _newName, value); }

    public RelayCommand SaveCurrentAsPresetCommand { get; }
    public RelayCommand LoadSelectedCommand { get; }
    public RelayCommand RenameSelectedCommand { get; }
    public RelayCommand DeleteSelectedCommand { get; }

    // Delegates set by MainWindow when opening this window.
    public Func<IEnumerable<UrlJob>>? CurrentJobsProvider { get; set; }
    public Func<int>? CurrentSessionCountProvider { get; set; }
    public Action<IReadOnlyList<UrlJob>, int>? RequestLoadPreset { get; set; }
    public Action? RequestClose { get; set; }
    public Func<string, string?>? RequestNewName { get; set; }

    public PresetViewModel(IPresetManager presets)
    {
        _presets = presets;

        SaveCurrentAsPresetCommand = new RelayCommand(async () => await SaveCurrentAsPresetAsync());
        LoadSelectedCommand = new RelayCommand(LoadSelected);
        RenameSelectedCommand = new RelayCommand(async () => await RenameAsync());
        DeleteSelectedCommand = new RelayCommand(async () => await DeleteAsync());

        Reload();
    }

    public void Reload()
    {
        Presets.Clear();
        foreach (var p in _presets.GetAll())
            Presets.Add(p);
    }

    private async Task SaveCurrentAsPresetAsync()
    {
        var name = RequestNewName?.Invoke("Save preset as");
        if (string.IsNullOrWhiteSpace(name)) return;

        var jobs = CurrentJobsProvider?.Invoke() ?? Enumerable.Empty<UrlJob>();
        var count = CurrentSessionCountProvider?.Invoke() ?? 0;

        await _presets.CreateAsync(name, jobs, count);
        Reload();
    }

    private void LoadSelected()
    {
        if (Selected is null) return;
        RequestLoadPreset?.Invoke(Selected.Jobs, Selected.SessionCount);
        RequestClose?.Invoke();
    }

    private async Task RenameAsync()
    {
        if (Selected is null) return;
        var name = RequestNewName?.Invoke("Rename preset");
        if (string.IsNullOrWhiteSpace(name)) return;
        await _presets.RenameAsync(Selected.Id, name);
        Reload();
    }

    private async Task DeleteAsync()
    {
        if (Selected is null) return;
        await _presets.DeleteAsync(Selected.Id);
        Selected = null;
        Reload();
    }
}