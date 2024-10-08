namespace BMTLab.ImeiType;

public readonly partial struct Imei
{
    /// <summary>
    ///     Gets the TAC (Type Allocation Code) part of the IMEI.
    /// </summary>
    public int Tac => (int) (_value / 01_000000_000000_0);

    /// <summary>
    ///     Gets the FAC (Final Assembly Code) part of the IMEI.
    /// </summary>
    public int Fac => (int) (_value / 00_0000001_000000_0 % 1_000_000);

    /// <summary>
    ///     Gets the SNR (Serial Number) part of the IMEI.
    /// </summary>
    public int Snr => (int) (_value / 00_0000000_000001_0 % 1_000_000);
}