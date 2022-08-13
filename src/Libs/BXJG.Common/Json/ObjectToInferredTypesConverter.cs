using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace System.Text.Json
{
    //参考
    //https://docs.microsoft.com/zh-cn/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-6-0#deserialize-inferred-types-to-object-properties 
    //微软推荐不要反序列化为object，而是通过GetProperty..ToXXX方式获取数据
    //我们遵循这个规则，建议不要注册ObjectToInferredTypesConverter为全局转换器，而是在使用时new Option对象

    public class ObjectToInferredTypesConverter : JsonConverter<object>
    {
        //public static readonly ObjectToInferredTypesConverter Instance = new ObjectToInferredTypesConverter();
        public override object Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) => reader.TokenType switch
            {
                JsonTokenType.True => true,
                JsonTokenType.False => false,
                JsonTokenType.Number when reader.TryGetInt64(out long l) => l,
                JsonTokenType.Number => reader.GetDouble(),
                JsonTokenType.String when reader.TryGetDateTime(out DateTime datetime) => datetime,
                JsonTokenType.String => reader.GetString()!,
                _ => JsonDocument.ParseValue(ref reader).RootElement.Clone()
            };

        public override void Write(
            Utf8JsonWriter writer,
            object objectToWrite,
            JsonSerializerOptions options) =>
            JsonSerializer.Serialize(writer, objectToWrite, objectToWrite.GetType(), options);

    }
}
