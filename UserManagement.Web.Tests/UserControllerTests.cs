using System;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public void EditUser_Post_ValidModel_UpdatesUserAndRedirects()
    {
        // Arrange
        var user = new User { Id = 1, Forename = "Old", Surname = "Name", Email = "old@example.com", DateOfBirth = DateTime.Today.AddYears(-30), IsActive = false };
        _userService.Setup(s => s.GetAll()).Returns(new[] { user });
        var controller = CreateController();

        var updatedModel = new UserListItemViewModel
        {
            Id = 1,
            Forename = "New",
            Surname = "Name",
            Email = "new@example.com",
            DateOfBirth = DateTime.Today.AddYears(-25),
            IsActive = true
        };

        // Act
        var result = controller.EditUser(1, updatedModel);

        // Assert
        _userService.Verify(s => s.Update(It.Is<User>(u =>
            u.Id == 1 &&
            u.Forename == "New" &&
            u.Surname == "Name" &&
            u.Email == "new@example.com" &&
            u.DateOfBirth == updatedModel.DateOfBirth &&
            u.IsActive == true
        )), Times.Once);

        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be("List");
    }

    [Fact]
    public void EditUser_ReturnsEditUserViewWithModel()
    {
        // Arrange
        var user = new User { Id = 1, Forename = "Edit", Surname = "User", Email = "edit@example.com", DateOfBirth = DateTime.Today, IsActive = true };
        _userService.Setup(s => s.GetAll()).Returns(new[] { user }); 
        var controller = CreateController();

        // Act
        var result = controller.EditUser(1) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result!.ViewName.Should().Contain("EditUser");
        result.Model.Should().BeOfType<UserListItemViewModel>();
        ((UserListItemViewModel)result.Model).Id.Should().Be(1);
    }

    [Fact]
    public void ViewUser_ReturnsViewUserViewWithModel()
    {
        // Arrange
        var user = new User { Id = 2, Forename = "View", Surname = "User", Email = "view@example.com", DateOfBirth = DateTime.Today.AddYears(-20), IsActive = false };
        _userService.Setup(s => s.GetAll()).Returns(new[] { user });
        var controller = CreateController();

        // Act
        var result = controller.ViewUser(2) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result!.ViewName.Should().Contain("ViewUser");
        result.Model.Should().BeOfType<UserListItemViewModel>();
        ((UserListItemViewModel)result.Model).Id.Should().Be(2);
    }

    [Fact]
    public void DeleteUser_ReturnsDeleteUserViewWithModel()
    {
        // Arrange
        var user = new User { Id = 3, Forename = "Delete", Surname = "User", Email = "delete@example.com", DateOfBirth = DateTime.Today.AddYears(-30), IsActive = true };
        _userService.Setup(s => s.GetAll()).Returns(new[] { user });
        var controller = CreateController();

        // Act
        var result = controller.DeleteUser(3) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result!.ViewName.Should().Contain("DeleteUser");
        result.Model.Should().BeOfType<UserListItemViewModel>();
        ((UserListItemViewModel)result.Model).Id.Should().Be(3);
    }

    [Fact]
    public void List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.List();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
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
        };

        _userService
            .Setup(s => s.GetAll())
            .Returns(users);

        return users;
    }

    private readonly Mock<IUserService> _userService = new();
    private UsersController CreateController() => new(_userService.Object);
}
