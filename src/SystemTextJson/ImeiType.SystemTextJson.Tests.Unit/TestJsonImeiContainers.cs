using JetBrains.Annotations;

namespace BMTLab.ImeiType.SystemTextJson.Tests.Unit;

/// <summary>
///     A container for testing scenarios where an <see cref="Imei" /> field may be null.
/// </summary>
[UsedImplicitly]
public record NullableImeiContainer
{
    /// <summary>Optional IMEI value.</summary>
    [JsonPropertyName("val")]
    public Imei? Imei { get; init; }
}


/// <summary>
///     A container for testing scenarios with a non-null <see cref="Imei" /> field.
/// </summary>
[UsedImplicitly]
public record ImeiContainer
{
    /// <summary>IMEI value.</summary>
    [JsonPropertyName("val")]
    public Imei Imei { get; init; }
}


/// <summary>
///     A container that stores a dictionary whose key and/or value may be an <see cref="Imei" />.
///     Used to test JSON scenarios involving dictionaries.
/// </summary>
/// <typeparam name="TKey">The dictionary key type.</typeparam>
/// <typeparam name="TValue">The dictionary value type, which may be <see cref="Imei" /> or another type.</typeparam>
[UsedImplicitly]
public record ImeiDictionaryContainer<TKey, TValue> where TKey : notnull
{
    /// <summary>Dictionary field for testing dictionary-based JSON structures.</summary>
    [JsonPropertyName("val")]
    public Dictionary<TKey, TValue?>? ImeiDictionary { get; init; }
}


/// <summary>
///     A container for testing scenarios where the <see cref="Imei" /> values are stored in a list.
/// </summary>
[UsedImplicitly]
public record ImeiListContainer
{
    /// <summary>List of IMEI values (nullable).</summary>
    [JsonPropertyName("val")]
    public List<Imei?>? ImeiList { get; init; }
}