using SchoolManagementAPI.Models.Embeded.Account;
using SchoolManagementAPI.Models.Embeded.SchoolClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementAPI.Test.Models.Test
{
    public class SchoolMember : Account
    {
        public PersonalInformation PersonalInfo { get; set; }
        public List<string> Classes { get; set; }
        public Dictionary<string, List<ClassSchedule>> ScheduleAggregations { get; set; }
        // string: ID của semester

        public SchoolMember()
        {
            PersonalInfo = new PersonalInformation();
            Classes = new List<string>();
            ScheduleAggregations = new Dictionary<string, List<ClassSchedule>>();
        }

        public static string GetFieldName<T>(Expression<Func<SchoolMember, T>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
            {
                throw new ArgumentException("Invalid expression. Must be a property access expression.", nameof(expression));
            }

            var stack = new Stack<string>();

            while (memberExpression != null)
            {
                stack.Push(memberExpression.Member.Name);
                memberExpression = memberExpression.Expression as MemberExpression;
            }

            return string.Join(".", stack);
        }

    }

    [TestFixture]
    public class SchoolMemberTest
    {
        private SchoolMember _schoolMember;

        [SetUp]
        public void SetUp()
        {
            _schoolMember = new SchoolMember();
        }

        [Test]
        public void Constructor_WhenCalled_InitializesProperties()
        {
            Assert.IsNotNull(_schoolMember.PersonalInfo);
            Assert.IsNotNull(_schoolMember.Classes);
            Assert.IsNotNull(_schoolMember.ScheduleAggregations);
        }

        [Test]
        public void GetFieldName_WhenCalledWithValidExpression_ReturnsCorrectFieldName_PersonalInfo()
        {
            string fieldName = SchoolMember.GetFieldName(x => x.PersonalInfo);
            Assert.AreEqual("PersonalInfo", fieldName);
        }

        [Test]
        public void GetFieldName_WhenCalledWithValidExpression_ReturnsCorrectFieldName_Classes()
        {
            string fieldName = SchoolMember.GetFieldName(x => x.Classes);
            Assert.AreEqual("Classes", fieldName);
        }

        [Test]
        public void GetFieldName_WhenCalledWithValidExpression_ReturnsCorrectFieldName_ScheduleAggregations()
        {
            string fieldName = SchoolMember.GetFieldName(x => x.ScheduleAggregations);
            Assert.AreEqual("ScheduleAggregations", fieldName);
        }

        //[Test]
        //public void GetFieldName_WhenCalledWithInvalidExpression_ThrowsArgumentException()
        //{
        //    Assert.Throws<ArgumentException>(() => SchoolMember.GetFieldName(x => x.PersonalInfo.Name));
        //}
    }
}
