using Asp.Versioning.ApiExplorer;

namespace LSL.Swashbuckle.AspNetCore;

/// <summary>
/// ApiExplorerOptionsExtensions
/// </summary>
public static class ApiExplorerOptionsExtensions
{
    /// <summary>
    /// Sets up the <c>GroupNameFormat</c> with <c>v{format}</c> and the <c>SubstitionFormat</c> with <c>format</c>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static ApiExplorerOptions WithVersionNumberFormat(this ApiExplorerOptions source, string format)
    {
        source.GroupNameFormat = $"'v'{format}";
        source.SubstitutionFormat = format;
        return source;
    }
}