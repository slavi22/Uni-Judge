using System.Text;
using Newtonsoft.Json;

namespace BE.Util.JsonConverters;

public class PlainTextConverter : JsonConverter
{
    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        if (reader.Value != null)
        {
            var valueInBytes = Convert.FromBase64String(reader.Value.ToString());
            return Encoding.UTF8.GetString(valueInBytes);
        }

        return existingValue;
    }

    public override bool CanConvert(Type objectType)
    {
        return true;
    }
}