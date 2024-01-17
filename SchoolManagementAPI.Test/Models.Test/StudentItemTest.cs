using SchoolManagementAPI.Models.Embeded.SchoolClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementAPI.Test.Models.Test
{
    public class StudentItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Progress { get; set; }
        public int Midterm { get; set; }
        public int Practice { get; set; }
        public int Final { get; set; }

        public StudentItem()
        {
            Id = string.Empty;
            Name = string.Empty;
            Progress = -1;
            Midterm = -1;
            Practice = -1;
            Final = -1;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            StudentItem other = (StudentItem)obj;

            // Compare the Id property for equality
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            // Use the Id property hash code for hashing
            return Id.GetHashCode();
        }
    }

    [TestFixture]
    public class StudentItemTest
    {
        [Test]
        public void Constructor_Sets_Default_Values()
        {
            var studentItem = new StudentItem();

            Assert.That(studentItem.Id, Is.EqualTo(string.Empty));
            Assert.That(studentItem.Name, Is.EqualTo(string.Empty));
            Assert.That(studentItem.Progress, Is.EqualTo(-1));
            Assert.That(studentItem.Midterm, Is.EqualTo(-1));
            Assert.That(studentItem.Practice, Is.EqualTo(-1));
            Assert.That(studentItem.Final, Is.EqualTo(-1));
        }

        [Test]
        public void Equals_Returns_False_For_Null_Object()
        {
            var studentItem = new StudentItem();

            Assert.IsFalse(studentItem.Equals(null));
        }

        [Test]
        public void Equals_Returns_False_For_Different_Type()
        {
            var studentItem = new StudentItem();

            Assert.IsFalse(studentItem.Equals("Not a StudentItem"));
        }

        [Test]
        public void Equals_Returns_True_For_Same_Object()
        {
            var studentItem1 = new StudentItem { Id = "1", Name = "Manh" };
            var studentItem2 = studentItem1;

            Assert.IsTrue(studentItem1.Equals(studentItem2));
        }

        [Test]
        public void Equals_Returns_True_For_Same_Values()
        {
            var studentItem1 = new StudentItem { Id = "1", Name = "Manh" };
            var studentItem2 = new StudentItem { Id = "1", Name = "Manh" };

            Assert.IsTrue(studentItem1.Equals(studentItem2));
        }

        [Test]
        public void Equals_Returns_False_For_Different_Values()
        {
            var studentItem1 = new StudentItem { Id = "1", Name = "Manh" };
            var studentItem2 = new StudentItem { Id = "2", Name = "Hnam" };

            Assert.IsFalse(studentItem1.Equals(studentItem2));
        }

        [Test]
        public void GetHashCode_Returns_Same_Hash_For_Same_Values()
        {
            var studentItem1 = new StudentItem { Id = "1", Name = "Manh" };
            var studentItem2 = new StudentItem { Id = "1", Name = "Manh" };

            Assert.That(studentItem2.GetHashCode(), Is.EqualTo(studentItem1.GetHashCode()));
        }

        [Test]
        public void GetHashCode_Returns_Different_Hash_For_Different_Values()
        {
            var studentItem1 = new StudentItem { Id = "1", Name = "Manh" };
            var studentItem2 = new StudentItem { Id = "2", Name = "Hnam" };

            Assert.That(studentItem2.GetHashCode(), Is.Not.EqualTo(studentItem1.GetHashCode()));
        }

        [Test]
        public void Test_Id_Property()
        {
            // Arrange & Act
            var studentItem = new StudentItem();
            string testId = "TestId";
            studentItem.Id = testId;

            // Assert
            Assert.That(studentItem.Id, Is.EqualTo(testId));
        }

        [Test]
        public void Test_Name_Property()
        {
            // Arrange & Act
            var studentItem = new StudentItem();
            string testName = "TestName";
            studentItem.Name = testName;

            // Assert
            Assert.That(studentItem.Name, Is.EqualTo(testName));
        }

        [Test]
        public void Test_Progress_Property()
        {
            // Arrange & Act
            var studentItem = new StudentItem();
            int testProgress = 7;
            studentItem.Progress = testProgress;

            // Assert
            Assert.That(studentItem.Progress, Is.EqualTo(testProgress));
        }

        [Test]
        public void Test_Midterm_Property()
        {
            // Arrange & Act
            var studentItem = new StudentItem();
            int testMidterm = 8;
            studentItem.Midterm = testMidterm;

            // Assert
            Assert.That(studentItem.Midterm, Is.EqualTo(testMidterm));
        }

        [Test]
        public void Test_Practice_Property()
        {
            // Arrange & Act
            var studentItem = new StudentItem();
            int testPractice = 9;
            studentItem.Practice = testPractice;

            // Assert
            Assert.That(studentItem.Practice, Is.EqualTo(testPractice));
        }

        [Test]
        public void Test_Final_Property()
        {
            // Arrange & Act
            var studentItem = new StudentItem();
            int testFinal = 10;
            studentItem.Final = testFinal;

            // Assert
            Assert.That(studentItem.Final, Is.EqualTo(testFinal));
        }
    }
}
