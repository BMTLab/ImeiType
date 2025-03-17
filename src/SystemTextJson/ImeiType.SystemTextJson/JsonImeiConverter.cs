using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

using JetBrains.Annotations;

namespace BMTLab.ImeiType.SystemTextJson;

/// <summary>
///     Provides custom JSON serialization and deserialization for <see cref="Imei" /> values.
/// </summary>
/// <remarks>
///     This converter can read <see cref="Imei" /> values from JSON strings or numbers and
///     can write them as either numbers or strings,
///     depending on <see cref="WriteOptions" /> and <see cref="JsonSerializerOptions.NumberHandling" />.
/// </remarks>
[PublicAPI]
public class JsonImeiConverter : JsonConverter<Imei>
{
    /// <summary>
    ///     Gets the behavior for writing <see cref="Imei" /> values to JSON.
    ///     The default value is <see cref="JsonImeiWriteOptions.Default" />.
    /// </summary>
    /// <remarks>
    ///     When set to <see cref="JsonImeiWriteOptions.Default" />, the converter
    ///     uses <see cref="JsonSerializerOptions.NumberHandling" /> to decide if the <see cref="Imei" />
    ///     is written as a numeric value or a string.
    /// </remarks>
    [DefaultValue(JsonImeiWriteOptions.Default)]
    public JsonImeiWriteOptions WriteOptions { get; init; } = JsonImeiWriteOptions.Default;


    /// <summary>
    ///     Reads an <see cref="Imei" /> value from the JSON data.
    /// </summary>
    /// <param name="reader">The <see cref="Utf8JsonReader" /> to read from.</param>
    /// <param name="typeToConvert">The type of the object to convert.</param>
    /// <param name="options">Optional serializer options.</param>
    /// <returns>An <see cref="Imei" /> object constructed from the JSON data.</returns>
    /// <exception cref="JsonException">Thrown if deserialization fails or if the JSON is not a valid <see cref="Imei" /> value.</exception>
    public override Imei Read
    (
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        try
        {
            return Imei.Parse(reader.ValueSpan);
        }
        catch (FormatException exception)
        {
            throw new JsonException("Failed to deserialize json field containing IMEI", exception);
        }
    }


    /// <summary>
    ///     Writes an <see cref="Imei" /> value to the JSON output.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter" /> to write to.</param>
    /// <param name="imei">The <see cref="Imei" /> value to write.</param>
    /// <param name="options">Optional serializer options.</param>
    /// <remarks>
    ///     Uses <see cref="WriteOptions" /> to determine whether to write the <see cref="Imei" /> as a number or a string.
    ///     If <see cref="JsonImeiWriteOptions.Default" /> is used, it falls back to <see cref="JsonSerializerOptions.NumberHandling" />:
    ///     if <see cref="JsonNumberHandling.WriteAsString" /> is not set, the converter writes a numeric value; otherwise, it writes a string.
    /// </remarks>
    [SuppressMessage("Major Code Smell", """S907:"goto" statement should not be used""")]
    public override void Write
    (
        Utf8JsonWriter writer,
        Imei imei,
        JsonSerializerOptions? options
    )
    {
        switch (WriteOptions)
        {
            case JsonImeiWriteOptions.ForceWriteAsNumber:
            {
                writer.WriteNumberValue(imei.ToInt64());
                return;
            }
            case JsonImeiWriteOptions.ForceWriteAsString:
            {
                writer.WriteStringValue(imei.ToReadOnlySpan());
                return;
            }
            case JsonImeiWriteOptions.Default:
            default:
            {
                if (options is null || !options.NumberHandling.HasFlag(JsonNumberHandling.WriteAsString))
                {
                    goto case JsonImeiWriteOptions.ForceWriteAsNumber;
                }

                goto case JsonImeiWriteOptions.ForceWriteAsString;
            }
        }
    }


#region DicionaryKeysHandling
    /// <summary>
    ///     Reads an <see cref="Imei" /> value from a property name token in JSON.
    /// </summary>
    /// <param name="reader">The <see cref="Utf8JsonReader" /> positioned at the property name.</param>
    /// <param name="typeToConvert">The type to convert to, which should be <see cref="Imei" />.</param>
    /// <param name="options">Optional serializer options.</param>
    /// <returns>An <see cref="Imei" /> object parsed from the property name.</returns>
    public override Imei ReadAsPropertyName
    (
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    ) =>
        Read(ref reader, typeToConvert, options);


    /// <summary>
    ///     Writes an <see cref="Imei" /> value as a JSON property name.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter" /> to write to.</param>
    /// <param name="imei">The <see cref="Imei" /> value to write as a property name.</param>
    /// <param name="options">Optional serializer options.</param>
    public override void WriteAsPropertyName
    (
        Utf8JsonWriter writer,
        Imei imei,
        JsonSerializerOptions options
    ) =>
        writer.WritePropertyName(imei.ToReadOnlySpan());
#endregion
}