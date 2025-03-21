namespace BMTLab.ImeiType.SystemTextJson.Tests.Unit;

public class JsonImeiConverterAttributeTests
{
    [Theory]
    [InlineData(JsonTokenType.String)]
    [InlineData(JsonTokenType.Number)]
    public void Deserialize_ShouldHandleImeiField_WhenItIsValid_WhenNoOptions
    (
        JsonTokenType jsonType
    )
    {
        // Arrange
        var toDeserialize =
            $$"""
              {
                "val": {{WriteJsonTokenAs(jsonType, val: 490154203237518)}}
              }
              """;

        // Act
        var actualObject = Deserialize<SomeClass>(toDeserialize);

        // Assert
        actualObject
           .Should().BeOfType<SomeClass>()
           .Which.Imei.Should().BeOfType<Imei>()
           .And.Be((Imei) 490154203237518);
    }


    [Fact]
    public void Serialize_ShouldHandleImeiField_AsNumberByDefault()
    {
        // Arrange
        var expectedJson =
            $$"""
                  {
                    "val": {{WriteJsonTokenAs(JsonTokenType.Number, val: 490154203237518)}}
                  }
                  """.Minify();

        var toSerialize = new SomeClass
        {
            Imei = (Imei) 490154203237518
        };

        // Act
        var json = Serialize(toSerialize);

        // Assert
        json.Should().Be(expectedJson);
    }
}


public class SomeClass
{
    [JsonPropertyName("val")]
    [JsonConverter(typeof(JsonImeiConverter))]
    public Imei Imei { get; set; }
}