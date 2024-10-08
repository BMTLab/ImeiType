using System.Diagnostics.Contracts;
#if !NETSTANDARD
using System.Runtime.CompilerServices;
#endif

namespace BMTLab.ImeiType;

public readonly partial struct Imei
{
    /// <summary>
    ///     Validates whether the given IMEI is valid.
    /// </summary>
    /// <remarks>
    ///     Although we restrict the initialization of the <see cref="Imei" /> type to an invalid or unset value,
    ///     we cannot restrict the creation of a structure via <c>default(Imei)</c>,
    ///     this method will verify that the structure does contain an proper IMEI number.
    /// </remarks>
    /// <param name="imei">The <see cref="Imei" /> value to validate.</param>
    /// <returns>
    ///     <see langword="true" /> if the IMEI number is valid; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    [Pure]
#if !NETSTANDARD
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static bool IsValid(Imei imei) =>
        imei != Invalid
        && IsValid(imei.ToInt64());


    /// <summary>
    ///     Validates whether the given IMEI number is valid.
    /// </summary>
    /// <param name="imeiNumber">The <see cref="long" /> value to validate.</param>
    /// <returns>
    ///     <see langword="true" /> if the IMEI number is valid; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    [Pure]
#if !NETSTANDARD
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
    public static bool IsValid(long imeiNumber) =>
        // Although 000_000_000_000_000_000_000 is a valid IMEI by structure, we do not consider it as a feasible IMEI.
        // 999_999_999_999_999_999_994 is the maximum possible IMEI and passing the checksum check.
        imeiNumber is >= MinValue and <= MaxValue
        && IsChecksumValid(imeiNumber);


    /// <summary>
    ///     Validates whether the given IMEI char span is valid.
    /// </summary>
    /// <param name="imeiCharSpan">The <see cref="ReadOnlySpan{T}" /> of chars to validate.</param>
    /// <returns><see langword="true" /> if the IMEI character span is valid; otherwise, <see langword="false" />.</returns>
    [Pure]
    public static bool IsValid(ReadOnlySpan<char> imeiCharSpan) =>
        TryParseInternal(imeiCharSpan, out var _);


    /// <summary>
    ///     Validates whether the given IMEI string is valid.
    /// </summary>
    /// <param name="s">The <see cref="string" /> to validate.</param>
    /// <returns>
    ///     <see langword="true" /> if the IMEI string is valid; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    [Pure]
    public static bool IsValid([NotNullWhen(true)] string? s) =>
        !string.IsNullOrWhiteSpace(s) &&
        TryParseInternal(s.AsSpan(), out var _);


    /// <summary>
    ///     Validates whether the given IMEI UTF-8 text span is valid.
    /// </summary>
    /// <param name="imeiUtf8Text">The <see cref="ReadOnlySpan{T}" /> of bytes to validate.</param>
    /// <returns>
    ///     <see langword="true" /> if the IMEI UTF-8 text span is valid; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    [Pure]
    public static bool IsValid(ReadOnlySpan<byte> imeiUtf8Text) =>
        TryParseInternal(imeiUtf8Text, out var _);


    /// <summary>
    ///     Luhn algorithm. Calculates a validation checksum to detect unintentional data corruption.
    /// </summary>
    /// <param name="imeiNumber">The IMEI number to validate the checksum for.</param>
    /// <returns><see langword="true" /> if the checksum is valid; otherwise, <see langword="false" />.</returns>
    [Pure]
#if !NETSTANDARD
    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
#endif
    internal static bool IsChecksumValid(long imeiNumber)
    {
        unchecked
        {
            var sum = 0;

            for (var digitPosition = 0; digitPosition < Length; digitPosition++)
            {
                // Extract the digit at the current position
                var currentDigit = (int) (imeiNumber % 10);

                // If the position is odd (every second digit from the right)
                if (digitPosition % 2 != 0)
                {
                    currentDigit *= 2;
                    if (currentDigit > 9)
                    {
                        currentDigit -= 9;
                    }
                }

                sum += currentDigit;
                imeiNumber /= 10;
            }

            return sum % 10 == 0;
        }
    }
}