using System;
using System.Linq;
using System.Threading.Tasks;
using Baseline;
using Microsoft.Extensions.FileProviders;

namespace LSL.Swashbuckle.AspNetCore.Tests.TestHelpers;

public static class Resources
{
    public static async Task<string> GetResource(string name)
    {
        var fp = new EmbeddedFileProvider(typeof(Resources).Assembly);
        var file = fp.GetDirectoryContents("/").Where(f => f.Name.EndsWith(name)).FirstOrDefault() ?? throw new ArgumentException("Resource not found");

        using var stream = file.CreateReadStream();
        return await stream.ReadAllTextAsync();
    }
}