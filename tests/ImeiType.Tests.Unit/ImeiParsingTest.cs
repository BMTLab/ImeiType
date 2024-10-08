using System.Text;

namespace BMTLab.ImeiType.Tests.Unit;

public class ImeiParsingTest
{
#region Parse Tests
    [Theory]
    [InlineData(490154203237518)]
    [InlineData(356303489916807)]
    public void Parse_WithValidLong_ReturnsImei(long validImeiNumber)
    {
        // Act
        var imei = Imei.Parse(validImeiNumber);

        // Assert
        imei.ToInt64().Should().Be(validImeiNumber, "Parse should return an Imei instance with the correct value");
    }


    [Theory]
    [InlineData(123456789012345)]
    [InlineData(0)]
    public void Parse_WithInvalidLong_ThrowsFormatException(long invalidImeiNumber)
    {
        // Act
        Action act = () => _ = Imei.Parse(invalidImeiNumber);

        // Assert
        act.Should().Throw<FormatException>("Parse should throw FormatException for invalid IMEI");
    }


    [Theory]
    [InlineData("490154203237518")]
    [InlineData("356303489916807")]
    public void Parse_WithValidString_ReturnsImei(string validImeiString)
    {
        // Act
        var imei = Imei.Parse(validImeiString, provider: null);

        // Assert
        imei.ToString().Should().Be(validImeiString, "Parse should return an Imei instance with the correct value");
    }


    [Theory]
    [InlineData("123456789012345")]
    [InlineData("")]
    [InlineData(null)]
    public void Parse_WithInvalidString_ThrowsFormatException(string? invalidImeiString)
    {
        // Act
        Action act = () => _ = Imei.Parse(invalidImeiString!, provider: null);

        // Assert
        act.Should().Throw<FormatException>("Parse should throw FormatException for invalid IMEI");
    }


    [Fact]
    public void ParseUtf8_WithValidUtf8Span_ReturnsImei()
    {
        // Arrange
        var utf8Text = "490154203237518"u8;

        // Act
        var imei = Imei.Parse(utf8Text, provider: null);

        // Assert
        imei.ToInt64().Should().Be(expected: 490154203237518, "Parse should correctly parse a valid UTF8 span");
    }


    [Fact]
    public void ParseUtf8_WithInvalidUtf8Span_ThrowsFormatException()
    {
        // Arrange & Act
        var act = () =>
        {
            var utf8Text = "123456789012345"u8;
            _ = Imei.Parse(utf8Text, provider: null);
        };

        // Assert
        act.Should().Throw<FormatException>("Parse should throw FormatException for invalid UTF8 span");
    }
#endregion _Parse Tests


#region TryParse Tests
    [Theory]
    [InlineData(490154203237518, true)]
    [InlineData(123456789012345, false)]
    [InlineData(490154203123, false)]
    [InlineData(0, false)]
    public void TryParse_WithVariousLongs_ReturnsExpectedResult(long imeiNumber, bool expectedResult)
    {
        // Act
        var result = Imei.TryParse(imeiNumber, out var imei);

        // Assert
        result.Should().Be(expectedResult, $"TryParse should return '{expectedResult}' for IMEI number '{imeiNumber}'");

        if (expectedResult)
        {
            imei.ToInt64().Should().Be(imeiNumber, "parsed IMEI should match the input number");
        }
        else
        {
            imei.ToInt64().Should().Be(expected: 0, "parsed IMEI should be default when parsing fails");
        }
    }


    [Theory]
    [InlineData("490154203237518", true)]
    [InlineData("123456789012345", false)]
    [InlineData("490154203123", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void TryParse_WithVariousStrings_ReturnsExpectedResult(string? imeiString, bool expectedResult)
    {
        // Act
        var result = Imei.TryParse(imeiString, provider: null, out var imei);

        // Assert
        result.Should().Be(expectedResult, $"TryParse should return '{expectedResult}' for IMEI string '{imeiString}'");

        if (expectedResult)
        {
            imei.ToString().Should().Be(imeiString, "parsed IMEI should match the input string");
        }
        else
        {
            imei.ToInt64().Should().Be(expected: 0, "parsed IMEI should be default when parsing fails");
        }
    }


    [Theory]
    [InlineData("490154203237518", 490154203237518, true)]
    [InlineData("123456789012345", Imei.InvalidValue, false)]
    [InlineData("490154203123", Imei.InvalidValue, false)]
    [InlineData("", Imei.InvalidValue, false)]
    public void TryParseUtf8_WithVariousUtf8Texts_ReturnsExpectedResult
    (
        string? utf8TextString,
        long expectedValue,
        bool expectedResult
    )
    {
        // Arrange
        ReadOnlySpan<byte> utf8Text = Encoding.UTF8.GetBytes(utf8TextString!);

        // Act
        var result = Imei.TryParse(utf8Text, provider: null, out var imei);

        // Assert
        imei.ToInt64().Should().Be(expectedValue);
        result.Should().Be(expectedResult);
    }
#endregion _TryParse Tests


#region Parse Tests (No provider)
    // These tests duplicate the logic above, but cause overloads without the IFormatProvider directly.


    [Theory]
    [InlineData("490154203237518")]
    [InlineData("356303489916807")]
    public void ParseCharSpan_NoProvider_WithValidString_ReturnsImei(string validImeiString)
    {
        // Arrange
        var charSpan = validImeiString.AsSpan();

        // Act
        var imei = Imei.Parse(charSpan);

        // Assert
        imei.ToString().Should().Be(validImeiString,
            "Parse(ReadOnlySpan<char>) should return an Imei instance with the correct value");
    }


    [Theory]
    [InlineData("123456789012345")]
    [InlineData("")]
    public void ParseCharSpan_NoProvider_WithInvalidString_ThrowsFormatException(string invalidImeiString)
    {
        // Act
        var act = () =>
        {
            var charSpan = invalidImeiString.AsSpan();
            _ = Imei.Parse(charSpan);
        };

        // Assert
        act.Should().Throw<FormatException>(
            "Parse(ReadOnlySpan<char>) should throw for invalid IMEI");
    }


    [Theory]
    [InlineData("490154203237518")]
    [InlineData("356303489916807")]
    public void ParseString_NoProvider_WithValidString_ReturnsImei(string validImeiString)
    {
        // Act
        var imei = Imei.Parse(validImeiString);

        // Assert
        imei.ToString().Should().Be(validImeiString,
            "Parse(string) without provider should work like the main method");
    }


    [Theory]
    [InlineData("123456789012345")]
    [InlineData("")]
    [InlineData(null)]
    public void ParseString_NoProvider_WithInvalidString_ThrowsFormatException(string? invalidImeiString)
    {
        // Act
        Action act = () => _ = Imei.Parse(invalidImeiString!);

        // Assert
        act.Should().Throw<FormatException>(
            "Parse(string) without provider should throw for invalid IMEI");
    }


    [Fact]
    public void ParseUtf8_NoProvider_WithValidUtf8Span_ReturnsImei()
    {
        // Arrange
        var utf8Text = "490154203237518"u8;

        // Act
        var imei = Imei.Parse(utf8Text); // <-- перегрузка без IFormatProvider

        // Assert
        imei.ToInt64().Should().Be(expected: 490154203237518,
            "Parse(ReadOnlySpan<byte>) without provider should parse a valid UTF8 span");
    }


    [Fact]
    public void ParseUtf8_NoProvider_WithInvalidUtf8Span_ThrowsFormatException()
    {
        // Act
        var act = () =>
        {
            var utf8Text = "123456789012345"u8;
            _ = Imei.Parse(utf8Text);
        };

        // Assert
        act.Should().Throw<FormatException>(
            "Parse(ReadOnlySpan<byte>) without provider should throw for invalid IMEI");
    }
#endregion Parse Tests (No provider)


#region TryParse Tests (No provider)
    [Theory]
    [InlineData("490154203237518", true)]
    [InlineData("123456789012345", false)]
    [InlineData("", false)]
    public void TryParseCharSpan_NoProvider_ReturnsExpectedResult(string imeiString, bool expectedResult)
    {
        // Arrange
        var charSpan = imeiString.AsSpan();

        // Act
        var success = Imei.TryParse(charSpan, out var imei);

        // Assert
        success.Should().Be(expectedResult, $"TryParse(ReadOnlySpan<char>) without provider should return {expectedResult}");
        if (success)
        {
            imei.ToString().Should().Be(imeiString);
        }
        else
        {
            imei.ToInt64().Should().Be(0);
        }
    }


    [Theory]
    [InlineData("490154203237518", true)]
    [InlineData("123456789012345", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void TryParseString_NoProvider_ReturnsExpectedResult(string? imeiString, bool expectedResult)
    {
        // Act
        var success = Imei.TryParse(imeiString, out var imei);

        // Assert
        success.Should().Be(expectedResult, $"TryParse(string) without provider should return {expectedResult}");
        if (success)
        {
            imei.ToString().Should().Be(imeiString);
        }
        else
        {
            imei.ToInt64().Should().Be(0);
        }
    }


    [Theory]
    [InlineData("490154203237518", 490154203237518, true)]
    [InlineData("123456789012345", Imei.InvalidValue, false)]
    [InlineData("", Imei.InvalidValue, false)]
    public void TryParseUtf8_NoProvider_ReturnsExpectedResult(string imeiString, long expectedValue, bool expectedResult)
    {
        // Arrange
        ReadOnlySpan<byte> utf8Span = Encoding.UTF8.GetBytes(imeiString);

        // Act
        var success = Imei.TryParse(utf8Span, out var imei);

        // Assert
        success.Should().Be(expectedResult);
        imei.ToInt64().Should().Be(expectedValue);
    }
#endregion TryParse Tests (No provider)
}