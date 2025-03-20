using JetBrains.Annotations;

using static System.ArgumentNullException;

namespace BMTLab.ImeiType.SystemTextJson.Extensions;

/// <summary>
///     Provides extension methods to configure JSON serialization options for <see cref="Imei" /> values.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static class JsonSerializerOptionsExtensions
{
    /// <summary>
    ///     Adds an <see cref="JsonImeiConverter" /> to this <see cref="JsonSerializerOptions" /> to handle IMEI serialization.
    /// </summary>
    /// <param name="options">The <see cref="JsonSerializerOptions" /> instance being configured.</param>
    /// <param name="writeOptions">
    ///     Defines how an <see cref="Imei" /> value is written (number, string, or fallback).
    ///     Defaults to <see cref="JsonImeiWriteOptions.Default" />.
    /// </param>
    /// <returns>
    ///     The same <see cref="JsonSerializerOptions" />, enabling call chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="options" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method extends <see cref="JsonSerializerOptions" />
    ///     so that any subsequent serialization or deserialization of <see cref="Imei" /> values
    ///     will use the <see cref="JsonImeiConverter" />.
    /// </remarks>
    [System.Diagnostics.Contracts.Pure]
    public static JsonSerializerOptions WithImeiConverter
    (
        this JsonSerializerOptions options,
        JsonImeiWriteOptions writeOptions = JsonImeiWriteOptions.Default
    )
    {
        ThrowIfNull(options);

        AddImeiConverter(options.Converters, writeOptions);

        return options;
    }


    /// <summary>
    ///     Adds an <see cref="JsonImeiConverter" /> to the specified collection of <see cref="JsonConverter" />.
    /// </summary>
    /// <param name="converters">The collection of <see cref="JsonConverter" /> being extended.</param>
    /// <param name="writeOptions">
    ///     Defines how an <see cref="Imei" /> value is written (number, string, or fallback).
    ///     Defaults to <see cref="JsonImeiWriteOptions.Default" />.
    /// </param>
    /// <returns>
    ///     The same collection of <see cref="JsonConverter" />, enabling call chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="converters" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This extension allows for a more granular configuration
    ///     where you can add the <see cref="Imei" /> converter directly to your custom list of JSON converters.
    ///     <para>
    ///         Example usage:
    ///         <code>
    ///             var converters = new List&lt;JsonConverter&gt;();
    ///             converters.AddImeiConverter(JsonImeiWriteOptions.ForceWriteAsString);
    ///         </code>
    ///     </para>
    /// </remarks>
    public static IList<JsonConverter> AddImeiConverter
    (
        this IList<JsonConverter> converters,
        JsonImeiWriteOptions writeOptions = JsonImeiWriteOptions.Default
    )
    {
        ThrowIfNull(converters);

        converters.Add(new JsonImeiConverter
        {
            WriteOptions = writeOptions
        });

        return converters;
    }
}