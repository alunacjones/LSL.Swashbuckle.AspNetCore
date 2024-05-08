using System;
using System.Linq;
using System.Reflection;
using Asp.Versioning;
using LSL.Swashbuckle.AspNetCore.Configuration;
using LSL.Swashbuckle.AspNetCore.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.Swashbuckle.AspNetCore;

/// <summary>
/// ServiceCollectionExtensions
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Sets up Swashbuckle and Asp.Versioning to handle multiple api versions with common sense defaults
    /// </summary>
    /// <param name="source"></param>
    /// <param name="configurator"></param>
    /// <returns></returns>
    public static IServiceCollection AddSwaggerGenWithVersioning(this IServiceCollection source, Action<SwaggerGenWithVersioningOptions>? configurator = null)
    {
        source.AddApiVersioning(c =>
        {
            c.DefaultApiVersion = new ApiVersion(1, 0);
            c.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
        .AddApiExplorer(c =>
        {
            c.GroupNameFormat = "'v'VVV";
            c.SubstituteApiVersionInUrl = true;
        });

        var options = new SwaggerGenWithVersioningOptions();
        
        configurator?.Invoke(options);
        source.AddSwaggerGen();
        source.Configure<SwaggerGenWithVersioningOptions>(i =>
        {
            i.SwaggerGenOptionsConfigurators.AddRange(options.SwaggerGenOptionsConfigurators);
            i.Title = options.Title;
        });

        source.ConfigureOptions<ConfigureSwaggerOptions>();        

        return source;
    }

    /// <summary>
    /// Add code version information to the service collection
    /// </summary>
    /// <param name="source"></param>
    /// <param name="assembly">The assembly whose AssemblyInformationalVersionAttribute will be queried for the required information</param>
    /// <returns></returns>
    public static IServiceCollection AddCodeVersionForAssembly(this IServiceCollection source, Assembly assembly)
    {
        source.Configure<CodeVersionOptions>(o =>
        {
            var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

            if (version == null) return;

            o.Version = version;
            o.CommitHash = version.Split("+").ElementAtOrDefault(1) ?? string.Empty;
        });

        return source;
    }

    /// <summary>
    /// Add code version information to the service collection and to the open api document under Info with a new property called x-code-version
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IServiceCollection AddCodeVersionForAssemblyOf<T>(this IServiceCollection source)
    {
        source.Configure<CodeVersionOptions>(o =>
        {
            var version = typeof(T).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

            if (version == null) return;

            o.Version = version;
            o.CommitHash = version.Split("+").ElementAtOrDefault(1) ?? string.Empty;
        })
        .AddSwaggerGen(c => c.DocumentFilter<CodeVersionDocumentFilter>());

        return source;
    }    
}