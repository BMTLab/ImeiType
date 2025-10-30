using System.Diagnostics.Contracts;
using System.Security.Cryptography;
#if !NETSTANDARD
using System.Runtime.CompilerServices;
#endif

namespace BMTLab.ImeiType;

public readonly partial struct Imei
{
    /// <summary>
    ///     Generates a new random IMEI using a specified seed.
    /// </summary>
    /// <param name="seed">The seed for random generation.</param>
    /// <remarks>
    ///     Utilizes the <see cref="Random" /> class.
    ///     <i>For testing purposes.</i>
    /// </remarks>
    /// <returns>A new random <see cref="Imei" /> instance.</returns>
    [Pure]
    public static Imei NewRandomImei(int seed) =>
        NewRandomImei((min, max) => new Random(seed).Next(min, max));


    /// <summary>
    ///     Generates a new random IMEI.
    /// </summary>
    /// <remarks>
    ///     Utilizes the secure <see cref="RandomNumberGenerator" /> class.
    /// </remarks>
    /// <returns>A new random <see cref="Imei" /> instance.</returns>
    [Pure]
    public static Imei NewRandomImei() =>
        NewRandomImei(RandomNumberGenerator.GetInt32);


    [Pure]
#if !NETSTANDARD
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
    private static Imei NewRandomImei(Func<int, int, int> randomFunc)
    {
        Debug.Assert(randomFunc is not null);

        unchecked
        {
            Span<int> imeiDigits = stackalloc int[Length];

            // We consider pairs of elements in this array,
            // like this 01, 10, 30, 33, 35, ...
            ReadOnlySpan<int> reportingBodyIds =
            [
                0, 1, 1, 0, 3, 0,
                3, 3, 3, 5, 4, 4,
                4, 5, 4, 9, 5, 0,
                5, 1, 5, 2, 5, 3,
                5, 4, 8, 6, 9, 1,
                9, 8, 9, 9
            ];

            // Select a random Reporting Body Identifier (RBI)
            var sub = reportingBodyIds.Slice(randomFunc(arg1: 0, reportingBodyIds.Length >> 1), length: 2);
            imeiDigits[0] = sub[0];
            imeiDigits[1] = sub[1];

            var sum = 0;
            var imeiNumber = 0L;
            var placeValue = 100_000_000_000_000;

            for (var digitPosition = 0; digitPosition < Length - 1; digitPosition++)
            {
                if (digitPosition % 2 != 0)
                {
                    if (digitPosition > 1)
                    {
                        imeiDigits[digitPosition] = randomFunc(arg1: 0, arg2: 9);
                    }

                    // Apply Luhn algorithm weighting
                    var digit = imeiDigits[digitPosition] << 1;
                    if (digit > 9)
                    {
                        digit -= 9;
                    }

                    sum += digit;
                }
                else
                {
                    sum += imeiDigits[digitPosition];
                }

                imeiNumber += imeiDigits[digitPosition] * placeValue;
                placeValue /= 10;
            }

            // Calculate the check digit
            imeiNumber += (10 - sum % 10) % 10;

            var result = new Imei(imeiNumber, shouldValidate: false);

            Debug.Assert(IsValid(result));

            return result;
        }
    }
}