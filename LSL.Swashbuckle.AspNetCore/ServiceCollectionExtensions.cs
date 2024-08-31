using System;
using System.Linq;
using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using LSL.Swashbuckle.AspNetCore.Configuration;
using LSL.Swashbuckle.AspNetCore.Filters;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

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
    /// <param name="swaggerGenconfigurator"></param>
    /// <param name="apiVersioningConfigurator"></param>
    /// <param name="apiExplorerConfigurator"></param>
    /// <returns></returns>
    public static IServiceCollection AddSwaggerGenWithVersioning(
        this IServiceCollection source,
        Action<SwaggerGenOptions>? swaggerGenconfigurator = null,
        Action<ApiVersioningOptions>? apiVersioningConfigurator = null,
        Action<ApiExplorerOptions>? apiExplorerConfigurator = null)
    {
        source.AddApiVersioning(c =>
        {
            c.DefaultApiVersion = new ApiVersion(1, 0);
            c.ApiVersionReader = new UrlSegmentApiVersionReader();
            c.ReportApiVersions = true;
            apiVersioningConfigurator?.Invoke(c);
        })
        .AddApiExplorer(c =>
        {
            c.GroupNameFormat = "'v'VVV";
            c.SubstituteApiVersionInUrl = true;
            apiExplorerConfigurator?.Invoke(c);
        });

        source.AddSwaggerGen(options =>
        {
            swaggerGenconfigurator?.Invoke(options);
        })
        .ConfigureOptions<ConfigureSwaggerOptions>();

        return source;
    }

    /// <summary>
    /// Add code version information to the service collection
    /// </summary>
    /// <param name="source"></param>
    /// <param name="assembly">The assembly whose AssemblyInformationalVersionAttribute will be queried for the required information</param>
    /// <param name="configurator">Optional configurator to potentially provide a commit URL for your source control provider</param>
    /// <returns></returns>
    public static IServiceCollection AddCodeVersionForAssembly(
        this IServiceCollection source,
        Assembly assembly,
        Action<AddCodeVersionOptions>? configurator = null)
    {
        source.Configure<CodeVersionOptions>(o =>
        {
            var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

            if (version == null) return;

            var options = new AddCodeVersionOptions();
            configurator?.Invoke(options);

            var commitHash = version.Split("+").ElementAtOrDefault(1) ?? string.Empty;
            o.Version = version;
            o.CommitHash = commitHash;
            o.CommitUrl = commitHash == null ? null : options.CommitUrlProvider?.Invoke(commitHash);
        })
        .AddSwaggerGen(c => c.DocumentFilter<CodeVersionDocumentFilter>());

        return source;
    }

    /// <summary>
    /// Add code version information to the service collection and to the open api document under Info with a new property called x-code-version
    /// </summary>
    /// <param name="source"></param>
    /// <param name="configurator">Optional configurator to potentially provide a commit URL for your source control provider</param>
    /// <returns></returns>
    public static IServiceCollection AddCodeVersionForAssemblyOf<T>(this IServiceCollection source, Action<AddCodeVersionOptions>? configurator = null) => 
        source.AddCodeVersionForAssembly(typeof(T).Assembly, configurator);
}