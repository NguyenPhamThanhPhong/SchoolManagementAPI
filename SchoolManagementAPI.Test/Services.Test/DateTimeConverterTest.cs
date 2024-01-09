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
    }
}
