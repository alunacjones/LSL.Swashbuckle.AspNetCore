using System;
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
}