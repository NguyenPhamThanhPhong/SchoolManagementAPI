using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementAPI.Test.Models.Test
{
    public class DataLink
    {
        public string? ID { get; set; }
        public string? Name { get; set; }
    }

    [TestFixture]
    public class DataLinkTest
    {
        [Test]
        public void DataLink_SetProperties_ShouldReturnCorrectValues()
        {
            // Arrange
            var dataLink = new DataLink();

            // Act
            dataLink.ID = "TestID";
            dataLink.Name = "TestName";

            // Assert
            Assert.AreEqual("TestID", dataLink.ID);
            Assert.AreEqual("TestName", dataLink.Name);
        }

        [Test]
        public void DataLink_SetNullValues_ShouldReturnNull()
        {
            // Arrange
            var dataLink = new DataLink();

            // Act
            dataLink.ID = null;
            dataLink.Name = null;

            // Assert
            Assert.IsNull(dataLink.ID);
            Assert.IsNull(dataLink.Name);
        }

        [Test]
        public void DataLink_SetEmptyValues_ShouldReturnEmpty()
        {
            // Arrange
            var dataLink = new DataLink();

            // Act
            dataLink.ID = string.Empty;
            dataLink.Name = string.Empty;

            // Assert
            Assert.AreEqual(string.Empty, dataLink.ID);
            Assert.AreEqual(string.Empty, dataLink.Name);
        }
    }
}
