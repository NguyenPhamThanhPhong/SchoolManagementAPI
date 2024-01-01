using MongoDB.Bson.Serialization.Attributes;
using SchoolManagementAPI.Models.Embeded.ReuseTypes;
using SchoolManagementAPI.Models.Embeded.Subject;
using System.Linq.Expressions;

namespace SchoolManagementAPI.Models.Entities
{
    [BsonIgnoreExtraElements]
    public class Subject
    {
        [BsonId]
        public string ID { get; set; } = "";
        public string? Name { get; set; }
        public List<string>? PrequisiteIds { get; set; }
        public List<string>? PreviousSubjectIds { get; set; }
        public string? FacultyId { get; set; }
        public List<string>? ClassIds { get; set; }
        public Subject()
        {
            ClassIds = new List<string>();
            PrequisiteIds = new List<string>();
            PreviousSubjectIds = new List<string>();
        }
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
