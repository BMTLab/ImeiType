using System.Diagnostics.Contracts;
#if NET7_0_OR_GREATER
using System.Numerics;
#endif

namespace BMTLab.ImeiType;

public readonly partial struct Imei
#if NET7_0_OR_GREATER
    : IEqualityOperators<Imei, Imei, bool>
#endif
{
    /// <summary>
    ///     Determines whether two IMEI instances are equal.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns><see langword="true" /> if both instances are equal; otherwise, <see langword="false" />.</returns>
    [Pure]
    public static bool operator ==(Imei left, Imei right) =>
        left.Equals(right);


    /// <summary>
    ///     Determines whether two IMEI instances are not equal.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns><see langword="true" /> if both instances are not equal; otherwise, <see langword="false" />.</returns>
    [Pure]
    public static bool operator !=(Imei left, Imei right) =>
        !left.Equals(right);
}