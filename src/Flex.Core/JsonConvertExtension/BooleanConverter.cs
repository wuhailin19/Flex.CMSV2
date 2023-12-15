using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Flex.Core.JsonConvertExtension
{
    public class BooleanConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.True || reader.TokenType == JsonTokenType.False)
            {
                return reader.GetBoolean();
            }

            // 处理其他情况，例如处理字符串 "true" 或 "false"
            if (reader.TokenType == JsonTokenType.String && bool.TryParse(reader.GetString(), out var boolValue))
            {
                return boolValue;
            }

            throw new JsonException($"Unable to convert value to {typeToConvert}.");
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteBooleanValue(value);
        }
    }

}
