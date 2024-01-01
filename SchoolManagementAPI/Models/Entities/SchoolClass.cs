using MongoDB.Bson.Serialization.Attributes;
using SchoolManagementAPI.Models.Embeded.ReuseTypes;
using SchoolManagementAPI.Models.Embeded.SchoolClass;
using System.Linq.Expressions;

namespace SchoolManagementAPI.Models.Entities
{
    [BsonIgnoreExtraElements]
    public class SchoolClass
    {
        [BsonId]
        public string ID { get; set; }
        public string? Name { get; set; }
        public string? RoomName { get; set; }
        public string? Program { get; set; }
        public string? ClassType { get; set; }
        public string? SemesterId { get; set; }
        public DataLink? Subject { get; set; }
        public DataLink? Lecturer { get; set; }
        public ClassSchedule? Schedule { get; set; }
        public List<StudentLog> StudentLogs { get; set; }
        public List<ExamMileStone> Exams { get; set; }
        public List<Section> Sections { get; set; }

        public SchoolClass()
        {
            ID = String.Empty;
            StudentLogs = new List<StudentLog>();
            Exams = new List<ExamMileStone>();
            Sections = new List<Section>();
        }
        public static string GetFieldName<T>(Expression<Func<SchoolClass, T>> expression)
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
