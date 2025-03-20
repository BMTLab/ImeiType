namespace BMTLab.ImeiType.SystemTextJson.Tests.Unit;

public class JsonImeiConverterSerializationTests
{
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


    /// <summary>
    ///     Method needed only for a correct and convenient way to compose a expected JSON value.
    /// </summary>
    [SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance")]
    private static object WriteImeiAs(Type type, in long val)
    {
        if (type == typeof(string))
        {
            return "\"" + val + "\"";
        }

        if (type == typeof(long))
        {
            return val;
        }

        throw new NotSupportedException
        (
            $"It allowed to embed the IMEI value into JSON, only as {typeof(string)} or as {typeof(long)}, but not as {type}."
        );
    }


    [Theory]
    [InlineData(Default, null, typeof(long))]
    [InlineData(Default, WriteAsString, typeof(string))]
    [InlineData(ForceWriteAsNumber, null, typeof(long))]
    [InlineData(ForceWriteAsNumber, WriteAsString, typeof(long))]
    [InlineData(ForceWriteAsString, null, typeof(string))]
    [InlineData(ForceWriteAsString, WriteAsString, typeof(string))]
    public void Serialize_ShouldHandleImeiField_WithDifferentWritingOptions
    (
        JsonImeiWriteOptions imeiWriteOptions,
        JsonNumberHandling? globalWriteOptions,
        Type expectedJsonType
    )
    {
        // Arrange
        var expectedJson =
            $$"""
                  {
                    "val": {{WriteImeiAs(expectedJsonType, val: 490154203237518)}}
                  }
                  """.Minify();

        var toSerialize = new ImeiContainer
        {
            Imei = (Imei) 490154203237518
        };

        // Act
        var json = Serialize(toSerialize, GetOptions(imeiWriteOptions, globalWriteOptions));

        // Assert
        json.Should().Be(expectedJson);
    }


    [Theory]
    [InlineData(Default, null, typeof(long))]
    [InlineData(Default, WriteAsString, typeof(string))]
    [InlineData(ForceWriteAsNumber, null, typeof(long))]
    [InlineData(ForceWriteAsNumber, WriteAsString, typeof(long))]
    [InlineData(ForceWriteAsString, null, typeof(string))]
    [InlineData(ForceWriteAsString, WriteAsString, typeof(string))]
    public void Serialize_ShouldHandleImeiList_WithDifferentWritingOptions
    (
        JsonImeiWriteOptions imeiWriteOptions,
        JsonNumberHandling? globalWriteOptions,
        Type expectedJsonType
    )
    {
        // Arrange
        var expectedJson =
            $$"""
                  {
                    "val": [
                      {{WriteImeiAs(expectedJsonType, val: 490154203237518)}},
                      {{WriteImeiAs(expectedJsonType, val: 356303489916807)}}
                    ]
                  }
                  """.Minify();

        var toSerialize = new ImeiListContainer
        {
            ImeiList =
            [
                (Imei) 490154203237518,
                (Imei) 356303489916807
            ]
        };

        // Act
        var json = Serialize(toSerialize, GetOptions(imeiWriteOptions, globalWriteOptions));

        // Assert
        json.Should().Be(expectedJson);
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
        var expectedJson =
            $$"""
                  {
                    "val": {
                      {{WriteImeiAs(typeof(string), val: 490154203237518)}}: "A",
                      {{WriteImeiAs(typeof(string), val: 356303489916807)}}: "B",
                      {{WriteImeiAs(typeof(string), val: 356656423384345)}}: null
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
        json.Should().Be(expectedJson);
    }


    [Theory]
    [InlineData(Default, null, typeof(long))]
    [InlineData(Default, WriteAsString, typeof(string))]
    [InlineData(ForceWriteAsNumber, null, typeof(long))]
    [InlineData(ForceWriteAsNumber, WriteAsString, typeof(long))]
    [InlineData(ForceWriteAsString, null, typeof(string))]
    [InlineData(ForceWriteAsString, WriteAsString, typeof(string))]
    public void Serialize_ShouldHandleImeiDictionary_WhenValuesAreImei_WithDifferentWritingOptions
    (
        JsonImeiWriteOptions imeiWriteOptions,
        JsonNumberHandling? globalWriteOptions,
        Type expectedJsonType
    )
    {
        // Arrange
        var expectedJson =
            $$"""
                  {
                    "val": {
                      "A": {{WriteImeiAs(expectedJsonType, val: 490154203237518)}},
                      "B": {{WriteImeiAs(expectedJsonType, val: 356303489916807)}},
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
        json.Should().Be(expectedJson);
    }
}