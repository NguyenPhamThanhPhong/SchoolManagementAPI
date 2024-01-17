using SchoolManagementAPI.Models.Embeded.SchoolClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementAPI.Test.Models.Test
{
    public class Section
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public Dictionary<string, string?>? DocumentUrls { get; set; }

        public Section()
        {
            Title = string.Empty;
            Content = string.Empty;
        }
    }

    [TestFixture]
    public class SectionTest
    {
        [Test]
        public void Constructor_Sets_Default_Values()
        {
            // Arrange & Act
            var section = new Section();

            // Assert
            Assert.That(section.Title, Is.EqualTo(string.Empty));
            Assert.That(section.Content, Is.EqualTo(string.Empty));
            Assert.That(section.DocumentUrls, Is.Null);
        }

        //[Test]
        //public void Parameterized_Constructor_Sets_Values_Correctly()
        //{
        //    // Arrange
        //    string title = "Test Title";
        //    string content = "Test Content";
        //    List<string> documentUrls = new List<string> { "http://test.com/doc1", "http://test.com/doc2" };

        //    // Act
        //    var section = new Section(title, content, documentUrls);

        //    // Assert
        //    Assert.That(section.Title, Is.EqualTo(title));
        //    Assert.That(section.Content, Is.EqualTo(content));
        //    Assert.That(section.DocumentUrls, Is.EqualTo(documentUrls));
        //}

        [Test]
        public void Test_Title_Property()
        {
            // Arrange & Act
            var section = new Section();
            string testTitle = "TestTitle";
            section.Title = testTitle;

            // Assert
            Assert.That(section.Title, Is.EqualTo(testTitle));
        }

        [Test]
        public void Test_Content_Property()
        {
            // Arrange & Act
            var section = new Section();
            string testContent = "TestContent";
            section.Content = testContent;

            // Assert
            Assert.That(section.Content, Is.EqualTo(testContent));
        }
    }
}
