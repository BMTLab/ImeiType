using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace BMTLab.ImeiType.SystemTextJson.Tests.Unit.Extensions;

/// <summary>
///     Provides extension methods for JSON text manipulation in test scenarios.
/// </summary>
public static partial class JsonTextExtensions
{
    /// <summary>
    ///     Removes any spaces and line breaks from the specified <paramref name="json" /> string,
    ///     so that it can be used in test assertions with consistent formatting.
    /// </summary>
    [Pure]
    public static partial string Minify(this string json);


    /// <summary>
    ///     Method needed only for a correct and convenient way to compose an expected JSON text value.
    /// </summary>
    [Pure]
    [SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance")]
    public static object WriteJsonTokenAs(JsonTokenType jsonType, object? val)
    {
        if (val is null || jsonType == JsonTokenType.Null)
        {
            return "null";
        }

        return jsonType switch
        {
            JsonTokenType.String => $"\"{val}\"",
            JsonTokenType.Number => val,
            var _ => throw new ArgumentOutOfRangeException(
                nameof(jsonType),
                jsonType,
                $"It allowed to embed the IMEI value into JSON, only as {JsonTokenType.String} or as {JsonTokenType.Number}, but not as {jsonType}."
            )
        };
    }
}


#if NET7_0_OR_GREATER
partial class JsonTextExtensions
{
    public static partial string Minify(this string json) =>
        WhiteSpacesPattern().Replace(json, string.Empty);


    [GeneratedRegex(@"\s*")]
    private static partial Regex WhiteSpacesPattern();
}
#else
partial class JsonTextExtensions
{
    public static partial string Minify(this string json) =>
        Regex.Replace(json, @"\s*", string.Empty);
}
#endif