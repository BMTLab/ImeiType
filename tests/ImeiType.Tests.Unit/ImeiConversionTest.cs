namespace BMTLab.ImeiType.Tests.Unit;

public class ImeiConversionTest
{
    [Fact]
    public void ToReadOnlySpan_ReturnsCorrectSpan()
    {
        // Arrange
        var imei = new Imei(490154203237518);
        var expectedSpan = "490154203237518".AsSpan();

        // Act
        var span = imei.ToReadOnlySpan();

        // Assert
        span.ToString().Should().Be(expectedSpan.ToString(), "ToReadOnlySpan should return the correct ReadOnlySpan<char>");
    }


    [Fact]
    public void ToInt64_ReturnsCorrectValue()
    {
        // Arrange
        var imei = new Imei(490154203237518);

        // Act
        var number = imei.ToInt64();

        // Assert
        number.Should().Be(expected: 490154203237518, "ToInt64 should return the stored IMEI number");
    }


    [Fact]
    public void ToString_ReturnsCorrectString()
    {
        // Arrange
        var imei = new Imei(490154203237518);

        // Act
        var imeiString = imei.ToString();

        // Assert
        imeiString.Should().Be("490154203237518", "ToString should return the string representation of the IMEI");
    }



#region Conversion Operators Tests
    [Theory]
    [InlineData(490154203237518)]
    [InlineData(356303489916807)]
    public void ExplicitConversion_FromValidLong_CreatesImei(long validImeiNumber)
    {
        // Act
        var imei = (Imei) validImeiNumber;

        // Assert
        imei.ToInt64().Should().Be(validImeiNumber, "explicit conversion should create an Imei with the correct value");
    }


    [Theory]
    [InlineData(123456789012345)]
    [InlineData(0)]
    public void ExplicitConversion_FromInvalidLong_ThrowsFormatException(long invalidImeiNumber)
    {
        // Act
        Action act = () => _ = (Imei) invalidImeiNumber;

        // Assert
        act.Should().Throw<FormatException>("explicit conversion should validate the IMEI number");
    }


    [Fact]
    public void ImplicitConversion_ToString_ReturnsStringRepresentation()
    {
        // Arrange
        var imei = new Imei(490154203237518);

        // Act
        string imeiString = imei;

        // Assert
        imeiString.Should().Be("490154203237518", "implicit conversion should return the string representation of the IMEI");
    }


    [Fact]
    public void ImplicitConversion_ToReadOnlySpanChar_ReturnsCorrectSpan()
    {
        // Arrange
        var imei = new Imei(490154203237518);
        var expectedSpan = "490154203237518".AsSpan();

        // Act
        ReadOnlySpan<char> span = imei;

        // Assert
        span.ToArray().Should().Equal(expectedSpan.ToArray(), "implicit conversion should return the correct ReadOnlySpan<char>");
    }


    [Fact]
    public void ImplicitConversion_ToReadOnlySpanByte_ReturnsCorrectSpan()
    {
        // Arrange
        var imei = new Imei(490154203237518);
        var expectedBytes = "490154203237518"u8;

        // Act
        ReadOnlySpan<byte> span = imei;

        // Assert
        span.ToArray().Should().Equal(expectedBytes.ToArray(), "implicit conversion should return the correct ReadOnlySpan<byte>");
    }
#endregion _Conversion Operators Tests
}