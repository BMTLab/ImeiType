namespace BMTLab.ImeiType.Tests.Unit;

public class ImeiEqualityTests
{
    [Theory]
    [InlineData(490154203237518, 490154203237518, true)]
    [InlineData(490154203237518, 356303489916807, false)]
    public void Equals_WithVariousImeis_ReturnsExpectedResult(long imeiNumberA, long imeiNumberB, bool expectedEquality)
    {
        // Arrange
        var imeiA = new Imei(imeiNumberA);
        var imeiB = new Imei(imeiNumberB);

        // Act
        var areEqual = imeiA.Equals(imeiB);

        // Assert
        areEqual.Should().Be(expectedEquality, $"IMEIs '{imeiNumberA}' and '{imeiNumberB}' equality should be '{expectedEquality}'");
        (imeiA == imeiB).Should().Be(expectedEquality, "operator == should match Equals method");
        (imeiA != imeiB).Should().Be(!expectedEquality, "operator != should be the inverse of operator ==");
    }


    [Theory]
    [InlineData(490154203237518, 490154203237518, true)]
    [InlineData(490154203237518, 356303489916807, false)]
    public void GetHashCode_WithVariousImeis_ReturnsExpectedResult(long imeiNumberA, long imeiNumberB, bool expectSameHash)
    {
        // Arrange
        var imeiA = new Imei(imeiNumberA);
        var imeiB = new Imei(imeiNumberB);

        // Act
        var hashA = imeiA.GetHashCode();
        var hashB = imeiB.GetHashCode();

        // Assert
        if (expectSameHash)
        {
            hashA.Should().Be(hashB, "IMEIs with the same value should have the same hash code");
        }
        else
        {
            hashA.Should().NotBe(hashB, "IMEIs with different values should have different hash codes");
        }
    }
}