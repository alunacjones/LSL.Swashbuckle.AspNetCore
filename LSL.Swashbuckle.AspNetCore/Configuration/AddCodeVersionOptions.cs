using System;

namespace LSL.Swashbuckle.AspNetCore.Configuration;

/// <summary>
/// Extra options for code version details
/// </summary>
public class AddCodeVersionOptions
{
    /// <summary>
    /// A provider that takes a commit hash and returns a URL to the actual commit on the provider's site
    /// </summary>
    public Func<string, string?> CommitUrlProvider { get; set; } = _ => null;
}
