namespace PooSee.Models;

public sealed class BrowserInfo
{
    public string Name { get; set; } = "";
    public string ExecutablePath { get; set; } = "";
    public string? Version { get; set; }
    public bool IsAvailable { get; set; }
    public string Channel { get; set; } = "";

    public override string ToString()
        => IsAvailable ? $"{Name} — {Version ?? "detected"}" : $"{Name} — not found";
}