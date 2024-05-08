using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LSL.Swashbuckle.AspNetCore.Filters;

internal class ServerUrlsDocumentFilter : IDocumentFilter
{
    private readonly IEnumerable<string> _serverUrls;

    public ServerUrlsDocumentFilter(IEnumerable<string> serverUrls)
    {
        _serverUrls = serverUrls;
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Servers = _serverUrls.Select(url => new OpenApiServer { Url = url }).ToList();
    }
}