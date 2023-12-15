using Newtonsoft.Json;

namespace Flex.Core.JsonConvertExtension
{
    public class StringToLongConverter : JsonConverter<long>
    {
        public override long ReadJson(JsonReader reader, Type objectType, long existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String && long.TryParse((string)reader.Value, out var result))
            {
                return result;
            }

            throw new JsonSerializationException($"Unable to parse '{reader.Value}' as {objectType}");
        }

        public override void WriteJson(JsonWriter writer, long value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }

}
