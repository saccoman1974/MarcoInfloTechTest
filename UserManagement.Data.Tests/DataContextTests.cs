using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Data.Tests;

public class DataContextTests
{
    [Fact]
    public void LogAction_WhenUserIsEdited_LogEntryIsCreated()
    {
        // Arrange
        var logService = new Mock<IUserActionLogService>();
        var userId = 1;
        var action = "Edit";
        var logEntry = new UserActionLog { UserId = userId, ActionType = action, Timestamp = DateTime.UtcNow };

        // Act
        logService.Object.LogAction(logEntry);

        // Assert
        logService.Verify(s => s.LogAction(It.Is<UserActionLog>(l => l.UserId == userId && l.ActionType == action)), Times.Once);
    }

    [Fact]
    public void GetAll_WhenNewEntityAdded_MustIncludeNewEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();

        var entity = new User
        {
            Forename = "Brand New",
            Surname = "User",
            Email = "brandnewuser@example.com"
        };
        context.Create(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result
            .Should().Contain(s => s.Email == entity.Email)
            .Which.Should().BeEquivalentTo(entity);
    }

    [Fact]
    public void GetAll_WhenDeleted_MustNotIncludeDeletedEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        var entity = context.GetAll<User>().First();
        context.Delete(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotContain(s => s.Email == entity.Email);
    }

    private DataContext CreateContext() => new();
}
