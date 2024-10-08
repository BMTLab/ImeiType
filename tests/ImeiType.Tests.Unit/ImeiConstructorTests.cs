using System.Text;

namespace BMTLab.ImeiType.Tests.Unit;

public class ImeiConstructorTests
{
    [Fact]
    public void DefaultImei_IsInvalid()
    {
        // Arrange
        var imei = default(Imei);

        // Act
        var value = imei.ToInt64();

        // Assert
        value.Should().Be(expected: 0, "default Imei should have a value of 0");
        Imei.IsValid(value).Should().BeFalse("default Imei should be invalid");
    }


    [Theory]
    [InlineData(490154203237518)]
    [InlineData(356303489916807)]
    public void Constructor_WithValidLong_CreatesImei(long validImeiNumber)
    {
        // Arrange & Act
        var imei = new Imei(validImeiNumber);

        // Assert
        imei.ToInt64().Should().Be(validImeiNumber, "the constructor should store the valid IMEI number");
    }


    [Theory]
    [InlineData(123456789012345)]
    [InlineData(0)]
    [InlineData(long.MaxValue)]
    public void Constructor_WithInvalidLong_ThrowsFormatException(long invalidImeiNumber)
    {
        // Arrange & Act
        Action act = () => _ = new Imei(invalidImeiNumber);

        // Assert
        act.Should().Throw<FormatException>("the constructor should validate the IMEI number");
    }


    [Theory]
    [InlineData("490154203237518")]
    [InlineData("356303489916807")]
    public void Constructor_WithValidString_CreatesImei(string validImeiString)
    {
        // Arrange & Act
        var imei = new Imei(validImeiString);

        // Assert
        imei.ToString().Should().Be(validImeiString, "the constructor should store the valid IMEI string");
    }


    [Theory]
    [InlineData("123456789012345")]
    [InlineData("")]
    [InlineData(null)]
    public void Constructor_WithInvalidString_ThrowsFormatException(string? invalidImeiString)
    {
        // Arrange & Act
        Action act = () => _ = new Imei(invalidImeiString!);

        // Assert
        act.Should().Throw<FormatException>("the constructor should validate the IMEI string");
    }


    [Theory]
    [InlineData("490154203237518")]
    [InlineData("356303489916807")]
    public void Constructor_WithValidReadOnlySpanChar_CreatesImei(string validImeiString)
    {
        // Arrange
        var imeiSpan = validImeiString.AsSpan();

        // Act
        var imei = new Imei(imeiSpan);

        // Assert
        imei.ToInt64().Should().Be(long.Parse(validImeiString), "constructor should correctly handle ReadOnlySpan<char>");
    }


    [Theory]
    [InlineData("123456789012345")]
    [InlineData("")]
    public void Constructor_WithInvalidReadOnlySpanChar_ThrowsFormatException(string invalidImeiString)
    {
        // Arrange & Act
        var act = () =>
        {
            var invalidImeiSpan = invalidImeiString.AsSpan();

            _ = new Imei(invalidImeiSpan);
        };

        // Assert
        act.Should().Throw<FormatException>("the constructor should validate the IMEI string");
    }


    [Theory]
    [InlineData("490154203237518")]
    [InlineData("356303489916807")]
    public void Constructor_WithValidReadOnlySpanByte_CreatesImei(string validImeiString)
    {
        // Arrange
        ReadOnlySpan<byte> imeiUtf8Span = Encoding.UTF8.GetBytes(validImeiString);

        // Act
        var imei = new Imei(imeiUtf8Span);

        // Assert
        imei.ToInt64().Should().Be(long.Parse(validImeiString), "constructor should correctly handle ReadOnlySpan<byte>");
    }


    [Theory]
    [InlineData("4901542032375")]
    [InlineData("123456789012345")]
    [InlineData("")]
    public void Constructor_WithInvalidReadOnlySpanByte_ThrowsFormatException(string invalidImeiString)
    {
        // Arrange & Act
        var act = () =>
        {
            ReadOnlySpan<byte> invalidImeiUtf8Span = Encoding.UTF8.GetBytes(invalidImeiString);

            _ = new Imei(invalidImeiUtf8Span);
        };

        // Assert
        act.Should().Throw<FormatException>("the constructor should validate the IMEI string");
    }
}