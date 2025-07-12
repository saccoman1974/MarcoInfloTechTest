using System;
using System.Linq;

public class UserActionLogServiceTests
{
    [Fact]
    public void LogAction_And_GetLogsByUserId_ShouldStoreAndRetrieveLog()
    {
        // Arrange
        var service = new UserActionLogService();
        var log = new UserActionLog
        {
            Id = 1,
            UserId = 42,
            ActionType = "TestAction",
            Timestamp = DateTime.UtcNow
        };

        // Act
        service.LogAction(log);
        var logs = service.GetLogsByUserId(42).ToList();

        // Assert
        Assert.Single(logs);
        Assert.Equal("TestAction", logs[0].ActionType);
        Assert.Equal(42, logs[0].UserId);
    }
}
