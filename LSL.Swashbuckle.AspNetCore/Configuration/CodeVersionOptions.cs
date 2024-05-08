namespace LSL.Swashbuckle.AspNetCore.Configuration;

internal class CodeVersionOptions
{
    public string Version { get; set; } = default!;
    public string CommitHash { get; set; } = default!;
}
