using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementAPI.Test.Models.Test
{
    public class StudentLog
    {
        public string ID { get; set; }
        public List<int> Scores { get; set; }

        public StudentLog()
        {
            Scores = new List<int>();
        }
    }

    public class RollCall
    {
        public DateTime Time { get; set; }
        public bool IsPresent { get; set; }
    }

    [TestFixture]
    public class StudentLogTest
    {
        [Test]
        public void TestStudentLogCreation()
        {
            // Arrange & Act
            var studentLog = new StudentLog
            {
                ID = "21522329",
                Scores = new List<int> { 7, 8, 9, 10 }
            };

            // Assert
            Assert.That(studentLog.ID, Is.EqualTo("21522329"));
            CollectionAssert.AreEqual(new List<int> { 7, 8, 9, 10 }, studentLog.Scores);
        }

        [Test]
        public void TestRollCallCreation()
        {
            // Arrange & Act
            var rollCall = new RollCall
            {
                Time = DateTime.Now,
                IsPresent = true
            };

            // Assert
            Assert.That(rollCall.IsPresent, Is.EqualTo(true));
            Assert.That(rollCall.Time.ToString(), Is.EqualTo(DateTime.Now.ToString()));
        }

        [Test]
        public void Test_ID_Property()
        {
            // Arrange & Act
            var studentLog = new StudentLog();
            string testID = "TestID";
            studentLog.ID = testID;

            // Assert
            Assert.That(studentLog.ID, Is.EqualTo(testID));
        }

        [Test]
        public void Test_Scores_Property()
        {
            // Arrange & Act
            var studentLog = new StudentLog();
            List<int> testScores = new List<int> { 7, 8, 9, 10 };
            studentLog.Scores = testScores;

            // Assert
            Assert.That(studentLog.Scores, Is.EqualTo(testScores));
        }

        [Test]
        public void Test_Time_Property()
        {
            // Arrange & Act
            var rollCall = new RollCall();
            DateTime testDate = DateTime.Now;
            rollCall.Time = testDate;

            // Assert
            Assert.That(rollCall.Time, Is.EqualTo(testDate));
        }

        [Test]
        public void Test_IsPresent_True_Property()
        {
            // Arrange & Act
            var rollCall = new RollCall();
            bool isTestPresent = true;
            rollCall.IsPresent = isTestPresent;

            // Assert
            Assert.That(rollCall.IsPresent, Is.EqualTo(isTestPresent));
        }

        [Test]
        public void Test_IsPresent_False_Property()
        {
            // Arrange & Act
            var rollCall = new RollCall();
            bool isTestPresent = false;
            rollCall.IsPresent = isTestPresent;

            // Assert
            Assert.That(rollCall.IsPresent, Is.EqualTo(isTestPresent));
        }
    }
}
