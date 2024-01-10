using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SchoolManagementAPI.Test.Services.Test
{
    [TestFixture]
    public class TimeSpanConverterTest
    {
        private TimeSpanConverter _converter;
        private JsonSerializerOptions _options;

        [SetUp]
        public void Setup()
        {
            _converter = new TimeSpanConverter();
            _options = new JsonSerializerOptions();
        }

        [Test]
        public void TestRead()
        {
            var validTimeSpanString = "00:00:30";
            var invalidTimeSpanString = "invalid";
            var validTimeSpan = TimeSpan.FromSeconds(30);

            var validReader = new Utf8JsonReader(new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes($"\"{validTimeSpanString}\"")));
            validReader.Read();
            var result = _converter.Read(ref validReader, typeof(TimeSpan), _options);
            Assert.AreEqual(validTimeSpan, result);

            var invalidReader = new Utf8JsonReader(new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes($"\"{invalidTimeSpanString}\"")));
            invalidReader.Read();
            result = _converter.Read(ref invalidReader, typeof(TimeSpan), _options);
            Assert.AreEqual(TimeSpan.Zero, result);
        }

        [Test]
        public void TestWrite()
        {
            var timeSpan = TimeSpan.FromSeconds(30);
            var stream = new MemoryStream();
            var writer = new Utf8JsonWriter(stream);

            _converter.Write(writer, timeSpan, _options);
            writer.Flush();

            var result = Encoding.UTF8.GetString(stream.ToArray());
            Assert.AreEqual($"\"{timeSpan}\"", result);
        }
    }
}
