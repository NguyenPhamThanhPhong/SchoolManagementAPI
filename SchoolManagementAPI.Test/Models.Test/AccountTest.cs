using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementAPI.Test.Models.Test
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string ID { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }

        public Account()
        {
            this.ID = string.Empty;
        }

        public static string GetFieldName<T>(Expression<Func<Account, T>> expression)
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
    public class AccountTest
    {
        [Test]
        public void GetFieldName_ReturnsCorrectFieldName_ID()
        {
            // Arrange
            Expression<Func<Account, object>> expression = x => x.ID;

            // Act
            var result = Account.GetFieldName(expression);

            // Assert
            Assert.AreEqual("ID", result);
        }

        [Test]
        public void GetFieldName_ReturnsCorrectFieldName_Username()
        {
            // Arrange
            Expression<Func<Account, object>> expression = x => x.Username;

            // Act
            var result = Account.GetFieldName(expression);

            // Assert
            Assert.AreEqual("Username", result);
        }

        [Test]
        public void GetFieldName_ReturnsCorrectFieldName_Password()
        {
            // Arrange
            Expression<Func<Account, object>> expression = x => x.Password;

            // Act
            var result = Account.GetFieldName(expression);

            // Assert
            Assert.AreEqual("Password", result);
        }

        [Test]
        public void GetFieldName_ReturnsCorrectFieldName_Email()
        {
            // Arrange
            Expression<Func<Account, object>> expression = x => x.Email;

            // Act
            var result = Account.GetFieldName(expression);

            // Assert
            Assert.AreEqual("Email", result);
        }

        [Test]
        public void GetFieldName_ReturnsCorrectFieldName_Role()
        {
            // Arrange
            Expression<Func<Account, object>> expression = x => x.Role;

            // Act
            var result = Account.GetFieldName(expression);

            // Assert
            Assert.AreEqual("Role", result);
        }

        [Test]
        public void GetFieldName_ThrowsArgumentExceptionForInvalidExpression()
        {
            // Arrange
            Expression<Func<Account, object>> expression = x => x.ID + "invalid";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Account.GetFieldName(expression));
        }
    }
}
