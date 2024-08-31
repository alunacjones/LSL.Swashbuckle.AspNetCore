using System.Diagnostics.CodeAnalysis;
using LSL.Swashbuckle.AspNetCore;
using LSL.Swashbuckle.AspNetCore.Configuration;

[assembly:ExcludeFromCodeCoverage]

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

string? GetArgument(string name) => args.FirstOrDefault(c => c.StartsWith($"--{name}"))?.Split('=')[1];

var versionFormat = GetArgument("versionFormat");
var commitProvider = GetArgument("commitProvider");

builder.Services
    .AddCodeVersionForAssemblyOf<Program>(c => 
    {
        var org = "MyOrg";
        var project = "MyProj";
        var repo = "MyRepo";

        switch (commitProvider)
        {
            case "devops": 
                c.AddUrlForDevopsGitCommit(org, project, repo);
                break;

            case "github": 
                c.AddUrlForGitHubCommit(org, repo);
                break;

            case "bitbucket": 
                c.AddUrlForBitBucketCommit(org, repo);
                break;                                
        }
    })
    .AddSwaggerGenWithVersioning()
    .AddSwaggerGenWithVersioning(swaggerGenOptions => swaggerGenOptions        
        .AddStringEnumFilter()
        .AddXmlCommentsForAssemblyOf<Program>()
        .AddCodeVersionToApiDescription()       
        .WithServerUrls(["https://nowhere.com"])
        .WithTitleFromAssemblyOf<Program>()
    ,
    apiVersioningConfigurator: o => { },
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
