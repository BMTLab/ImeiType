using System.ComponentModel;

namespace BMTLab.ImeiType;

public readonly partial struct Imei
{
    /// <summary>
    ///     The standard length of an IMEI number.
    /// </summary>
    public const int Length = 15;

    /// <summary>
    ///     The minimum valid value for an IMEI number.
    /// </summary>
    /// <value>00_000000_000001_9</value>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public const long MinValue = 00_000000_000001_9;

    /// <summary>
    ///     The maximum valid value for an IMEI number.
    /// </summary>
    // <value>99_999999_999999_4</value>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public const long MaxValue = 99_999999_999999_4;

    /// <summary>
    ///     Represents an invalid IMEI value.
    /// </summary>
    internal const long InvalidValue = 00_000000_000000_0; // 0L, that is default(long)

    /// <summary>
    ///     Represents an invalid IMEI with an invalid value.
    /// </summary>
    internal static readonly Imei Invalid = new(InvalidValue, shouldValidate: false);
}