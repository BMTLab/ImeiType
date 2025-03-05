using System.Buffers.Text;
using System.Diagnostics.Contracts;
using System.Text;
#if !NETSTANDARD
using System.Runtime.CompilerServices;
#endif

namespace BMTLab.ImeiType;

public readonly partial struct Imei
#if NET7_0_OR_GREATER
    : ISpanParsable<Imei>
#endif
#if NET8_0_OR_GREATER
      ,
      IUtf8SpanParsable<Imei>
#endif
{
#region Parse long
    /// <summary>
    ///     Parses a <see cref="long" /> into an <see cref="Imei" />.
    /// </summary>
    /// <param name="number">The <see cref="long" /> value to parse.</param>
    /// <exception cref="FormatException">If the value passed is not a valid IMEI format.</exception>
    /// <returns>An <see cref="Imei" /> instance.</returns>
    [Pure]
    public static Imei Parse(long number)
    {
        if (TryParse(number, out var result))
        {
            return result;
        }

        throw new FormatException($"'{number.ToString()}' is not a valid IMEI.");
    }


    /// <summary>
    ///     Tries to parse a <see cref="long" /> into an <see cref="Imei" />.
    /// </summary>
    /// <param name="number">The <see cref="long" /> value to parse.</param>
    /// <param name="result">
    ///     When this method returns, contains the result of successfully parsing <paramref name="number" />
    ///     or an undefined value on failure.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if <paramref name="number" /> was successfully parsed; otherwise, <see langword="false" />.
    /// </returns>
    public static bool TryParse(long number, out Imei result)
    {
        result = Invalid;

        if (IsValid(number))
        {
            result = new Imei(number, shouldValidate: false);

            Debug.Assert(IsValid(result));

            return true;
        }

        return false;
    }
#endregion _Parse long


#region Parse ReadOnlySpan<char>
    /// <summary>
    ///     Parses a <see cref="ReadOnlySpan{T}" /> of chars into an <see cref="Imei" />.
    /// </summary>
    /// <param name="s">The span of chars to parse.</param>
    /// <param name="provider">An object that provides culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="FormatException">If the value passed is not a valid IMEI format.</exception>
    /// <returns>An <see cref="Imei" /> instance.</returns>
    [Pure]
    public static Imei Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        throw new FormatException($"'{s.ToString()}' is not a valid IMEI.");
    }


    /// <summary>
    ///     Tries to parse a <see cref="ReadOnlySpan{T}" /> of chars into an <see cref="Imei" />.
    /// </summary>
    /// <param name="s">The span of chars to parse.</param>
    /// <param name="provider">An object that provides culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">
    ///     When this method returns, contains the result of successfully parsing <paramref name="s" /> or an undefined value
    ///     on failure.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if <paramref name="s" /> was successfully parsed; otherwise, <see langword="false" />.
    /// </returns>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Imei result)
    {
        result = Invalid;

        if (TryParseInternal(s, out var number))
        {
            result = new Imei(number, shouldValidate: false);

            return true;
        }

        return false;
    }
#endregion _Parse ReadOnlySpan<char>


#region Parse string
    /// <summary>
    ///     Parses a <see cref="string" /> into an <see cref="Imei" />.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An object that provides culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="FormatException">If the value passed is not a valid IMEI format.</exception>
    /// <returns>An <see cref="Imei" /> instance.</returns>
    [Pure]
    public static Imei Parse(string s, IFormatProvider? provider) =>
        Parse(s.AsSpan(), provider);


    /// <summary>
    ///     Tries to parse a <see cref="string" /> into an <see cref="Imei" />.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An object that provides culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">
    ///     When this method returns, contains the result of successfully parsing <paramref name="s" /> or an undefined value
    ///     on failure.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if <paramref name="s" /> was successfully parsed; otherwise, <see langword="false" />.
    /// </returns>
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Imei result) =>
        TryParse(s.AsSpan(), provider, out result);
#endregion _Parse string


#region Parse ReadOnlySpan<byte>
    /// <summary>
    ///     Parses a <see cref="ReadOnlySpan{T}" /> of bytes into an <see cref="Imei" />.
    /// </summary>
    /// <param name="utf8Text">The span of UTF-8 bytes to parse.</param>
    /// <param name="provider">
    ///     An object that provides culture-specific formatting information about
    ///     <paramref name="utf8Text" />.
    /// </param>
    /// <exception cref="FormatException">If the value passed is not a valid IMEI format.</exception>
    /// <returns>An <see cref="Imei" /> instance.</returns>
    [Pure]
    public static Imei Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
    {
        if (TryParse(utf8Text, provider, out var result))
        {
            Debug.Assert(IsValid(result));

            return result;
        }

        throw new FormatException($"'{Encoding.UTF8.GetString(utf8Text)}' is not a valid IMEI.");
    }


    /// <summary>
    ///     Tries to parse a <see cref="ReadOnlySpan{T}" /> of bytes into an <see cref="Imei" />.
    /// </summary>
    /// <param name="utf8Text">The span of UTF-8 bytes to parse.</param>
    /// <param name="provider">
    ///     An object that provides culture-specific formatting information about
    ///     <paramref name="utf8Text" />.
    /// </param>
    /// <param name="result">
    ///     On return, contains the result of successfully parsing <paramref name="utf8Text" /> or an undefined value on
    ///     failure.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if <paramref name="utf8Text" /> was successfully parsed; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out Imei result)
    {
        result = Invalid;

        if (TryParseInternal(utf8Text, out var number))
        {
            result = new Imei(number, shouldValidate: false);

            Debug.Assert(IsValid(result));

            return true;
        }

        return false;
    }
#endregion _Parse ReadOnlySpan<byte>


#region Overloads without IFormatProvider
#region Parse ReadOnlySpan<char> (no provider)
    /// <summary>
    ///     Parses a <see cref="ReadOnlySpan{T}" /> of chars into an <see cref="Imei" />.
    /// </summary>
    /// <param name="s">The span of chars to parse.</param>
    /// <exception cref="FormatException">If the value passed is not a valid IMEI format.</exception>
    /// <returns>An <see cref="Imei" /> instance.</returns>
    [Pure]
    public static Imei Parse(ReadOnlySpan<char> s) =>
        Parse(s, provider: null);


    /// <summary>
    ///     Tries to parse a <see cref="ReadOnlySpan{T}" /> of chars into an <see cref="Imei" />.
    /// </summary>
    /// <param name="s">The span of chars to parse.</param>
    /// <param name="result">
    ///     When this method returns, contains the result of successfully parsing <paramref name="s" /> or an undefined value
    ///     on failure.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if <paramref name="s" /> was successfully parsed; otherwise, <see langword="false" />.
    /// </returns>
    public static bool TryParse(ReadOnlySpan<char> s, out Imei result) =>
        TryParse(s, provider: null, out result);
#endregion _Parse ReadOnlySpan<char> (no provider)


#region Parse string (no provider)
    /// <summary>
    ///     Parses a <see cref="string" /> into an <see cref="Imei" />.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <exception cref="FormatException">If the value passed is not a valid IMEI format.</exception>
    /// <returns>An <see cref="Imei" /> instance.</returns>
    [Pure]
    public static Imei Parse(string s) =>
        Parse(s, provider: null);


    /// <summary>
    ///     Tries to parse a <see cref="string" /> into an <see cref="Imei" />.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="result">
    ///     When this method returns, contains the result of successfully parsing <paramref name="s" /> or an undefined value
    ///     on failure.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if <paramref name="s" /> was successfully parsed; otherwise, <see langword="false" />.
    /// </returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out Imei result) =>
        TryParse(s, provider: null, out result);
#endregion _Parse string (no provider)


#region Parse ReadOnlySpan<byte> (no provider)
    /// <summary>
    ///     Parses a <see cref="ReadOnlySpan{T}" /> of bytes into an <see cref="Imei" />.
    /// </summary>
    /// <param name="utf8Text">The span of UTF-8 bytes to parse.</param>
    /// <exception cref="FormatException">If the value passed is not a valid IMEI format.</exception>
    /// <returns>An <see cref="Imei" /> instance.</returns>
    [Pure]
    public static Imei Parse(ReadOnlySpan<byte> utf8Text) =>
        Parse(utf8Text, provider: null);


    /// <summary>
    ///     Tries to parse a <see cref="ReadOnlySpan{T}" /> of bytes into an <see cref="Imei" />.
    /// </summary>
    /// <param name="utf8Text">The span of UTF-8 bytes to parse.</param>
    /// <param name="result">
    ///     On return, contains the result of successfully parsing <paramref name="utf8Text" /> or an undefined value on
    ///     failure.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if <paramref name="utf8Text" /> was successfully parsed; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public static bool TryParse(ReadOnlySpan<byte> utf8Text, out Imei result) =>
        TryParse(utf8Text, provider: null, out result);
#endregion _Parse ReadOnlySpan<byte> (no provider)
#endregion _Overloads without IFormatProvider


#region Internal parser arrays to long
#if !NETSTANDARD
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
    private static bool TryParseInternal(ReadOnlySpan<char> s, out long result)
    {
        result = InvalidValue;

        if (s.Length != Length)
        {
            return false;
        }

        if (long.TryParse(s, out var number) && IsValid(number))
        {
            result = number;
            return true;
        }

        return false;
    }


#if !NETSTANDARD
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
    private static bool TryParseInternal(ReadOnlySpan<byte> s, out long result)
    {
        result = InvalidValue;

        if (s.Length != Length)
        {
            return false;
        }

        if (Utf8Parser.TryParse(s, out long number, out var bytesConsumed)
            && bytesConsumed == Length
            && IsValid(number))
        {
            result = number;
            return true;
        }

        return false;
    }
#endregion _Internal parser arrays to long
}