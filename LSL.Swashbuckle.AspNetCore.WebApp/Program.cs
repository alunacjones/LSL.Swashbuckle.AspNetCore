using System.Diagnostics.CodeAnalysis;
using LSL.Swashbuckle.AspNetCore;
using LSL.Swashbuckle.AspNetCore.Configuration;

[assembly:ExcludeFromCodeCoverage]

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var versionFormat = args.FirstOrDefault(c => c.StartsWith("--versionFormat"))?.Split('=')[1];

builder.Services
    .AddCodeVersionForAssemblyOf<Program>(c => c.AddUrlForDevopsGitCommit("MyOrg", "MyProj", "MyRepo"))
    .AddSwaggerGenWithVersioning()
    .AddSwaggerGenWithVersioning(swaggerGenOptions => swaggerGenOptions        
        .AddStringEnumFilter()
        .AddXmlCommentsForAssemblyOf<Program>()
        .AddCodeVersionToApiDescription()       
        .WithServerUrls(["https://nowhere.com"])
        .WithTitleFromAssemblyOf<Program>()
    ,
    apiExplorerConfigurator: o => {
        if (versionFormat != null) o.WithVersionNumberFormat(versionFormat);        
    });

var app = builder.Build();

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUIWithVersioning(o => o.AddDocumentTitleFromAssemblyOf<Program>());
app.MapControllers();

app.Run();

public partial class Program {}
