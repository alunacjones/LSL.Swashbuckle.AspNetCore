using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace LSL.Swashbuckle.AspNetCore;

/// <summary>
/// SwaggerUIOptionsExtensions
/// </summary>
public static class SwaggerUIOptionsExtensions
{
    /// <summary>
    /// Sets the document title to "{AssemblyName} - Swagger UI"
    /// </summary>
    /// <param name="options"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static SwaggerUIOptions AddDocumentTitleFromAssembly(this SwaggerUIOptions options, Assembly assembly)
    {
        options.DocumentTitle = $"{assembly.GetName().Name} - Swagger UI";
        return options;
    }

    /// <summary>
    /// Sets the document title to "{AssemblyName of type T} - Swagger UI"
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public static SwaggerUIOptions AddDocumentTitleFromAssemblyOf<T>(this SwaggerUIOptions options) => 
        options.AddDocumentTitleFromAssembly(typeof(T).Assembly);
}