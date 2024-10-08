namespace BMTLab.ImeiType.Tests.Unit;

public class ImeiValidationFlagTests
{
    [Fact]
    public void ShouldValidateWhileInitialization_Disabled_AllowsInvalidImei()
    {
        try
        {
            // Arrange
            Imei.ShouldValidateWhileInitialization = false;
            const long invalidImeiNumber = 123456789012345;

            // Act
            var imei = new Imei(invalidImeiNumber);

            // Assert
            imei.ToInt64().Should().Be(invalidImeiNumber, "constructor should not validate IMEI when validation is disabled");
        }
        // Clean up
        finally
        {
            Imei.ShouldValidateWhileInitialization = true;
        }
    }


    [Fact]
    public void ShouldValidateWhileInitialization_Enabled_ThrowsFormatException()
    {
        try
        {
            // Arrange
            Imei.ShouldValidateWhileInitialization = true;
            const long invalidImeiNumber = 123456789012345;

            // Act
            Action act = () => _ = new Imei(invalidImeiNumber);

            // Assert
            act.Should().Throw<FormatException>("constructor should validate IMEI when validation is enabled");
        }
        // Clean up
        finally
        {
            Imei.ShouldValidateWhileInitialization = true;
        }
    }
}