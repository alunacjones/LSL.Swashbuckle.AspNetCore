using LSL.Swashbuckle.AspNetCore.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LSL.Swashbuckle.AspNetCore.Filters;

internal class CodeVersionDocumentFilter : IDocumentFilter
{
    private readonly CodeVersionOptions _options;

    public CodeVersionDocumentFilter(IOptions<CodeVersionOptions> options)
    {
        _options = options.Value;
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (_options.Version != null)
        {
            swaggerDoc.Info.AddExtension("x-code-version", new OpenApiObject
            {
                ["version"] = new OpenApiString(_options.Version),
                ["commitHash"] = new OpenApiString(_options.CommitHash),
                ["commitUrl"] = new OpenApiString(_options.CommitUrl)
            });
        }
    }
}
