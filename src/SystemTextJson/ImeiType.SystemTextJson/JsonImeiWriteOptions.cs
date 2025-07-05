using System.ComponentModel;

using JetBrains.Annotations;

namespace BMTLab.ImeiType.SystemTextJson;

/// <summary>
///     Defines how an <see cref="Imei" /> value should be written to JSON.
/// </summary>
/// <remarks>
///     <list type="bullet">
///         <item>
///             <term>
///                 <see cref="Default" />
///             </term>
///             <description>
///                 Uses the logic of <see cref="JsonSerializerOptions.NumberHandling" />
///                 to decide whether to write the <see cref="Imei" /> as a number or a string.
///             </description>
///         </item>
///         <item>
///             <term>
///                 <see cref="ForceWriteAsNumber" />
///             </term>
///             <description>
///                 Always writes the <see cref="Imei" /> as a numeric value,
///                 regardless of other serializer settings.
///             </description>
///         </item>
///         <item>
///             <term>
///                 <see cref="ForceWriteAsString" />
///             </term>
///             <description>
///                 Always writes the <see cref="Imei" /> as a string,
///                 ignoring any numeric-based serializer settings.
///             </description>
///         </item>
///     </list>
/// </remarks>
[PublicAPI]
[DefaultValue(Default)]
public enum JsonImeiWriteOptions
{
    /// <summary>
    ///     Applies the default logic based on <see cref="JsonSerializerOptions.NumberHandling" />
    ///     to determine how the <see cref="Imei" /> is written.
    /// </summary>
    Default = 0,

    /// <summary>
    ///     Always writes the <see cref="Imei" /> as a numeric (for example, <c>{"val": 356303489916807}</c>).
    /// </summary>
    ForceWriteAsNumber,

    /// <summary>
    ///     Always writes the <see cref="Imei" /> as a string (for example, <c>{"val": "356303489916807"}</c>).
    /// </summary>
    ForceWriteAsString
}