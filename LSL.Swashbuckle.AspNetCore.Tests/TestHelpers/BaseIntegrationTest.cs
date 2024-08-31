using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.Swashbuckle.AspNetCore.Tests.TestHelpers;

public abstract class BaseIntegrationTest
{
    protected async Task RunTests(
        Func<WebApplicationFactory<Program>, Task> codeToRun,
        Action<IServiceCollection>? serviceConfigurator = null,
        string environment = "Development",
        bool prioritiseLocalSettings = false,
        IEnumerable<string>? commandLineArguments = null)
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(b =>
            {
                b.UseEnvironment(environment);

                if (commandLineArguments != null)
                {
                    foreach (var arg in commandLineArguments)
                    {
                        var split = arg.Split('=');
                        b.UseSetting(split[0], split[1]);
                    }
                }
            });
                
        application.Server.PreserveExecutionContext = true;

        await codeToRun(application);
    }
}