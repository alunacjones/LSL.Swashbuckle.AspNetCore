[![Build status](https://img.shields.io/appveyor/ci/alunacjones/lsl-swashbuckle-aspnetcore.svg)](https://ci.appveyor.com/project/alunacjones/lsl-swashbuckle-aspnetcore)
[![Coveralls branch](https://img.shields.io/coverallsCoverage/github/alunacjones/LSL.Swashbuckle.AspNetCore)](https://coveralls.io/github/alunacjones/LSL.Swashbuckle.AspNetCore)
[![NuGet](https://img.shields.io/nuget/v/LSL.Swashbuckle.AspNetCore.svg)](https://www.nuget.org/packages/LSL.Swashbuckle.AspNetCore/)

# LSL.Swashbuckle.AspNetCore

This package provides extensions to [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore) and [Asp.Versioning.Mvc.ApiExplorer](https://www.nuget.org/packages/Asp.Versioning.Mvc.ApiExplorer) to provide a quicker route to implement versioning in an API.

# Quick Start

The following code shows the setting up of an API to use all of the provided extensions:

```csharp
using LSL.Swahbuckle.AspNetCore
...
// Within ConfigureServices

// Adds the information found in "Program"'s assembly's 
// AssemblyInformationalVersionAttribute to generated OpenApi documents
// this is then found under the "x-code-version" property 
// under the standard "info" property in the OpenApi document
// Properties of "x-code-version":
//  "version": the assembly version
// "commitHash": the githash if found in the AssemblyInformationalVersionAttribute,
// "commitUrl": only setup if a call is made to add the commit hash url 
//    (the following sets up a URL for Azure devops)
//    This is optional but when applied then any call to AddCodeVersionToApiDescription
//    will add a link to the version number to the commit
//      Other methods to setup the url are included for GitHub And BitBucket:
//          AddUrlForGitHubCommit
//          AddUrlForBitBucketCommit
services.AddCodeVersionForAssemblyOf<Program>(c => c
        .AddUrlForDevopsGitCommit("MyOrg", "MyProject", "MyRepo")
    )
    // Ensures all versions of the API are available in swagger UI
    .AddSwaggerGenWithVersioning(c =>
    {
        // Shows the code version in the Swagger UI description
        c.AddCodeVersionToApiDescription()
            // Picks up the xml comments for the assembly of the provided class 
            // (Program in this case)
            .AddXmlCommentsForAssemblyOf<Program>()
            // Adds a schema filter to automatically convert enums to strings
            .AddStringEnumFilter()
            // Populates the OpenApi documents with a list of server Urls 
            // (just one picked up here from the API's configuration)
            .WithServerUrls(new[] { builder.Configuration.GetValue<string>("SwaggerServerUrl")! })
            // Sets up the Swagger UI title 
            // (shown in the UI - not the html title) from the assembly name
            // of the provided class
            .WithTitleFromAssemblyOf<Program>();
    },
    // Allows for further configuration of the ApiVersioningOptions
    apiVersioningConfigurator: o => { }
    // Allows for further configuration of the ApiExplorerOptions
    // Default Version number format is "VVV" and a groupNameFormat of "'v'VVV"
    apiExplorerConfigurator: o => o.WithVersionNumberFormat("VV"));

...

var app = builder.Build();


// Enables swagger UI with versioning
app.UseSwaggerUIWithVersioning(c => 
{
    // Sets the html document title to "{assemblyName} - Swagger UI"
    c.AddDocumentTitleFromAssemblyOf<Program>();
});
```

> **NOTE**: It is recommended to setup a base controller to inherit from in your own controllers as follows:

```csharp
[ApiController]
[Route("v{version:apiVersion}/[controller]")]
public abstract class BaseController : ControllerBase
{
    
}
```
