namespace LSL.Swashbuckle.AspNetCore.Configuration;

/// <summary>
/// SwaggerGenWithVersioningOptionsExtensions
/// </summary>
public static class SwaggerGenWithVersioningOptionsExtensions
{
    /// <summary>
    /// Use the name of the assembly containg the given type as the API title
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static SwaggerGenWithVersioningOptions WithTitleFromAssemblyOf<T>(this SwaggerGenWithVersioningOptions source)
    {
        source.Title = typeof(T).Assembly.GetName().Name!;
        return source;
    }
}