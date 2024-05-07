using System;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LSL.Swashbuckle.AspNetCore.Configuration;

/// <summary>
/// Options for setting up versioning for swaggers
/// </summary>
public class SwaggerGenWithVersioningOptions
{
    /// <summary>
    /// Delegate for further configuring SwaggerGenOptions after versioning has been added
    /// </summary>
    public Action<SwaggerGenOptions>? SwaggerGenOptionsConfigurator { get; set; }

    /// <summary>
    /// The title for the API
    /// </summary>
    public string Title { get; set; } = Assembly.GetEntryAssembly()?.GetName().Name ?? "<Title Not Set>";
}