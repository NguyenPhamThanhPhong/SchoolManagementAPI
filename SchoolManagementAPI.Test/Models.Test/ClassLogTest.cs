using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementAPI.Test.Models.Test
{
    public class ClassLog
    {
        public string ID { get; set; }
        public int Current { get; set; }
        public int Max { get; set; }
    }

    [TestFixture]
    public class ClassLogTest
    {
        [Test]
        public void ClassLog_SetGet_ReturnsCorrectValues()
        {
            // Arrange
            var classLog = new ClassLog();

            // Act
            classLog.ID = "TestID";
            classLog.Current = 10;
            classLog.Max = 20;

            // Assert
            Assert.That(classLog.ID, Is.EqualTo("TestID"));
            Assert.That(classLog.Current, Is.EqualTo(10));
            Assert.That(classLog.Max, Is.EqualTo(20));
        }

        [Test]
        public void ClassLog_DefaultValues_AreCorrect()
        {
            // Arrange
            var classLog = new ClassLog();

            // Assert
            Assert.IsNull(classLog.ID);
            Assert.That(classLog.Current, Is.EqualTo(0));
            Assert.That(classLog.Max, Is.EqualTo(0));
        }

        //[Test]
        //public void ClassLog_SetNegativeCurrent_ThrowsArgumentException()
        //{
        //    // Arrange
        //    var classLog = new ClassLog();

        //    // Act & Assert
        //    Assert.Throws<ArgumentException>(() => classLog.Current = -1);
        //}

        //[Test]
        //public void ClassLog_SetNegativeMax_ThrowsArgumentException()
        //{
        //    // Arrange
        //    var classLog = new ClassLog();

        //    // Act & Assert
        //    Assert.Throws<ArgumentException>(() => classLog.Max = -1);
        //}

        [Test]
        public void Test_ID_Property()
        {
            // Arrange & Act
            var classLog = new ClassLog();
            string testID = "TestID";
            classLog.ID = testID;

            // Assert
            Assert.That(classLog.ID, Is.EqualTo(testID));
        }

        [Test]
        public void Test_Current_Property()
        {
            // Arrange & Act
            var classLog = new ClassLog();
            int testCurrent = 10;
            classLog.Current = testCurrent;

            // Assert
            Assert.That(classLog.Current, Is.EqualTo(testCurrent));
        }

        [Test]
        public void Test_Max_Property()
        {
            // Arrange & Act
            var classLog = new ClassLog();
            int testMax = 20;
            classLog.Max = testMax;

            // Assert
            Assert.That(classLog.Max, Is.EqualTo(testMax));
        }
    }
}
