using MongoDB.Bson.Serialization.Attributes;
using SchoolManagementAPI.Models.Abstracts;
using SchoolManagementAPI.Models.Embeded.Account;
using SchoolManagementAPI.Models.Embeded.ReuseTypes;
using System.Linq.Expressions;

#pragma warning disable CS8618

namespace SchoolManagementAPI.Models.Entities
{
    [BsonIgnoreExtraElements]

    public class Lecturer : SchoolMember
    {

        public Lecturer() : base()
        {
            Role = "lecturer";
        }
        public static string GetFieldName<T>(Expression<Func<Lecturer, T>> expression)
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
