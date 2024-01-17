using SchoolManagementAPI.Models.Embeded.Account;
using SchoolManagementAPI.Models.Embeded.SchoolClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementAPI.Test.Models.Test
{
    public class ExamMileStone
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? Room { get; set; }

        public ExamMileStone()
        {
            Id = string.Empty; Name = string.Empty;
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
    public class ExamMileStoneTest
    {
        private ExamMileStone _examMileStone;

        [SetUp]
        public void Setup()
        {
            _examMileStone = new ExamMileStone();
        }

        [Test]
        public void ExamMileStone_Constructor_InitializesPropertiesCorrectly()
        {
            Assert.AreEqual(string.Empty, _examMileStone.Id);
            Assert.AreEqual(string.Empty, _examMileStone.Name);
        }

        [Test]
        public void Test_ID_Property()
        {
            string testID = "TestID";
            _examMileStone.Id = testID;

            Assert.AreEqual(testID, _examMileStone.Id);
        }

        [Test]
        public void Test_Name_Property()
        {
            string testName = "TestName";
            _examMileStone.Name = testName;

            Assert.AreEqual(testName, _examMileStone.Name);
        }

        [Test]
        public void Test_Room_Property()
        {
            string testRoom = "TestRoom";
            _examMileStone.Room = testRoom;

            Assert.AreEqual(testRoom, _examMileStone.Room);
        }

        [Test]
        public void Test_StartTime_Property()
        {
            DateTime testStartTime = DateTime.Now;
            _examMileStone.StartTime = testStartTime;

            Assert.AreEqual(testStartTime, _examMileStone.StartTime);
        }

        [Test]
        public void Test_Duration_Property()
        {
            TimeSpan testDuration = TimeSpan.FromHours(1);
            _examMileStone.Duration = testDuration;

            Assert.AreEqual(testDuration, _examMileStone.Duration);
        }
    }
}
