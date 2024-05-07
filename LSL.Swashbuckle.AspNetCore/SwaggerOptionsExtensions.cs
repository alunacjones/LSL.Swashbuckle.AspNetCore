using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Extensions;
using Swashbuckle.AspNetCore.Swagger;

namespace LSL.Swashbuckle.AspNetCore;

/// <summary>
/// SwaggerOptionsExtensions
/// </summary>
public static class SwaggerOptionsExtensions
{
    /// <summary>
    /// Adds an extra option under info called "x-code-version" to supply version info from the supplied assembly
    /// </summary>
    /// <param name="source"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static SwaggerOptions AddCodeVersionInfo(this SwaggerOptions source, Assembly assembly)
    {
        source.PreSerializeFilters.Add((swagger, _) =>
        {
            (bool wasFound, string version, string commitHash) = GetVersionInfo(assembly);

            if (!wasFound) return;

            swagger.Info.AddExtension("x-code-version", new OpenApiObject
            {
                ["version"] = new OpenApiString(version),
                ["commit-hash"] = new OpenApiString(commitHash),
            });
        });

        return source;
    }

    /// <summary>
    /// Adds an extra option under info called "x-code-version" to supply version info from the supplied type's assembly
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static SwaggerOptions AddCodeVersionInfoForAssemblyOf<T>(this SwaggerOptions source) => 
        source.AddCodeVersionInfo(typeof(T).Assembly);

    /// <summary>
    /// Add the code version to the Api Description
    /// </summary>
    /// <param name="source"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static SwaggerOptions AddCodeVersionToApiDescription(this SwaggerOptions source, Assembly assembly)
    {
        (bool wasFound, string version, string commitHash) = GetVersionInfo(assembly);
        
        if (!wasFound) return source;

        source.PreSerializeFilters.Add((swagger, _) =>
        {
            swagger.Info.Description = $"<code>API Code Version {version}</code>";
        });

        return source;
    }

    /// <summary>
    /// Add the code version to the Api Description
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static SwaggerOptions AddCodeVersionToApiDescriptionOf<T>(this SwaggerOptions source) => 
        source.AddCodeVersionToApiDescription(typeof(T).Assembly);

    internal static (bool wasFound, string version, string commitHash) GetVersionInfo(Assembly assembly)
    {
        var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

        if (version == null) return (false, string.Empty, string.Empty);

        return (true, version, version.Split("+").ElementAtOrDefault(1) ?? string.Empty);
    }
}
