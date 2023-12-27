using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SchoolManagementAPI.Services.Converters
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private readonly string _dateFormat;

        public DateTimeConverter(string dateFormat)
        {
            _dateFormat = dateFormat;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Implement custom deserialization logic if needed
            return DateTime.ParseExact(reader.GetString(), _dateFormat, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Implement custom serialization logic if needed
            writer.WriteStringValue(value.ToString(_dateFormat));
        }
    }
}
