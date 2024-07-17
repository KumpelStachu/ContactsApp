using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace api.Utils
{
    /// <summary>
    /// Json converter for DateOnly objects.
    /// </summary>
    /// <remarks>
    /// This class is used to serialize and deserialize DateOnly objects.
    /// </remarks>
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string DateFormat = "yyyy-MM-dd";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (string.IsNullOrEmpty(value)) return default;
            return DateOnly.ParseExact(value, DateFormat, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(DateFormat));
        }
    }
}