using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementAPI.Test.Models.Test
{
    public class PersonalInformation
    {
        public string? Name { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }

        [BsonIgnoreIfNull]
        // CLC hay đại trà
        public string? Program { get; set; } 
        
        [BsonIgnoreIfNull]
        public string? Major { get; set; }
        
        public string? FacultyId { get; set; }
        public string? IDCard { get; set; }
        
        [BsonIgnoreIfNull]
        public string? JoinYear { get; set; }
        
        [BsonIgnoreIfNull]
        public DateTime? JoinDate { get; set; }
        
        [BsonIgnoreIfNull]
        public string? AcademicRank { get; set; }
        
        [BsonIgnoreIfNull]
        public string? AcademicDegree { get; set; }

    }

    [TestFixture]
    public class PersonalInformationTest
    {
        private PersonalInformation _personalInformation;

        [SetUp]
        public void Setup()
        {
            _personalInformation = new PersonalInformation();
        }

        [Test]
        public void Name_SetValue_GetSameValue()
        {
            string testName = "Test Name";
            _personalInformation.Name = testName;

            Assert.AreEqual(testName, _personalInformation.Name);
        }

        [Test]
        public void AvatarURL_SetValue_GetSameValue()
        {
            string testURL = "https://image.png";
            _personalInformation.AvatarUrl = testURL;

            Assert.AreEqual(testURL, _personalInformation.AvatarUrl);
        }

        [Test]
        public void DateOfBirth_SetValue_GetSameValue()
        {
            DateTime testDateOfBirth = new DateTime(2000, 1, 1);
            _personalInformation.DateOfBirth = testDateOfBirth;

            Assert.AreEqual(testDateOfBirth, _personalInformation.DateOfBirth);
        }

        [Test]
        public void Gender_SetValue_GetSameValue()
        {
            string testGender = "Male";
            _personalInformation.Gender = testGender;

            Assert.AreEqual(testGender, _personalInformation.Gender);
        }

        [Test]
        public void Phone_SetValue_GetSameValue()
        {
            string testPhone = "0123456789";
            _personalInformation.Phone = testPhone;

            Assert.AreEqual(testPhone, _personalInformation.Phone);
        }

        [Test]
        public void Program_SetValue_GetSameValue()
        {
            string testProgram = "CLC";
            _personalInformation.Program = testProgram;

            Assert.AreEqual(testProgram, _personalInformation.Program);
        }

        [Test]
        public void Major_SetValue_GetSameValue()
        {
            string testMajor = "CONG NGHE PHAN MEM";
            _personalInformation.Major = testMajor;

            Assert.AreEqual(testMajor, _personalInformation.Major);
        }

        [Test]
        public void FacultyId_SetValue_GetSameValue()
        {
            string testFacultyId = "SE";
            _personalInformation.FacultyId = testFacultyId;

            Assert.AreEqual(testFacultyId, _personalInformation.FacultyId);
        }

        [Test]
        public void IDCard_SetValue_GetSameValue()
        {
            string testIDCard = "21522329";
            _personalInformation.IDCard = testIDCard;

            Assert.AreEqual(testIDCard, _personalInformation.IDCard);
        }

        [Test]
        public void JoinYear_SetValue_GetSameValue()
        {
            string testJoinYear = "2021";
            _personalInformation.JoinYear = testJoinYear;

            Assert.AreEqual(testJoinYear, _personalInformation.JoinYear);
        }

        [Test]
        public void JoinDate_SetValue_GetSameValue()
        {
            DateTime testJoinDate = new DateTime(2021, 1, 1);
            _personalInformation.JoinDate = testJoinDate;

            Assert.AreEqual(testJoinDate, _personalInformation.JoinDate);
        }

        [Test]
        public void AcademicRank_SetValue_GetSameValue()
        {
            string testAcademicRank = "CU NHAN";
            _personalInformation.AcademicRank = testAcademicRank;

            Assert.AreEqual(testAcademicRank, _personalInformation.AcademicRank);
        }

        [Test]
        public void AcademicDegree_SetValue_GetSameValue()
        {
            string testAcademicDegree = "CU NHAN";
            _personalInformation.AcademicDegree = testAcademicDegree;

            Assert.AreEqual(testAcademicDegree, _personalInformation.AcademicDegree);
        }
    }
}
