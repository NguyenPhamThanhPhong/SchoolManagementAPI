using SchoolManagementAPI.Models.Abstracts;
using SchoolManagementAPI.Models.Embeded.Account;
using System.Linq.Expressions;

namespace SchoolManagementAPI.Models.Entities
{
    public class Student : SchoolMember
    {
        public CreditLog CreditInfo { get; set; }
        public string? Programs { get; set; }

        public Student(): base()
        {
            Role = "student";
            this.CreditInfo = new CreditLog();

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
