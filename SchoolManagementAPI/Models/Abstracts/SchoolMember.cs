using SchoolManagementAPI.Models.Embeded.Account;
using SchoolManagementAPI.Models.Embeded.ReuseTypes;
using SchoolManagementAPI.Models.Embeded.SchoolClass;
using SchoolManagementAPI.Models.Entities;
using System.Linq.Expressions;

namespace SchoolManagementAPI.Models.Abstracts
{
    public class SchoolMember : Account
    {
        public PersonalInformation PersonalInfo { get; set; }
        public List<string> Classes { get; set; }
        public Dictionary<string,List<ClassSchedule>> ScheduleAggregations { get; set; } 
        // string: ID của semester

        public SchoolMember() {
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
}
