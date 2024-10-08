using System.Buffers.Text;
using System.ComponentModel;
using System.Text;

using JetBrains.Annotations;

namespace BMTLab.ImeiType;

/// <summary>
///     Represents a globally unique International Mobile Equipment Identity (IMEI).
/// </summary>
/// <remarks>
///     This struct provides methods to validate, parse, and generate IMEI numbers.
/// </remarks>
[PublicAPI]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct Imei
    : IEquatable<Imei>
{
    /// <summary>
    ///     Controls whether IMEI validation is performed during initialization.
    /// </summary>
    /// <remarks>
    ///     This static property <b>has a global effect</b> on all instances of <see cref="Imei" />.
    ///     When set to <see langword="false" />, validation is not enforced; otherwise, it's enforced.
    ///     <b>Use with caution.</b>
    /// </remarks>
    [DefaultValue(true)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static bool ShouldValidateWhileInitialization { get; set; } = true;

    /// <summary>
    ///     Private field that stores the IMEI number itself.
    /// </summary>
    private readonly long _value;


#region Constructors
    /// <summary>
    ///     Should not be called. Prevents instantiation without parameters.
    /// </summary>
    // The only way to restrict the use of the parameterless constructor
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Must not be used without parameters", error: true)]
    [SuppressMessage("Info Code Smell", "S1133:Deprecated code should be removed")]
    [ExcludeFromCodeCoverage]
    public Imei() =>
        _value = InvalidValue;


    /// <summary>
    ///     Initializes a new instance of the <see cref="Imei" /> struct with a specified number and validation.
    /// </summary>
    /// <param name="imeiNumber">The IMEI number.</param>
    /// <exception cref="FormatException">Thrown when the value is not a valid IMEI.</exception>
    public Imei(long imeiNumber) : this(imeiNumber, ShouldValidateWhileInitialization)
    {
    }


    /// <summary>
    ///     Initializes a new instance of the <see cref="Imei" /> struct with a specified string and validation.
    /// </summary>
    /// <param name="imeiString">The IMEI as a string.</param>
    /// <exception cref="FormatException">If the value passed is not a valid IMEI format.</exception>
    public Imei(string imeiString) : this(imeiString, ShouldValidateWhileInitialization)
    {
    }


    /// <summary>
    ///     Initializes a new instance of the <see cref="Imei" /> struct with a specified char span and validation.
    /// </summary>
    /// <param name="imeiCharSpan">The IMEI as an array of chars.</param>
    /// <exception cref="FormatException">Thrown when the value is not a valid IMEI.</exception>
    public Imei(ReadOnlySpan<char> imeiCharSpan) : this(imeiCharSpan, ShouldValidateWhileInitialization)
    {
    }


    /// <summary>
    ///     Initializes a new instance of the <see cref="Imei" /> struct with a specified UTF-8 string and validation.
    /// </summary>
    /// <param name="imeiUtf8Text">The IMEI as UTF-8 text.</param>
    /// <exception cref="FormatException">Thrown when the value is not a valid IMEI.</exception>
    public Imei(ReadOnlySpan<byte> imeiUtf8Text) : this(imeiUtf8Text, ShouldValidateWhileInitialization)
    {
    }


    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    private Imei(long imeiNumber, bool shouldValidate)
    {
        if (shouldValidate && !IsValid(imeiNumber))
        {
            throw new FormatException($"'{imeiNumber}' is not a valid IMEI.");
        }

        _value = imeiNumber;
    }


    private Imei(string imeiString, bool shouldValidate)
    {
        if (!shouldValidate)
        {
            _value = long.Parse(imeiString, provider: null);

            return;
        }

        if (!TryParseInternal(imeiString.AsSpan(), out var imeiNumber))
        {
            throw new FormatException($"'{imeiString}' is not a valid IMEI.");
        }

        _value = imeiNumber;
    }


    private Imei(ReadOnlySpan<char> imeiCharSpan, bool shouldValidate)
    {
        if (!shouldValidate)
        {
            _value = long.Parse(imeiCharSpan);

            return;
        }

        if (!TryParseInternal(imeiCharSpan, out var imeiNumber))
        {
            throw new FormatException($"'{imeiCharSpan.ToString()}' is not a valid IMEI.");
        }

        _value = imeiNumber;
    }


    [SuppressMessage("ReSharper", "RedundantAssignment")]
    private Imei(ReadOnlySpan<byte> imeiUtf8Text, bool shouldValidate)
    {
        if (!shouldValidate)
        {
            var isParsed = Utf8Parser.TryParse(imeiUtf8Text, out _value, out var bytesConsumed);

            Debug.Assert(isParsed);
            Debug.Assert(bytesConsumed == Length);

            return;
        }

        if (!TryParseInternal(imeiUtf8Text, out var imeiNumber))
        {
            throw new FormatException($"'{Encoding.UTF8.GetString(imeiUtf8Text)}' is not a valid IMEI.");
        }

        _value = imeiNumber;
    }
#endregion _Constructors


#region Conversion Methods & Operators
    /// <see cref="ToImei(long)" />
    /// <exception cref="FormatException">If the value passed is not a valid IMEI format.</exception>
    /// <returns>An <see cref="Imei" /> instance.</returns>
    [System.Diagnostics.Contracts.Pure]
    public static explicit operator Imei(long imeiNumber) =>
        ToImei(imeiNumber);


    /// <see cref="ToImei(string)" />
    /// <exception cref="FormatException">If the value passed is not a valid IMEI format.</exception>
    /// <returns>An <see cref="Imei" /> instance.</returns>
    [System.Diagnostics.Contracts.Pure]
    public static explicit operator Imei(string imeiString) =>
        ToImei(imeiString);


    /// <see cref="ToImei(ReadOnlySpan{char})" />
    /// <exception cref="FormatException">If the value passed is not a valid IMEI format.</exception>
    /// <returns>An <see cref="Imei" /> instance.</returns>
    [System.Diagnostics.Contracts.Pure]
    public static explicit operator Imei(ReadOnlySpan<char> imeiCharSpan) =>
        ToImei(imeiCharSpan);


    /// <see cref="ToImei(ReadOnlySpan{byte})" />
    /// <exception cref="FormatException">If the value passed is not a valid IMEI format.</exception>
    /// <returns>An <see cref="Imei" /> instance.</returns>
    [System.Diagnostics.Contracts.Pure]
    public static explicit operator Imei(ReadOnlySpan<byte> imeiUtf8Text) =>
        ToImei(imeiUtf8Text);


    /// <summary>
    ///     Converts a <see cref="long" /> to an <see cref="Imei" /> representation.
    /// </summary>
    /// <param name="imeiNumber">The IMEI number to convert.</param>
    /// <exception cref="FormatException">If the value passed is not a valid IMEI format.</exception>
    /// <returns>An <see cref="Imei" /> instance.</returns>
    [System.Diagnostics.Contracts.Pure]
    public static Imei ToImei(long imeiNumber) =>
        new(imeiNumber);


    /// <summary>
    ///     Converts a <see cref="string" /> to an <see cref="Imei" /> representation.
    /// </summary>
    /// <param name="imeiString">The IMEI string to convert.</param>
    /// <exception cref="FormatException">If the value passed is not a valid IMEI format.</exception>
    /// <returns>An <see cref="Imei" /> instance.</returns>
    [System.Diagnostics.Contracts.Pure]
    public static Imei ToImei(string imeiString) =>
        new(imeiString);


    /// <summary>
    ///     Converts a <see cref="ReadOnlySpan{T}" /> of chars to an <see cref="Imei" /> representation.
    /// </summary>
    /// <param name="imeiCharSpan">The IMEI character span to convert.</param>
    /// <exception cref="FormatException">If the value passed is not a valid IMEI format.</exception>
    /// <returns>An <see cref="Imei" /> instance.</returns>
    [System.Diagnostics.Contracts.Pure]
    public static Imei ToImei(ReadOnlySpan<char> imeiCharSpan) =>
        new(imeiCharSpan);


    /// <summary>
    ///     Converts a <see cref="ReadOnlySpan{T}" /> of bytes to an <see cref="Imei" /> representation.
    /// </summary>
    /// <param name="imeiUtf8Text">The IMEI UTF-8 text to convert.</param>
    /// <exception cref="FormatException">If the value passed is not a valid IMEI format.</exception>
    /// <returns>An <see cref="Imei" /> instance.</returns>
    [System.Diagnostics.Contracts.Pure]
    public static Imei ToImei(ReadOnlySpan<byte> imeiUtf8Text) =>
        new(imeiUtf8Text);


    /// <see cref="ToInt64()" />
    [System.Diagnostics.Contracts.Pure]
    public static explicit operator long(in Imei imei) =>
        imei.ToInt64();


    /// <see cref="ToString()" />
    [System.Diagnostics.Contracts.Pure]
    public static implicit operator string(in Imei imei) =>
        imei.ToString();


    /// <see cref="ToString()" />
    [System.Diagnostics.Contracts.Pure]
    public static implicit operator ReadOnlySpan<char>(in Imei imei) =>
        imei.ToString().AsSpan();


    /// <see cref="ToUtf8()" />
    [System.Diagnostics.Contracts.Pure]
    public static implicit operator ReadOnlySpan<byte>(in Imei imei) =>
        imei.ToUtf8();


    /// <summary>
    ///     Return the IMEI as a <see cref="long" /> representation.
    /// </summary>
    /// <returns>The IMEI number as a <see cref="long" />.</returns>
    [System.Diagnostics.Contracts.Pure]
    public long ToInt64() =>
        _value;


    /// <summary>
    ///     Converts the IMEI to a <see cref="ReadOnlySpan{T}" /> of chars representation.
    /// </summary>
    /// <returns>The IMEI as a <see cref="ReadOnlySpan{T}" /> of chars.</returns>
    [System.Diagnostics.Contracts.Pure]
    public ReadOnlySpan<char> ToReadOnlySpan() =>
        ToString().AsSpan();


    /// <summary>
    ///     Converts the IMEI to a <see cref="string" /> representation.
    /// </summary>
    /// <returns>The IMEI as a <see cref="string" />.</returns>
    [System.Diagnostics.Contracts.Pure]
    public override string ToString() =>
        _value.ToString();


    /// <summary>
    ///     Converts the IMEI to a UTF-8 encoded <see cref="ReadOnlySpan{T}" /> of bytes.
    /// </summary>
    [System.Diagnostics.Contracts.Pure]
    public ReadOnlySpan<byte> ToUtf8()
    {
        Span<byte> utf8String = new byte[Length];

        // ReSharper disable once RedundantAssignment
        var isParsed = Utf8Formatter.TryFormat(_value, utf8String, out var bytesWritten);

        Debug.Assert(isParsed);
        Debug.Assert(bytesWritten == Length);

        return utf8String;
    }
#endregion _Conversion Operators


    /// <inheritdoc />
    [System.Diagnostics.Contracts.Pure]
    public override int GetHashCode() =>
        _value.GetHashCode();


    /// <inheritdoc />
    [System.Diagnostics.Contracts.Pure]
    [SuppressMessage("ReSharper", "InlineTemporaryVariable")]
    public override bool Equals(object? obj)
    {
        var tmp = obj;

        return tmp is Imei other && Equals(other);
    }


    /// <inheritdoc />
    [System.Diagnostics.Contracts.Pure]
    public bool Equals(Imei other) =>
        _value.Equals(other._value);
}