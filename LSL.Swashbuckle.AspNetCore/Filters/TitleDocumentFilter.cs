using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LSL.Swashbuckle.AspNetCore.Filters;

internal class TitleDocumentFilter : IDocumentFilter
{
    private readonly string _title;

    public TitleDocumentFilter(string title)
    {
        _title = title;
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Info.Title = _title;
    }
}
