using System.Threading.Tasks;
using FluentAssertions;
using LSL.Swashbuckle.AspNetCore.Tests.TestHelpers;
using Newtonsoft.Json.Linq;
using FluentAssertions.Json;
using Newtonsoft.Json;
using FluentAssertions.Execution;
using System.Linq;

namespace LSL.Swashbuckle.AspNetCore.Tests;
#pragma warning disable CA1861 // Avoid constant arrays as arguments   

public class Tests : BaseIntegrationTest
{
    [TestCase("devops", "v1", "expected-v1-schema.json", new string[0], @"^https://dev\.azure\.com/MyOrg/MyProj/_git/MyRepo/commit/[a-f0-9]{40}$")]
    [TestCase("devops", "v2", "expected-v2-schema.json", new string[0], @"^https://dev\.azure\.com/MyOrg/MyProj/_git/MyRepo/commit/[a-f0-9]{40}$")]
    [TestCase("devops", "v1.0", "expected-v1.0-schema.json", new[] { "versionFormat=VV" }, @"^https://dev\.azure\.com/MyOrg/MyProj/_git/MyRepo/commit/[a-f0-9]{40}$")]
    [TestCase("devops", "v2.0", "expected-v2.0-schema.json", new[] { "versionFormat=VV" }, @"^https://dev\.azure\.com/MyOrg/MyProj/_git/MyRepo/commit/[a-f0-9]{40}$")]

    [TestCase("github", "v1", "expected-v1-schema.json", new string[0], @"^https://github\.com/MyOrg/MyRepo/commit/[a-f0-9]{40}$")]
    [TestCase("github", "v2", "expected-v2-schema.json", new string[0], @"^https://github\.com/MyOrg/MyRepo/commit/[a-f0-9]{40}$")]
    [TestCase("github", "v1.0", "expected-v1.0-schema.json", new[] { "versionFormat=VV" }, @"^https://github\.com/MyOrg/MyRepo/commit/[a-f0-9]{40}$")]
    [TestCase("github", "v2.0", "expected-v2.0-schema.json", new[] { "versionFormat=VV" }, @"^https://github\.com/MyOrg/MyRepo/commit/[a-f0-9]{40}$")]

    [TestCase("bitbucket", "v1", "expected-v1-schema.json", new string[0], @"^https://bitbucket\.org/MyOrg/MyRepo/commits/[a-f0-9]{40}$")]
    [TestCase("bitbucket", "v2", "expected-v2-schema.json", new string[0], @"^https://bitbucket\.org/MyOrg/MyRepo/commits/[a-f0-9]{40}$")]
    [TestCase("bitbucket", "v1.0", "expected-v1.0-schema.json", new[] { "versionFormat=VV" }, @"^https://bitbucket\.org/MyOrg/MyRepo/commits/[a-f0-9]{40}$")]
    [TestCase("bitbucket", "v2.0", "expected-v2.0-schema.json", new[] { "versionFormat=VV" }, @"^https://bitbucket\.org/MyOrg/MyRepo/commits/[a-f0-9]{40}$")]

    [TestCase(null, "v1", "expected-v1-schema.json", new string[0], null)]
    [TestCase(null, "v2", "expected-v2-schema.json", new string[0], null)]
    [TestCase(null, "v1.0", "expected-v1.0-schema.json", new[] { "versionFormat=VV" }, null)]
    [TestCase(null, "v2.0", "expected-v2.0-schema.json", new[] { "versionFormat=VV" }, null)]
    public async Task DoIt(string commitProvider, string apiVersion, string expectationResource, string[] args, string expectedCommitUrlRegex)
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

            if (expectedCommitUrlRegex == null)
            {
                info.Description.Should().StartWith("<code>API Code Version 1.0.0+");
                info.CodeVersion.CommitUrl.Should().BeNull();
            }
            else
            {
                info.Description.Should().StartWith("<code>API Code Version [1.0.0+");
                info.Description.Should().MatchRegex($"{expectedCommitUrlRegex[1..^1]}\\)</code>$");
                info.CodeVersion.CommitUrl.Should().MatchRegex(expectedCommitUrlRegex);
            }
            
            info.CodeVersion.CommitHash.Should().MatchRegex(@"^[a-f0-9]{40}$");            
            info.CodeVersion.Version.Should().MatchRegex(@"1\.0\.0\+[a-f0-9]{40}");

            contentWithoutInfo.Should().BeEquivalentTo(JObject.Parse(await GetResource(expectationResource)));
        },
        commandLineArguments: args.Concat([$"commitProvider={commitProvider}"]));
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
#pragma warning restore CA1861 // Avoid constant arrays as arguments    