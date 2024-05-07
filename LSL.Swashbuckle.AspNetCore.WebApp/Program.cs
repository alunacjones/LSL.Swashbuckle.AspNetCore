using LSL.Swashbuckle.AspNetCore;
using LSL.Swashbuckle.AspNetCore.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGenWithVersioning(c =>
{
    c.WithTitleFromAssemblyOf<Program>();
    c.WithSwaggerGenOptionsConfigurator(c => c.AddStringEnumFilter().AddXmlCommentsForAssemblyOf<Program>());    
});

var app = builder.Build();

app.UseRouting();
app.UseSwagger(o => o.AddCodeVersionInfoForAssemblyOf<Program>().AddCodeVersionToApiDescriptionOf<Program>() );
app.UseSwaggerUIWithVersioning(o => o.AddDocumentTitleFromAssmblyOf<Program>());
app.MapControllers();

app.Run();
