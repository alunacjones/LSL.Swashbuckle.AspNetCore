using LSL.Swashbuckle.AspNetCore.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LSL.Swashbuckle.AspNetCore.Filters;

internal class CodeVersionAssemblyDescriptionDocumentFilter : IDocumentFilter
{
    private CodeVersionOptions _options;

    public CodeVersionAssemblyDescriptionDocumentFilter(IOptions<CodeVersionOptions> options)
    {
        _options = options.Value;
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (_options.Version == null) return;

        swaggerDoc.Info.Description = $"<code>API Code Version {_options.Version}</code>";
    }
}