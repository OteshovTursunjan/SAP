using System.Text.Json;
using System.Text.Json.Serialization;
namespace SAP.Application.Service.lmpl;


public class IntOrStringToIntConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number => reader.GetInt32(),
            JsonTokenType.String => int.TryParse(reader.GetString(), out var value) ? value : throw new JsonException("Invalid int string"),
            _ => throw new JsonException("Unexpected token type")
        };
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}
