using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace BMTLab.ImeiType.SystemTextJson.Tests.Unit.Extensions;

/// <summary>
///     Provides extension methods for JSON text manipulation in test scenarios.
/// </summary>
internal static partial class JsonTextExtensions
{
    /// <summary>
    ///     Removes any spaces and line breaks from the specified <paramref name="json" /> string,
    ///     so that it can be used in test assertions with consistent formatting.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method is designed specifically for unit testing within this project.
    ///         It strips all whitespace characters (including newlines) without distinction,
    ///         which may alter string content in a way not suitable for other scenarios.
    ///     </para>
    ///     <para>
    ///         Use this method only for tests that require comparing "minified" JSON against expected outputs.
    ///     </para>
    /// </remarks>
    /// <param name="json">A string containing JSON that may have spaces or line breaks.</param>
    /// <returns>A new string that contains the same JSON data but with no whitespace.</returns>
    [Pure]
    internal static partial string Minify(this string json);
}


#if NET7_0_OR_GREATER
internal static partial class JsonTextExtensions
{
    internal static partial string Minify(this string json) =>
        WhiteSpacesPattern().Replace(json, string.Empty);


    [GeneratedRegex(@"\s*")]
    private static partial Regex WhiteSpacesPattern();
}
#else
internal static partial class JsonTextExtensions
{
    internal static partial string Minify(this string json) =>
        Regex.Replace(json, @"\s*", string.Empty);
}
#endif