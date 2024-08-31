using System.Threading.Tasks;
using FluentAssertions;
using LSL.Swashbuckle.AspNetCore.Tests.TestHelpers;
using Newtonsoft.Json.Linq;
using FluentAssertions.Json;
using Newtonsoft.Json;
using FluentAssertions.Execution;
using System;

namespace LSL.Swashbuckle.AspNetCore.Tests;

public class Tests : BaseIntegrationTest
{
    [TestCase("v1", "expected-v1-schema.json", new string[0])]
    [TestCase("v2", "expected-v2-schema.json", new string[0])]
    [TestCase("v1.0", "expected-v1.0-schema.json", new[] { "versionFormat=VV" })]
    [TestCase("v2.0", "expected-v2.0-schema.json", new[] { "versionFormat=VV" })]
    public async Task DoIt(string apiVersion, string expectationResource, string[] args)
    {
        await RunTests(async app =>
        {
            var client = app.CreateDefaultClient();

            var response = await client.GetAsync($"/swagger/{apiVersion}/swagger.json");

            response.Should().BeSuccessful();

            var content = JObject.Parse(await response.Content.ReadAsStringAsync())!;
            var contentWithoutInfo = content.DeepClone().RemoveFields(["info"]);
            var info = content.SelectToken("info")!.ToObject<InfoCodeVersion>()!;

            using var _ = new AssertionScope();

            info.Title.Should().Be("LSL.Swashbuckle.AspNetCore.WebApp");
            info.Description.Should().StartWith("<code>API Code Version [1.0.0+");
            info.CodeVersion.CommitHash.Should().MatchRegex(@"^[a-f0-9]{40}$");
            info.CodeVersion.CommitUrl.Should().MatchRegex(@"^https://dev.azure.com/MyOrg/MyProj/_git/MyRepo/commit/[a-f0-9]{40}$");
            info.CodeVersion.Version.Should().MatchRegex(@"1\.0\.0\+[a-f0-9]{40}");

            contentWithoutInfo.Should().BeEquivalentTo(JObject.Parse(await GetResource(expectationResource)));
        },
        commandLineArguments: args);
    }

    internal class InfoCodeVersion
    {
        [JsonProperty("x-code-version")]
        public CodeVersion CodeVersion { get; set; } = new();
        public string Description { get; set; } = default!;
        public string Title { get; set; } = default!;
    }

    internal class CodeVersion 
    {
        public string CommitHash { get; set; } = default!;
        public string Version { get; set; } = default!;
        public string CommitUrl { get; set; } = default!;
    }
}
