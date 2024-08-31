using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace LSL.Swashbuckle.AspNetCore;

/// <summary>
/// ApplicationBuilderExtensions
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Register SwaggerUI with mutiple API versions 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="configurator"></param>
    /// <param name="swaggerEndpointPathBase"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Thrown when the soure IApplicationBuilder does not implement IEndpointRouteBuilder</exception>
    public static IApplicationBuilder UseSwaggerUIWithVersioning(this IApplicationBuilder source, Action<SwaggerUIOptions>? configurator = null, string? swaggerEndpointPathBase = null)
    {
        var apiVersions = ((source as IEndpointRouteBuilder)?.DescribeApiVersions()) ?? throw new ArgumentException("Provided IApplicationBuilder instance does not implemet IEndpointRouteBuilder", nameof(source));

        source.UseSwaggerUI(options =>
        {    
            configurator?.Invoke(options);        
            swaggerEndpointPathBase = string.IsNullOrEmpty(swaggerEndpointPathBase) ? string.Empty : $"/{swaggerEndpointPathBase}";
            
            using var scope = source.ApplicationServices.CreateScope();

            var internalOptions = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<InternalOptions>>().Value;
            
            foreach (var apiVersion in apiVersions)
            {
                var url = $"{swaggerEndpointPathBase}/{options.RoutePrefix}/{apiVersion.GroupName}/swagger.json";
                internalOptions.SwaggerDocUrls.Add(apiVersion.GroupName, url);
                options.SwaggerEndpoint(url, apiVersion.GroupName);
            }
        });

        return source;
    }

    internal class InternalOptions
    {
        public Dictionary<string, string> SwaggerDocUrls = new();
    }
}
