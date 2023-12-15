using Newtonsoft.Json;

namespace Flex.Core.JsonConvertExtension
{
    public class IdToStringConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            if (reader.TokenType == JsonToken.String)
            {
                // 对于字符串类型，直接进行反序列化
                return reader.Value.ToString();
            }

            throw new JsonSerializationException($"Unexpected token type: {reader.TokenType}");
        }

        public override bool CanConvert(Type objectType)
        {
            // 检查是否为整数类型
            return objectType == typeof(int) || objectType == typeof(long) || objectType == typeof(short);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // 将值转换为字符串并序列化到 JSON 中
            serializer.Serialize(writer, value.ToString());
        }
    }

}
