using CloudinaryDotNet;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq.Expressions;

namespace SchoolManagementAPI.Models.Entities
{
    [BsonIgnoreExtraElements]
    public class Admin : Models.Abstracts.Account
    {
        public Admin() : base()
        {
            this.ID = Guid.NewGuid().ToString();
            this.Role = "admin";
        }
        public static string GetFieldName<T>(Expression<Func<Admin, T>> expression)
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
