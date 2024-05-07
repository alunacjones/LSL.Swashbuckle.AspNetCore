using System;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LSL.Swashbuckle.AspNetCore.Configuration;

/// <summary>
/// SwaggerGenWithVersioningOptionsExtensions
/// </summary>
public static class SwaggerGenWithVersioningOptionsExtensions
{
    /// <summary>
    /// Use the name of the assembly containg the given type as the API title
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static SwaggerGenWithVersioningOptions WithTitleFromAssemblyOf<T>(this SwaggerGenWithVersioningOptions source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        source.Title = typeof(T).Assembly.GetName().Name!;
        return source;
    }

    /// <summary>
    /// Adds a delegate for configuraing SwaggerGenOPtions
    /// </summary>
    /// <param name="source"></param>
    /// <param name="configurator"></param>
    /// <returns></returns>
    public static SwaggerGenWithVersioningOptions WithSwaggerGenOptionsConfigurator(this SwaggerGenWithVersioningOptions source, Action<SwaggerGenOptions> configurator)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(configurator);

        source.SwaggerGenOptionsConfigurators.Add(configurator);

        return source;
    }
}