namespace LSL.Swashbuckle.AspNetCore.Configurations;

internal class CodeVersionOptions
{
    public string Version { get; set; } = default!;
    public string CommitHash { get; set; } = default!;
}