using System;
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
            // Handle null values
            if (reader.TokenType == JsonTokenType.Null)
                return DateTime.MinValue; // or throw an exception if null is not allowed

            // Implement custom deserialization logic
            if (DateTime.TryParseExact(reader.GetString(), _dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                return result;

            Console.WriteLine(reader.GetString());

            // Handle parsing errors
            throw new JsonException($"Failed to parse date. Expected format: {_dateFormat}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Implement custom serialization logic
            writer.WriteStringValue(value.ToString(_dateFormat));
        }
    }
}
