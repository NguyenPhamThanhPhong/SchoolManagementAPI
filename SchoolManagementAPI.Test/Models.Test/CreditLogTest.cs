using SchoolManagementAPI.Models.Embeded.SchoolClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementAPI.Test.Models.Test
{
    public class CreditLog
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? SemesterId { get; set; }
        public int Progress { get; set; }
        public int Midterm { get; set; }
        public int Practice { get; set; }
        public int Final { get; set; }

        public CreditLog()
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

        public int CalculateTotalScore()
        {
            return Progress + Midterm + Practice + Final; 
        }

        public double CalculateAverageScore()
        {
            return CalculateTotalScore() * 1.0 / 4;
        }
    }

    [TestFixture]
    public class CreditLogTest
    {
        private CreditLog _creditLog;

        [SetUp]
        public void Setup()
        {
            _creditLog = new CreditLog();
        }

        [Test]
        public void CreditLog_Constructor_InitializesPropertiesCorrectly()
        {
            Assert.AreEqual(string.Empty, _creditLog.Id);
            Assert.AreEqual(string.Empty, _creditLog.Name);
            Assert.AreEqual(null, _creditLog.SemesterId);
            Assert.AreEqual(-1, _creditLog.Progress);
            Assert.AreEqual(-1, _creditLog.Midterm);
            Assert.AreEqual(-1, _creditLog.Practice);
            Assert.AreEqual(-1, _creditLog.Final);
        }

        [Test]
        public void CreditLog_PropertiesCanBeSetCorrectly()
        {
            _creditLog.Id = "testId";
            _creditLog.Name = "testName";
            _creditLog.SemesterId = "testSemesterId";
            _creditLog.Progress = 7;
            _creditLog.Midterm = 8;
            _creditLog.Practice = 9;
            _creditLog.Final = 10;

            Assert.AreEqual("testId", _creditLog.Id);
            Assert.AreEqual("testName", _creditLog.Name);
            Assert.AreEqual("testSemesterId", _creditLog.SemesterId);
            Assert.AreEqual(7, _creditLog.Progress);
            Assert.AreEqual(8, _creditLog.Midterm);
            Assert.AreEqual(9, _creditLog.Practice);
            Assert.AreEqual(10, _creditLog.Final);
        }

        [Test]
        public void CreditLog_CalculateTotalScore_ReturnsCorrectResult()
        {
            _creditLog.Progress = 7;
            _creditLog.Midterm = 8;
            _creditLog.Practice = 9;
            _creditLog.Final = 10;

            var totalScore = _creditLog.CalculateTotalScore();

            Assert.AreEqual(34, totalScore);
        }

        [Test]
        public void CreditLog_CalculateAverageScore_ReturnsCorrectResult()
        {
            _creditLog.Progress = 7;
            _creditLog.Midterm = 8;
            _creditLog.Practice = 9;
            _creditLog.Final = 10;

            var averageScore = _creditLog.CalculateAverageScore();

            Assert.AreEqual(8.5, averageScore);
        }
    }
}
