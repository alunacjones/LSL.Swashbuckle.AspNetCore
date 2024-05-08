using LSL.Swashbuckle.AspNetCore;
using LSL.Swashbuckle.AspNetCore.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddCodeVersionForAssemblyOf<Program>()
    .AddSwaggerGenWithVersioning(swaggerOptions =>
    {
        swaggerOptions
            .WithTitleFromAssemblyOf<Program>()
            .WithSwaggerGenOptionsConfigurator(c => c
                .AddStringEnumFilter()
                .AddXmlCommentsForAssemblyOf<Program>()
                .AddCodeVersionToApiDescription());            
    });

var app = builder.Build();

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUIWithVersioning(o => o.AddDocumentTitleFromAssemblyOf<Program>());
app.MapControllers();

app.Run();
