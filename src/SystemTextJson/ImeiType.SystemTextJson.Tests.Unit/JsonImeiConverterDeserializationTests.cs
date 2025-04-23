namespace BMTLab.ImeiType.SystemTextJson.Tests.Unit;

public class JsonImeiConverterDeserializationTests
{
    private static readonly JsonSerializerOptions Options =
        new JsonSerializerOptions().WithImeiConverter();

#if NET7_0_OR_GREATER
    [StringSyntax(StringSyntaxAttribute.Json)]
#endif
    private string? _toDeserialize;


    [Theory]
    [InlineData(JsonTokenType.String)]
    [InlineData(JsonTokenType.Number)]
    public void Deserialize_ShouldHandleImeiField_WhenItIsValid
    (
        JsonTokenType jsonType
    )
    {
        // Arrange
        _toDeserialize =
            $$"""
              {
                "val": {{WriteAs(jsonType, val: 490154203237518)}}
              }
              """;

        // Act
        var actualObject = Deserialize<ImeiContainer>(_toDeserialize, Options);

        // Assert
        actualObject
           .Should().BeOfType<ImeiContainer>()
           .Which.Imei.Should().BeOfType<Imei>()
           .And.Be((Imei) 490154203237518);
    }


    [Theory]
    [InlineData(JsonTokenType.String)]
    [InlineData(JsonTokenType.Number)]
    public void Deserialize_ShouldHandleNullableImeiField_WhenItIsValid
    (
        JsonTokenType jsonType
    )
    {
        // Arrange
        _toDeserialize =
            $$"""
              {
                "val": {{WriteAs(jsonType, val: 490154203237518)}}
              }
              """;

        // Act
        var actualObject = Deserialize<NullableImeiContainer>(_toDeserialize, Options);

        // Assert
        actualObject
           .Should().BeOfType<NullableImeiContainer>()
           .Which.Imei.Should().BeOfType<Imei>()
           .And.Be((Imei) 490154203237518);
    }


    [Fact]
    public void Deserialize_ShouldHandleNullableImeiField_WhenItIsNull()
    {
        // Arrange
        _toDeserialize =
            """
            {
              "val": null
            }
            """;

        // Act
        var actualObject = Deserialize<NullableImeiContainer>(_toDeserialize, Options);

        // Assert
        actualObject
           .Should().BeOfType<NullableImeiContainer>()
           .Which.Imei.Should().BeNull();
    }


    [Theory]
    [InlineData("abc", JsonTokenType.String)]
    [InlineData(111111111111111, JsonTokenType.Number)]
    [InlineData(null, JsonTokenType.Null)]
    public void Deserialize_ShouldHandleImeiField_WhenItIsInvalid
    (
        object? invalidValue,
        JsonTokenType jsonType
    )
    {
        // Arrange
        _toDeserialize =
            $$"""
              {
                "val": {{WriteAs(jsonType, invalidValue)}}
              }
              """;

        // Act
        Action act = () => _ = Deserialize<ImeiContainer>(_toDeserialize, Options);

        // Assert
        act
           .Should().ThrowExactly<JsonException>()
           .Which.InnerException.Should().BeOfType<FormatException>();
    }


    [Fact]
    public void Deserialize_ShouldHandleImeiList_WhenItemsAreImei()
    {
        // Arrange
        _toDeserialize =
            """
            {
              "val": [
                "490154203237518",
                356303489916807,
                null
              ]
            }
            """;

        // Act
        var actualObject = Deserialize<ImeiListContainer>(_toDeserialize, Options);

        // Assert
        actualObject
           .Should().BeOfType<ImeiListContainer>()
           .Which.ImeiList.Should().SatisfyRespectively(
                itemA => itemA.Should().Be((Imei) 490154203237518),
                itemB => itemB.Should().Be((Imei) 356303489916807),
                itemC => itemC.Should().BeNull()
            );
    }


    [Fact]
    public void Deserialize_ShouldHandleImeiDictionary_WhenKeysAreImei()
    {
        // Arrange
        _toDeserialize =
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
        var actualObject = Deserialize<ImeiDictionaryContainer<Imei, string?>>(_toDeserialize, Options);

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
        _toDeserialize =
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
        var actualObject = Deserialize<ImeiDictionaryContainer<string, Imei?>>(_toDeserialize, Options);

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