using System.Text.Json;
using System.Text.Json.Serialization;

namespace Flex.Core.JsonConvertExtension
{
    public class IdToStringConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // 从 JSON 中读取 long 值
            if (reader.TryGetInt64(out long result))
            {
                return result;
            }

            // 如果无法成功读取，则尝试解析字符串并转换为 long
            if (long.TryParse(reader.GetString(), out result))
            {
                return result;
            }

            // 如果无法成功解析，则抛出异常
            throw new JsonException($"Unable to convert value '{reader.GetString()}' to {typeof(long)}.");
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            // 将 long 值写入 JSON 中
            writer.WriteNumberValue(value);
        }
    }
}
