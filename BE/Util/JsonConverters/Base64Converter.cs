using System.Text;
using Newtonsoft.Json;

namespace BE.Util.JsonConverters;

//https://stackoverflow.com/a/46040273
public class Base64Converter : JsonConverter
{
    //public override bool CanRead => false;
    //https://code-maze.com/json-dotnet-create-custom-jsonconverter/#:~:text=By%20overriding%20CanWrite,called%20on%20deserialization.
    public override bool CanRead => false;

    // this is used for serializing the json, in this case it will never be called as we set "CanWrite" to false
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        writer.WriteValue(Convert.ToBase64String(Encoding.UTF8.GetBytes((string)value)));
        //throw new NotImplementedException();
    }

    // this is used when deserializing the json (when i receive the model in the controller it will convert the values annotated with this converter to base64)
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var valueInBytes = Encoding.UTF8.GetBytes((string)reader.Value);
        return Convert.ToBase64String(valueInBytes);
    }

    public override bool CanConvert(Type objectType)
    {
        return true;
    }
}