namespace BMTLab.ImeiType.SystemTextJson.Tests.Unit;

public class JsonImeiConverterSerializationTests
{
#if NET7_0_OR_GREATER
    [StringSyntax(StringSyntaxAttribute.Json)]
#endif
    private string? _expectedJson;


    private static JsonSerializerOptions GetOptions
    (
        JsonImeiWriteOptions imeiWriteOptions = Default,
        JsonNumberHandling? globalWriteOptions = null
    ) =>
        new JsonSerializerOptions
        {
            WriteIndented = false,
            NumberHandling = AllowReadingFromString | (globalWriteOptions ?? AllowReadingFromString)
        }.WithImeiConverter(imeiWriteOptions);


    [Theory]
    [InlineData(Default, null, JsonTokenType.Number)]
    [InlineData(Default, WriteAsString, JsonTokenType.String)]
    [InlineData(ForceWriteAsNumber, null, JsonTokenType.Number)]
    [InlineData(ForceWriteAsNumber, WriteAsString, JsonTokenType.Number)]
    [InlineData(ForceWriteAsString, null, JsonTokenType.String)]
    [InlineData(ForceWriteAsString, WriteAsString, JsonTokenType.String)]
    public void Serialize_ShouldHandleImeiField_WithDifferentWritingOptions
    (
        JsonImeiWriteOptions imeiWriteOptions,
        JsonNumberHandling? globalWriteOptions,
        JsonTokenType expectedJsonType
    )
    {
        // Arrange
        _expectedJson =
            $$"""
                  {
                    "val": {{WriteAs(expectedJsonType, val: 490154203237518)}}
                  }
                  """.Minify();

        var toSerialize = new ImeiContainer
        {
            Imei = (Imei) 490154203237518
        };

        // Act
        var json = Serialize(toSerialize, GetOptions(imeiWriteOptions, globalWriteOptions));

        // Assert
        json.Should().Be(_expectedJson);
    }


    [Theory]
    [InlineData(Default, null, JsonTokenType.Number)]
    [InlineData(Default, WriteAsString, JsonTokenType.String)]
    [InlineData(ForceWriteAsNumber, null, JsonTokenType.Number)]
    [InlineData(ForceWriteAsNumber, WriteAsString, JsonTokenType.Number)]
    [InlineData(ForceWriteAsString, null, JsonTokenType.String)]
    [InlineData(ForceWriteAsString, WriteAsString, JsonTokenType.String)]
    public void Serialize_ShouldHandleNullableImeiField_WithDifferentWritingOptions
    (
        JsonImeiWriteOptions imeiWriteOptions,
        JsonNumberHandling? globalWriteOptions,
        JsonTokenType expectedJsonType
    )
    {
        // Arrange
        _expectedJson =
            $$"""
                  {
                    "val": {{WriteAs(expectedJsonType, val: 490154203237518)}}
                  }
                  """.Minify();

        var toSerialize = new NullableImeiContainer
        {
            Imei = (Imei) 490154203237518
        };

        // Act
        var json = Serialize(toSerialize, GetOptions(imeiWriteOptions, globalWriteOptions));

        // Assert
        json.Should().Be(_expectedJson);
    }


    [Fact]
    public void Serialize_ShouldHandleNullableImeiField_WhenItIsNull()
    {
        // Arrange
        _expectedJson =
            """
                {
                  "val": null
                }
                """.Minify();

        var toSerialize = new NullableImeiContainer
        {
            Imei = null
        };

        // Act
        var json = Serialize(toSerialize, GetOptions());

        // Assert
        json.Should().Be(_expectedJson);
    }


    [Theory]
    [InlineData(Default, null, JsonTokenType.Number)]
    [InlineData(Default, WriteAsString, JsonTokenType.String)]
    [InlineData(ForceWriteAsNumber, null, JsonTokenType.Number)]
    [InlineData(ForceWriteAsNumber, WriteAsString, JsonTokenType.Number)]
    [InlineData(ForceWriteAsString, null, JsonTokenType.String)]
    [InlineData(ForceWriteAsString, WriteAsString, JsonTokenType.String)]
    public void Serialize_ShouldHandleImeiList_WithDifferentWritingOptions
    (
        JsonImeiWriteOptions imeiWriteOptions,
        JsonNumberHandling? globalWriteOptions,
        JsonTokenType expectedJsonType
    )
    {
        // Arrange
        _expectedJson =
            $$"""
                  {
                    "val": [
                      {{WriteAs(expectedJsonType, val: 490154203237518)}},
                      {{WriteAs(expectedJsonType, val: 356303489916807)}},
                      null
                    ]
                  }
                  """.Minify();

        var toSerialize = new ImeiListContainer
        {
            ImeiList =
            [
                (Imei) 490154203237518,
                (Imei) 356303489916807,
                null
            ]
        };

        // Act
        var json = Serialize(toSerialize, GetOptions(imeiWriteOptions, globalWriteOptions));

        // Assert
        json.Should().Be(_expectedJson);
    }


    [Theory]
    [InlineData(Default, null)]
    [InlineData(Default, WriteAsString)]
    [InlineData(ForceWriteAsNumber, null)]
    [InlineData(ForceWriteAsNumber, WriteAsString)]
    [InlineData(ForceWriteAsString, null)]
    [InlineData(ForceWriteAsString, WriteAsString)]
    public void Serialize_ShouldHandleImeiDictionary_WhenKeysAreImei_WithDifferentWritingOptions
    (
        JsonImeiWriteOptions imeiWriteOptions,
        JsonNumberHandling? globalWriteOptions
    )
    {
        // Arrange
        _expectedJson =
            $$"""
                  {
                    "val": {
                      {{WriteAs(JsonTokenType.String, val: 490154203237518)}}: "A",
                      {{WriteAs(JsonTokenType.String, val: 356303489916807)}}: "B",
                      {{WriteAs(JsonTokenType.String, val: 356656423384345)}}: null
                    }
                  }
                  """.Minify();

        var toSerialize = new ImeiDictionaryContainer<Imei, string?>
        {
            ImeiDictionary = new Dictionary<Imei, string?>
            {
                [(Imei) 490154203237518] = "A",
                [(Imei) 356303489916807] = "B",
                [(Imei) 356656423384345] = null
            }
        };

        // Act
        var json = Serialize(toSerialize, GetOptions(imeiWriteOptions, globalWriteOptions));

        // Assert
        json.Should().Be(_expectedJson);
    }


    [Theory]
    [InlineData(Default, null, JsonTokenType.Number)]
    [InlineData(Default, WriteAsString, JsonTokenType.String)]
    [InlineData(ForceWriteAsNumber, null, JsonTokenType.Number)]
    [InlineData(ForceWriteAsNumber, WriteAsString, JsonTokenType.Number)]
    [InlineData(ForceWriteAsString, null, JsonTokenType.String)]
    [InlineData(ForceWriteAsString, WriteAsString, JsonTokenType.String)]
    public void Serialize_ShouldHandleImeiDictionary_WhenValuesAreImei_WithDifferentWritingOptions
    (
        JsonImeiWriteOptions imeiWriteOptions,
        JsonNumberHandling? globalWriteOptions,
        JsonTokenType expectedJsonType
    )
    {
        // Arrange
        _expectedJson =
            $$"""
                  {
                    "val": {
                      "A": {{WriteAs(expectedJsonType, val: 490154203237518)}},
                      "B": {{WriteAs(expectedJsonType, val: 356303489916807)}},
                      "C": null
                    }
                  }
                  """.Minify();

        var toSerialize = new ImeiDictionaryContainer<string, Imei?>
        {
            ImeiDictionary = new Dictionary<string, Imei?>
            {
                ["A"] = (Imei) 490154203237518,
                ["B"] = (Imei) 356303489916807,
                ["C"] = null
            }
        };

        // Act
        var json = Serialize(toSerialize, GetOptions(imeiWriteOptions, globalWriteOptions));

        // Assert
        json.Should().Be(_expectedJson);
    }
}