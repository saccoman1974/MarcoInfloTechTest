using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public void ListActive_ReturnsListView()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.FilterByActive(true)).Returns(new List<User>());
            var controller = new UsersController(mockService.Object);

            // Act
            var result = controller.ListActive();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("List", viewResult.ViewName);
            Assert.IsType<UserListViewModel>(viewResult.Model);
        }

        [Fact]
        public void ListNonActive_ReturnsListView()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.FilterByActive(false)).Returns(new List<User>());
            var controller = new UsersController(mockService.Object);

            // Act
            var result = controller.ListActive();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("List", viewResult.ViewName);
            Assert.IsType<UserListViewModel>(viewResult.Model);
        }

        [Fact]
        public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
        {
            var service = CreateService();
            var users = SetupUsers();
            var result = service.GetAll();
            result.Should().BeSameAs(users);
        }

        [Fact]
        public void FilterByActive_ReturnsOnlyActiveOrNonActiveUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Forename = "Active", Surname = "User", Email = "active@example.com", IsActive = true },
                new User { Forename = "Inactive", Surname = "User", Email = "inactive@example.com", IsActive = false },
                new User { Forename = "AnotherActive", Surname = "User", Email = "active2@example.com", IsActive = true }
            }.AsQueryable();
            _dataContext.Setup(s => s.GetAll<User>()).Returns(users);
            var service = CreateService();

            // Act
            var activeUsers = service.FilterByActive(true).ToList();
            var nonActiveUsers = service.FilterByActive(false).ToList();

            // Assert
            activeUsers.Should().OnlyContain(u => u.IsActive);
            activeUsers.Should().HaveCount(2);
            nonActiveUsers.Should().OnlyContain(u => !u.IsActive);
            nonActiveUsers.Should().HaveCount(1);
        }

        private IQueryable<User> SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
        {
            var users = new[]
            {
                new User
                {
                    Forename = forename,
                    Surname = surname,
                    Email = email,
                    IsActive = isActive
                }
            }.AsQueryable();

            _dataContext
                .Setup(s => s.GetAll<User>())
                .Returns(users);

            return users;
        }

        private readonly Mock<IDataContext> _dataContext = new();
        private UserService CreateService() => new(_dataContext.Object);
    }
}
