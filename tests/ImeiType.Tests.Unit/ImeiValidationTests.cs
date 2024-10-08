namespace BMTLab.ImeiType.Tests.Unit;

public class ImeiValidationTests
{
    [Theory]
    [InlineData(490154203237518, true)]
    [InlineData(-490154203237518, false)]
    [InlineData(123456789012345, false)]
    [InlineData(0, false)]
    [InlineData(long.MaxValue, false)]
    public void IsValid_WithVariousLongs_ReturnsExpectedResult(long imeiNumber, bool expectedValidity)
    {
        // Act
        var isValid = Imei.IsValid(imeiNumber);

        // Assert
        isValid.Should().Be(expectedValidity, $"IMEI number '{imeiNumber}' validity should be '{expectedValidity}'");
    }


    [Theory]
    [InlineData("490154203237518", true)]
    [InlineData("123456789012345", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValid_WithVariousStrings_ReturnsExpectedResult(string? imeiString, bool expectedValidity)
    {
        // Act
        var isValid = Imei.IsValid(imeiString!);

        // Assert
        isValid.Should().Be(expectedValidity, $"IMEI string '{imeiString}' validity should be '{expectedValidity}'");
    }
}