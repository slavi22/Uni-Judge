using System.Text;
using Newtonsoft.Json;

namespace BE.Util.JsonConverters;

public class Base64Converter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        writer.WriteValue(Convert.ToBase64String(Encoding.UTF8.GetBytes((string)value)));
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String((string)reader.Value));
    }

    public override bool CanConvert(Type objectType)
    {
        return true;
    }
}