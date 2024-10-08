# BMTLab.ImeiType

[![Release](https://github.com/BMTLab/ImeiType/actions/workflows/ci-main.yml/badge.svg)](https://github.com/BMTLab/ImeiType/actions/workflows/ci-main.yml)
[![Build](https://github.com/BMTLab/ImeiType/actions/workflows/ci-build.yml/badge.svg)](https://github.com/BMTLab/ImeiType/actions/workflows/ci-build.yml)
[![CodeQL](https://github.com/BMTLab/ImeiType/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/BMTLab/ImeiType/actions/workflows/github-code-scanning/codeql)
[![NuGet](https://img.shields.io/nuget/v/BMTLab.ImeiType?logo=nuget)](https://www.nuget.org/packages/BMTLab.ImeiType)
![Nuget Downloads](https://img.shields.io/nuget/dt/BMTLab.ImeiType)

**BMTLab.ImeiType** is a .NET library
that provides a strongly-typed IMEI (International Mobile Equipment Identity) `readonly struct`.
It offers a simple API for **validation**, **parsing**, and **generating** IMEI numbers.
This package helps improve type safety, reduce errors, and handle IMEI values consistently in your applications.

> **Supported Platforms**  
> \- `.NET 9.0`, `8.0`, `7.0`, `6.0`, and  
> \- `.NET Standard 2.1`

## Installation

Install via [NuGet](https://nuget.org):

```bash
dotnet add package BMTLab.ImeiType
```

Or edit your `.csproj`:

```xml
<PackageReference Include="BMTLab.ImeiType" Version="1.0.0" />
```

## Features

```csharp
public readonly struct Imei
  : IEquatable<Imei>, 
    IEqualityOperators<Imei, Imei, bool>, // .NET 7.0 or greater
    ISpanParsable<Imei>,                  // .NET 7.0 or greater
    IUtf8SpanParsable<Imei>               // .NET 8.0 or greater

```

1. **Strongly-Typed Imei Struct**

Stores the IMEI number (`long`) internally.
Providing high-performance methods and approaches with minimized memory allocation,
as well as a set of specific tweaks and interface implementations for each supported platform.

2. **Multiple Parsing And Creation Options**

`Imei.Parse(...)` and `Imei.TryParse(...)` from `long`, `string`, `ReadOnlySpan<char>`, or `ReadOnlySpan<byte> (UTF-8)`
with overload options.

3. **Random IMEI Generation**

```csharp
Imei.NewRandomImei();         // Creates a secure random IMEI using secure RandomNumberGenerator.
Imei.NewRandomImei(int seed); // Useful for testing or reproducible results.
```

4. **Detailed IMEI Parts & Constants**

```csharp
Imei.Tac; // Type Allocation Code ('35', e.g.)
Imei.Fac; // Final Assembly Code  ('630348', e.g.)
Imei.Snr; // Serial Number        ('991680', e.g.)
```

```csharp
public const int Length = 15;

// Minimum and maximum allowed value of IMEI number
public const long MinValue = 00_000000_000001_9; 
public const long MaxValue = 99_999999_999999_4;
```

5. **Conversions**

   :white_check_mark: Implicit conversion to `string`, `ReadOnlySpan<char>`, and `ReadOnlySpan<byte>`;

   :white_check_mark: Explicit conversion from `long`, `string`, or spans;

   :white_check_mark: Helper methods: `ToInt64()`, `ToReadOnlySpan()` and `ToUtf8()` and vice versa `ToImei()`.

6. **Validation**

Checks values of different types to see if the value is a valid IMEI (_via Luhn checksum_).

## Quick Example

### Initialization

```csharp
using BMTLab.ImeiType;

// This line shall not compile
Imei illegal = new Imei();
    
// Create an IMEI from a long (int64)
Imei imei1 = new Imei(356303489916807);

// From string or char arrays (or spans)
Imei imei2 = new Imei("356303489916807");
Imei imei3 = new Imei("356303489916807".AsSpan());
Imei imei4 = new Imei(['3', '5', '6', '3', '0', '3', '4', '8', '9', '9', '1', '6', '8', '0', '7']);

// From UTF-8 text
Imei imei5 = new Imei("356303489916807"u8);
Imei imei6 = new Imei([0x33, 0x35, 0x36, 0x33, 0x30, 0x33, 0x34, 0x38, 0x39, 0x39, 0x31, 0x36, 0x38, 0x30, 0x37]);
```

> [!IMPORTANT]
> Because `Imei` is a `readonly struct`, a parameterless constructor cannot be fully removed.
> We therefore mark the empty constructor with `[Obsolete(error: true)]` to prevent its usage.

Alternatively, you can cast values into an `Imei`:

```csharp
using BMTLab.ImeiType;

Imei imei1 = (Imei) 356303489916807;
Imei imei2 = (Imei) "356303489916807";
Imei imei3 = (Imei) "356303489916807".AsSpan();
Imei imei4 = (Imei) new char[] {'3', '5', '6', '3', '0', '3', '4', '8', '9', '9', '1', '6', '8', '0', '7'};
Imei imei5 = (Imei) "356303489916807"u8;
Imei imei6 = (Imei) stackalloc byte[] { 0x33, 0x35, 0x36, 0x33, 0x30, 0x33, 0x34, 0x38, 0x39, 0x39, 0x31, 0x36, 0x38, 0x30, 0x37 };
```

Or parse it, for example:

```csharp
using BMTLab.ImeiType;

bool canBeParsedAndValid = Imei.TryParse(356303489916807, out Imei parsedImei);
// ...and so on
```

### Validation

By default,
every new `Imei(...)` is validated to ensure the 15-digit number is correct,
according to the IMEI specification (_using a Luhn checksum_).

To avoid getting a `FormatException` during creation, you can use one of these overloads to check validity
(_return boolean_):

- `IsValid(long)`
- `IsValid(ReadOnlySpan<byte>)`
- `IsValid(ReadOnlySpan<char>)`
- `IsValid(string?)`
- `IsValid(Imei)` - in case `Imei` was created via `default(Imei)`, which unfortunately we cannot restrict.

> [!TIP]
> If you are sure that the passed value will never be wrong,
> you can disable the check on creation and improve performance a bit.
> To do this, set this static property:

```csharp
/* (`true` by default) can be set to `false` to skip validation on creation (use carefully) */
Imei.ShouldValidateWhileInitialization = false; 
```

> [!IMPORTANT]
> but use this at your own risk as it will affect the behavior globally.

### Conversion

Where a primitive type is expected, the IMEI type itself can easily be used, typically:

```csharp
using BMTLab.ImeiType;

var imei = (Imei) 356303489916807;

string imeiAsString = imei;
ReadOnlySpan<char> imeiAsCharSpan = imei;
ReadOnlySpan<byte> imeiAsUtf8Text = imei;

/* Conversion to long is explicit, it is necessary to avoid ambiguous type reference when printing e.g. */
long imeiAsLong = (long) imei; 
```

```csharp
Console.WriteLine($"IMEI value: {imei}"); //> "IMEI value: 015434904561440", e.g.
```

### Equality check

```csharp
using BMTLab.ImeiType;

var imeiA = (Imei) "356303489916807";
var imeiB = (Imei) "356303489916807";

Console.WriteLine(imeiA == imeiB);      //> true
Console.WriteLine(imeiA.Equals(imeiB)); //> true
```

### Generating a new random IMEI

```csharp
using BMTLab.ImeiType;

var newImeiA = Imei.NewRandomImei(seed: 42); // By using System.Random. Useful for testing or reproducible results
var newImeiB = Imei.NewRandomImei();         // By using System.Security.Cryptography

WriteLine($"Random IMEI (with seed): {newImeiA}");
WriteLine($"Random IMEI (secure, but slower a bit): {newImeiB}");
```

## Future Ideas

I plan to add a couple of new optional extension projects for `ImeiType`,
such as integration with `System.Text.Json` and `EF Core`.


****************************

## Notice

This project uses the dependency [FluentAssertions](https://github.com/fluentassertions/fluentassertions/tree/7.0.0)
version `7.0.0`,
which is distributed under a license other than
MIT — [Apache 2.0](https://github.com/fluentassertions/fluentassertions/blob/7.0.0/LICENSE) —
but the `ImeiType` project does not include any binary files or source code
of [FluentAssertions](https://github.com/fluentassertions/fluentassertions/tree/7.0.0).

## [Contributing](CONTRIBUTING.md)

Please feel free to fork this, contribute or let me know if you find a bug.
Any ideas for improvement are always welcome as well :innocent: