﻿using SchoolManagementAPI.Models.Abstracts;
using SchoolManagementAPI.Services.Authentication;
using SchoolManagementAPI.Services.Configs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementAPI.Test.Services.Test
{
    [TestFixture]
    public class TokenGeneratorTest
    {
        private TokenGenerator _tokenGenerator;
        private readonly Random random = new Random();
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public string GenerateRandomString(int length)
        {
            char[] stringChars = new char[length];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }

        [SetUp]
        public void Setup()
        {
            var tokenConfig = new TokenConfig
            {
                AccessTokenSecret = GenerateRandomString(32),
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                AccessTokenExpirationMinutes = 60
            };

            _tokenGenerator = new TokenGenerator(tokenConfig);
        }

        [Test]
        public void GenerateAccessToken_ShouldReturnToken()
        {
            // Arrange
            var account = new Account
            {
                ID = "TestId",
                Email = "test@example.com",
                Username = "TestUser",
                Role = "TestRole"
            };

            // Act
            var result = _tokenGenerator.GenerateAccessToken(account);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GenerateAccessToken_ShouldContainCorrectClaims()
        {
            // Arrange
            var account = new Account
            {
                ID = "TestId",
                Email = "test@example.com",
                Username = "TestUser",
                Role = "TestRole"
            };

            // Act
            var token = _tokenGenerator.GenerateAccessToken(account);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Assert
            Assert.IsTrue(jwtToken.Claims.Any(c => c.Value == account.ID));
            Assert.IsTrue(jwtToken.Claims.Any(c => c.Value == account.Role));
        }

        [Test]
        public void GenerateAccessToken_ShouldHaveCorrectIssuer()
        {
            // Arrange
            var account = new Account
            {
                ID = "TestId",
                Email = "test@example.com",
                Username = "TestUser",
                Role = "TestRole"
            };

            // Act
            var token = _tokenGenerator.GenerateAccessToken(account);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Assert
            Assert.AreEqual("TestIssuer", jwtToken.Issuer);
        }

        [Test]
        public void GenerateAccessToken_ShouldHaveCorrectAudience()
        {
            // Arrange
            var account = new Account
            {
                ID = "TestId",
                Email = "test@example.com",
                Username = "TestUser",
                Role = "TestRole"
            };

            // Act
            var token = _tokenGenerator.GenerateAccessToken(account);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Assert
            Assert.IsTrue(jwtToken.Audiences.Contains("TestAudience"));
        }
    }
}
