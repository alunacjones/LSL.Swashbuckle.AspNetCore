using System;

namespace LSL.Swashbuckle.AspNetCore.Configuration;

/// <summary>
/// AddCodeVersionOptionsExtensions
/// </summary>
public static class AddCodeVersionOptionsExtensions
{
    /// <summary>
    /// Allows for the adding of a URL to a git commit in DevOps
    /// </summary>
    /// <param name="source"></param>
    /// <param name="organisationName">Your organisation name</param>
    /// <param name="projectName">The project name</param>
    /// <param name="repositoryName">The repository name</param>
    /// <returns></returns>
    public static AddCodeVersionOptions AddUrlForDevopsGitCommit(this AddCodeVersionOptions source, string organisationName, string projectName, string repositoryName)
    {
        source.CommitUrlProvider = new Func<string, string>(hash => $"https://dev.azure.com/{organisationName}/{projectName}/_git/{repositoryName}/commit/{hash}");
        return source;
    }

    /// <summary>
    /// Allows for the adding of a URL to a commit in BitBucket
    /// </summary>
    /// <param name="source"></param>
    /// <param name="organisationName"></param>
    /// <param name="repositoryName"></param>
    /// <returns></returns>
    public static AddCodeVersionOptions AddUrlForBitBucketCommit(this AddCodeVersionOptions source, string organisationName, string repositoryName)
    {
        source.CommitUrlProvider = new Func<string, string>(hash => $"https://bitbucket.org/{organisationName}/{repositoryName}/commits/{hash}");
        return source;
    }

    /// <summary>
    /// Allows for the adding of a URL to a commit in GitHub
    /// </summary>
    /// <param name="source"></param>
    /// <param name="organisationName"></param>
    /// <param name="repositoryName"></param>
    /// <returns></returns>
    public static AddCodeVersionOptions AddUrlForGitHubCommit(this AddCodeVersionOptions source, string organisationName, string repositoryName)
    {
        source.CommitUrlProvider = new Func<string, string>(hash => $"https://github.com/{organisationName}/{repositoryName}/commit/{hash}");
        return source;
    }    
}