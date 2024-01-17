using MongoDB.Bson.Serialization.Attributes;
using SchoolManagementAPI.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementAPI.Test.Models.Test
{
    public class ClassSchedule
    {
        [BsonIgnoreIfNull]
        public string? ID { get; set; }

        [BsonIgnoreIfNull]
        public string? Name { get; set; }

        public DateOfWeek Dateofweek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        [BsonIgnoreIfDefault]
        public DateTime BeginTime { get; set; }

        [BsonIgnoreIfDefault]
        public DateTime FinalTime { get; set; }
    }

    [TestFixture]
    public class ClassScheduleTest
    {
        private ClassSchedule _classSchedule;

        [SetUp]
        public void Setup()
        {
            _classSchedule = new ClassSchedule();
        }

        [Test]
        public void Test_ID_Property()
        {
            string testID = "TestID";
            _classSchedule.ID = testID;

            Assert.AreEqual(testID, _classSchedule.ID);
        }

        [Test]
        public void Test_Name_Property()
        {
            string testName = "TestName";
            _classSchedule.Name = testName;

            Assert.AreEqual(testName, _classSchedule.Name);
        }

        [Test]
        public void Test_BeginTime_Property()
        {
            DateTime testBeginTime = DateTime.Now;
            _classSchedule.BeginTime = testBeginTime;

            Assert.AreEqual(testBeginTime, _classSchedule.BeginTime);
        }

        [Test]
        public void Test_FinalTime_Property()
        {
            DateTime testFinalTime = DateTime.Now;
            _classSchedule.FinalTime = testFinalTime;

            Assert.AreEqual(testFinalTime, _classSchedule.FinalTime);
        }

        [Test]
        public void Test_Dateofweek_Property()
        {
            DateOfWeek testDateofweek = (DateOfWeek)DayOfWeek.Monday;
            _classSchedule.Dateofweek = testDateofweek;

            Assert.AreEqual(testDateofweek, _classSchedule.Dateofweek);
        }

        [Test]
        public void Test_StartTime_Property()
        {
            TimeSpan testStartTime = TimeSpan.FromHours(1);
            _classSchedule.StartTime = testStartTime;

            Assert.AreEqual(testStartTime, _classSchedule.StartTime);
        }

        [Test]
        public void Test_EndTime_Property()
        {
            TimeSpan testEndTime = TimeSpan.FromHours(1);
            _classSchedule.EndTime = testEndTime;

            Assert.AreEqual(testEndTime, _classSchedule.EndTime);
        }
    }
}
