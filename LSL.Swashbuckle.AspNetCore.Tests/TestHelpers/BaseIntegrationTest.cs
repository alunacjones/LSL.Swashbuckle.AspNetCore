using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.Swashbuckle.AspNetCore.Tests.TestHelpers;

public abstract class BaseIntegrationTest
{
    protected async Task RunTests(Func<WebApplicationFactory<Program>, Task> codeToRun, Action<IServiceCollection>? serviceConfigurator = null, string environment = "Development", bool prioritiseLocalSettings = false)
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(b =>
            {
                
            });
                
        application.Server.PreserveExecutionContext = true;

        await codeToRun(application);
    }
}