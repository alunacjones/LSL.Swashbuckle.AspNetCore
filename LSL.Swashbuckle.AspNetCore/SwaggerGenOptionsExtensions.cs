using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LSL.Swashbuckle.AspNetCore;

/// <summary>
/// SwaggergenOptionsExtensions
/// </summary>
public static class SwaggerGenOptionsExtensions
{
    /// <summary>
    /// Quicker method for adding Xml Comments to the generated Open Api document
    /// </summary>
    /// <param name="source"></param>
    /// <param name="assembly">The assembly whose nae will be the base for the xml file to find</param>
    /// <returns></returns>
    public static SwaggerGenOptions AddXmlCommentsForAssembly(this SwaggerGenOptions source, Assembly assembly)
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml");
        source.IncludeXmlComments(filePath, includeControllerXmlComments: true);
        
        return source;
    }

    /// <summary>
    /// Quicker method for adding Xml Comments to the generated Open Api document via a given type
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static SwaggerGenOptions AddXmlCommentsForAssemblyOf<T>(this SwaggerGenOptions source) => 
        source.AddXmlCommentsForAssembly(typeof(T).Assembly);

    /// <summary>
    /// Adds a schema filter that ensures all enum values generate string values for the open api specification
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static SwaggerGenOptions AddStringEnumFilter(this SwaggerGenOptions source)
    {
        source.SchemaFilter<EnumSchemaFilter>();
        return source;
    }

    /// <summary>
    /// Add the code version to the Api Description
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static SwaggerGenOptions AddCodeVersionToApiDescription(this SwaggerGenOptions source)
    {
        source.DocumentFilter<CodeVersionAssemblyDescriptionDocumentFilter>();
        return source;
    }    
}
