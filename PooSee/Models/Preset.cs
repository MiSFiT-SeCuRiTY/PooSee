namespace PooSee.Models;

public sealed class Preset
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedUtc { get; set; } = DateTime.UtcNow;
    public List<UrlJob> Jobs { get; set; } = new();
    public int SessionCount { get; set; } = 0;
}

public sealed class PresetCollection
{
    public List<Preset> Items { get; set; } = new();
}