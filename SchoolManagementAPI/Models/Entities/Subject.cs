using MongoDB.Bson.Serialization.Attributes;
using SchoolManagementAPI.Models.Embeded.ReuseTypes;
using SchoolManagementAPI.Models.Embeded.Subject;
using System.Linq.Expressions;

namespace SchoolManagementAPI.Models.Entities
{
    public class Subject
    {
        [BsonId]
        public string ID { get; set; } = "";
        public string? Name { get; set; }
        public string? Prequisite { get; set; }
        public string? PreviousSubject { get; set; }
        public ClassLog? Classes { get; set; }
        public static string GetFieldName<T>(Expression<Func<Subject, T>> expression)
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
