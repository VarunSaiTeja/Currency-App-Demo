using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CurrencyApp.Data.Entities;
using CurrencyApp.Infra.Options;
using CurrencyApp.Infra.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace CurrencyAPI.Tests.Services
{
    public class TokenServiceTests
    {
        private TokenService CreateService(JwtOptions options)
        {
            var mockOptions = new Mock<IOptionsSnapshot<JwtOptions>>();
            mockOptions.Setup(o => o.Value).Returns(options);
            return new TokenService(mockOptions.Object);
        }

        [Fact]
        public void GenerateAccessToken_ShouldIncludeUserIdAndClaims()
        {
            // Arrange
            var options = new JwtOptions
            {
            Issuer = "TestIssuer",
            Secret = "SAMPLEw8Hay9CrKPe2CNZppxCQvGiPtwP1tv10Zbe2g6HY91YD1LgqpDTaKq63R1Jn57L7wUWeB6fqndmj879KpLCbrT1QMCGUiZTux9Erz6qYcX1vmFKULbdH0TteSa",
            AccessExpiryInMins = 60,
            RefreshExpiryInDays = 7
            };
            var user = new User
            {
            Id = 42,
            Name = "Test User",
            Roles = new List<UserRole> { UserRole.Admin, UserRole.Customer }
            };
            var service = CreateService(options);

            // Act
            var token = service.GenerateAccessToken(user);
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            // Assert
            Assert.Contains(jwt.Claims, c => c.Type == "userId" && c.Value == "42");
            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Name && c.Value == "42");
            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Role && c.Value == UserRole.Admin.ToString());
            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Role && c.Value == UserRole.Customer.ToString());
        }
    }
}
