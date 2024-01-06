using MongoDB.Bson.Serialization.Attributes;
using SchoolManagementAPI.Models.Abstracts;
using SchoolManagementAPI.Models.Embeded.Account;
using System.Linq.Expressions;

namespace SchoolManagementAPI.Models.Entities
{
    [BsonIgnoreExtraElements]
    public class Student : SchoolMember
    {
        public Dictionary<string,List<CreditLog>> creditLogs { get; set; }

        public Student(): base()
        {
            Role = "student";
            creditLogs = new Dictionary<string, List<CreditLog>>();
        }
        public static string GetFieldName<T>(Expression<Func<Student, T>> expression)
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
