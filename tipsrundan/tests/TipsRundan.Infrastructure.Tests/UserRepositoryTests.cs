using NUnit.Framework;
using Moq;
using TipsRundan.Domain.Entities;
using TipsRundan.Domain.Interfaces;
using TipsRundan.Infrastructure.Repositories;

namespace TipsRundan.Infrastructure.Tests
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private Mock<IAccountRepository> _userRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IAccountRepository>();
        }

        [Test]
        public void AddUser_Should_Call_Add_Method()
        {
            // Arrange
            var user = User.Create("username", "email@example.com", "password");

            // Act
            _userRepositoryMock.Object.Create(user);

            // Assert
            _userRepositoryMock.Verify(static repo => repo.Create(It.IsAny<User>()), Times.Once);
        }
    }
}
