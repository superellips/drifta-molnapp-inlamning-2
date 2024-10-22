using TipsRundan.Domain.Entities;
using Xunit;

namespace TipsRundan.Domain.Tests
{
    public class UserTests
    {
        [Fact]
        public void User_Should_Set_Properties_Correctly()
        {
            // Arrange
            var user = User.Create("username", "user@example.com", "passwordHash");

            // Act & Assert
            Assert.Equal("username", user.Username);
            Assert.Equal("user@example.com", user.Email);
            Assert.Equal("passwordHash", user.PasswordHash);
        }
    }
}
