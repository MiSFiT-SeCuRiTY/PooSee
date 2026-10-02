namespace PooSee.Models;

public sealed class UrlJob
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Url { get; set; } = "";
    public int MinimumIntervalSeconds { get; set; } = 30;
    public int MaximumIntervalSeconds { get; set; } = 60;
    public bool Enabled { get; set; } = true;

    public UrlJob Clone() => new()
    {
        Id = Guid.NewGuid(),
        Name = Name,
        Url = Url,
        MinimumIntervalSeconds = MinimumIntervalSeconds,
        MaximumIntervalSeconds = MaximumIntervalSeconds,
        Enabled = Enabled
    };
}