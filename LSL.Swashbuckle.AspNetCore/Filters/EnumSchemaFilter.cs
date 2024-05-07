using System;
using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LSL.Swashbuckle.AspNetCore;

/// <summary>
/// Ensures enum types generate the textual values as opposed to the numberic value
/// </summary>
public class EnumSchemaFilter : ISchemaFilter
{
    /// <inheritdoc/>
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            model.Enum.Clear();
            model.Type = "string";

            Enum.GetNames(context.Type)
                .ToList()
                .ForEach(n => model.Enum.Add(new OpenApiString(n)));
        }
    }
}