using SchoolManagementAPI.Services.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SchoolManagementAPI.Test.Services.Test
{
    [TestFixture]
    public class DateTimeConverterTest
    {
        [Test]
        public void Read_ShouldReturnCorrectDateTime()
        {
            // Arrange
            var converter = new DateTimeConverter("yyyy-MM-dd");
            var jsonString = "\"2024-01-01\"";
            var jsonBytes = Encoding.UTF8.GetBytes(jsonString);
            var reader = new Utf8JsonReader(jsonBytes);

            // Act
            reader.Read();
            var result = converter.Read(ref reader, typeof(DateTime), null);

            // Assert
            Assert.AreEqual(new DateTime(2024, 1, 1), result);
        }

        [Test]
        public void Read_ShouldReturnCorrectDateTime_WithDifferentFormat()
        {
            // Arrange
            var converter = new DateTimeConverter("dd/MM/yyyy");
            var jsonString = "\"01/01/2024\"";
            var jsonBytes = Encoding.UTF8.GetBytes(jsonString);
            var reader = new Utf8JsonReader(jsonBytes);

            // Act
            reader.Read();
            var result = converter.Read(ref reader, typeof(DateTime), null);

            // Assert
            Assert.AreEqual(new DateTime(2024, 1, 1), result);
        }

        [Test]
        public void Read_ShouldThrowException_WhenInvalidDate()
        {
            // Arrange
            var converter = new DateTimeConverter("yyyy-MM-dd");
            var jsonString = "\"2024-13-01\""; // Invalid date
            var jsonBytes = Encoding.UTF8.GetBytes(jsonString);
            var reader = new Utf8JsonReader(jsonBytes);
            reader.Read();

            // Act & Assert
            JsonException exception = null;
            try
            {
                var result = converter.Read(ref reader, typeof(DateTime), null);
            }
            catch (JsonException ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);
        }

        [Test]
        public void Read_ShouldThrowException_WhenInvalidFormat()
        {
            // Arrange
            var converter = new DateTimeConverter("yyyy-MM-dd");
            var jsonString = "\"01/01/2024\""; // Date doesn't match the format
            var jsonBytes = Encoding.UTF8.GetBytes(jsonString);
            var reader = new Utf8JsonReader(jsonBytes);
            reader.Read();

            // Act & Assert
            JsonException exception = null;
            try
            {
                var result = converter.Read(ref reader, typeof(DateTime), null);
            }
            catch (JsonException ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);
        }
    }
}
