using LSL.Swashbuckle.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddCodeVersionForAssemblyOf<Program>()
    .AddSwaggerGenWithVersioning()
    .AddSwaggerGenWithVersioning(swaggerGenOptions => swaggerGenOptions        
        .AddStringEnumFilter()
        .AddXmlCommentsForAssemblyOf<Program>()
        .AddCodeVersionToApiDescription()       
        .WithServerUrls(["https://nowhere.com"])
        .WithTitleFromAssemblyOf<Program>()
    );

var app = builder.Build();

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUIWithVersioning(o => o.AddDocumentTitleFromAssemblyOf<Program>());
app.MapControllers();

app.Run();

public partial class Program {}