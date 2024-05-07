using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
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
    /// <returns></returns>
    /// <exception cref="ArgumentException">Thrown when the soure IApplicationBuilder does not implement IEndpointRouteBuilder</exception>
    public static IApplicationBuilder UseSwaggerUIWithVersioning(this IApplicationBuilder source, Action<SwaggerUIOptions>? configurator = null)
    {
        var apiVersions = ((source as IEndpointRouteBuilder)?.DescribeApiVersions()) ?? throw new ArgumentException("Provided IApplicationBuilder instance does not implemet IEndpointRouteBuilder", nameof(source));

        source.UseSwaggerUI(options =>
        {
            foreach (var apiVersion in apiVersions)
            {
                options.SwaggerEndpoint($"/swagger/{apiVersion.GroupName}/swagger.json", apiVersion.GroupName);
            }

            configurator?.Invoke(options);
        });

        return source;
    }
}
