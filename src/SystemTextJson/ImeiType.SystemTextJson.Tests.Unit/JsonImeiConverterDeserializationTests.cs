using System.Text.Json;

using BMTLab.ImeiType.SystemTextJson.Extensions;

namespace BMTLab.ImeiType.SystemTextJson.Tests.Unit;

public class JsonImeiConverterDeserializationTests
{
    private static readonly JsonSerializerOptions Options =
        new JsonSerializerOptions().WithImeiConverter();


    [Fact]
    public void Deserialize_ShouldHandleImeiField_WhenItIsNull()
    {
        // Arrange
        const string toDeserialize =
            """
            {
              "val": null
            }
            """;

        // Act
        var actualObject = JsonSerializer.Deserialize<NullableImeiContainer>(toDeserialize, Options);

        // Assert
        actualObject
           .Should().BeOfType<NullableImeiContainer>()
           .Which.Imei.Should().BeNull();
    }


    [Fact]
    public void Deserialize_ShouldHandleValidImeiField_WhenAsText()
    {
        // Arrange
        const string toDeserialize =
            """
            {
              "val": "490154203237518"
            }
            """;

        // Act
        var actualObject = JsonSerializer.Deserialize<ImeiContainer>(toDeserialize, Options);

        // Assert
        actualObject
           .Should().BeOfType<ImeiContainer>()
           .Which.Imei.Should().BeOfType<Imei>()
           .And.Be((Imei) 490154203237518);
    }


    [Fact]
    public void Deserialize_ShouldHandleValidImeiField_WhenAsNumber()
    {
        // Arrange
        const string toDeserialize =
            """
            {
              "val": 490154203237518
            }
            """;

        // Act
        var actualObject = JsonSerializer.Deserialize<ImeiContainer>(toDeserialize, Options);

        // Assert
        actualObject
           .Should().BeOfType<ImeiContainer>()
           .Which.Imei.Should().BeOfType<Imei>()
           .And.Be((Imei) 490154203237518);
    }


    [Fact]
    public void Deserialize_ShouldHandleInvalidImeiField_WhenAsText()
    {
        // Arrange
        const string toDeserialze =
            """
            {
              "val": "abc"
            }
            """;

        // Act
        Action act = () => _ = JsonSerializer.Deserialize<ImeiContainer>(toDeserialze, Options);

        // Assert
        act
           .Should().ThrowExactly<JsonException>()
           .Which.InnerException.Should().BeOfType<FormatException>();
    }


    [Fact]
    public void Deserialize_ShouldHandleNullableValidImeiField_WhenAsText()
    {
        // Arrange
        const string toDeserialize =
            """
            {
              "val": "490154203237518"
            }
            """;

        // Act
        var actualObject = JsonSerializer.Deserialize<NullableImeiContainer>(toDeserialize, Options);

        // Assert
        actualObject
           .Should().BeOfType<NullableImeiContainer>()
           .Which.Imei.Should().BeOfType<Imei>()
           .And.Be((Imei) 490154203237518);
    }


    [Fact]
    public void Deserialize_ShouldHandleNullableValidImeiField_WhenAsNumber()
    {
        // Arrange
        const string toDeserialize =
            """
            {
              "val": 490154203237518
            }
            """;

        // Act
        var actualObject = JsonSerializer.Deserialize<NullableImeiContainer>(toDeserialize, Options);

        // Assert
        actualObject
           .Should().BeOfType<NullableImeiContainer>()
           .Which.Imei.Should().BeOfType<Imei>()
           .And.Be((Imei) 490154203237518);
    }


    [Fact]
    public void Deserialize_ShouldHandleImeiList_WhenItemsAreImei()
    {
        // Arrange
        const string json =
            """
            {
              "val": [
                "490154203237518",
                356303489916807
              ]
            }
            """;

        // Act
        var actualObject = JsonSerializer.Deserialize<ImeiListContainer>(json, Options);

        // Assert
        actualObject
           .Should().BeOfType<ImeiListContainer>()
           .Which.ImeiList.Should().SatisfyRespectively(
                itemA => itemA.Should().Be((Imei) 490154203237518),
                itemB => itemB.Should().Be((Imei) 356303489916807)
            );
    }


    [Fact]
    public void Deserialize_ShouldHandleImeiDictionary_WhenKeysAreImei()
    {
        // Arrange
        const string toDeserialize =
            """
            {
              "val": {
                "490154203237518": "A",
                "356303489916807": "B",
                "356656423384345": null
              }
            }
            """;

        // Act
        var actualObject = JsonSerializer.Deserialize<ImeiDictionaryContainer<Imei, string?>>(toDeserialize, Options);

        // Assert
        actualObject
           .Should().BeOfType<ImeiDictionaryContainer<Imei, string?>>()
           .Which.ImeiDictionary.Should().SatisfyRespectively(
                pairA =>
                {
                    pairA.Key.Should().Be((Imei) 490154203237518);
                    pairA.Value.Should().Be("A");
                },
                pairB =>
                {
                    pairB.Key.Should().Be((Imei) 356303489916807);
                    pairB.Value.Should().Be("B");
                },
                pairC =>
                {
                    pairC.Key.Should().Be((Imei) 356656423384345);
                    pairC.Value.Should().BeNull();
                }
            );
    }


    [Fact]
    public void Deserialize_ShouldHandleImeiDictionary_WhenValuesAreImei()
    {
        // Arrange
        const string toDeserialze =
            """
            {
              "val": {
                "A": "490154203237518",
                "B": 356303489916807,
                "C": null
              }
            }
            """;

        // Act
        var actualObject = JsonSerializer.Deserialize<ImeiDictionaryContainer<string, Imei?>>(toDeserialze, Options);

        // Assert
        actualObject
           .Should().BeOfType<ImeiDictionaryContainer<string, Imei?>>()
           .Which.ImeiDictionary.Should().SatisfyRespectively(
                pairA =>
                {
                    pairA.Key.Should().Be("A");
                    pairA.Value.Should().Be((Imei) 490154203237518);
                },
                pairB =>
                {
                    pairB.Key.Should().Be("B");
                    pairB.Value.Should().Be((Imei) 356303489916807);
                },
                pairC =>
                {
                    pairC.Key.Should().Be("C");
                    pairC.Value.Should().BeNull();
                }
            );
    }
}