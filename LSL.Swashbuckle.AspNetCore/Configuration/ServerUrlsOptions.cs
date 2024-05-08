using System.Collections.Generic;

namespace LSL.Swashbuckle.AspNetCore.Configuration;

internal class ServerUrlsOptions
{
    public IEnumerable<string> ServerUrls { get; set; } = new List<string>();
}