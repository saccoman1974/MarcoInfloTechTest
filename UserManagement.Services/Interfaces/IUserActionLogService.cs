using System.Collections.Generic;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserActionLogService
{
    /// <summary>
    /// Logs an action performed by a user.
    /// </summary>
    /// <param name="logEntry">The log entry to be added.</param>
    void LogAction(UserActionLog logEntry);
    /// <summary>
    /// Retrieves all action logs.
    /// </summary>
    /// <returns>A collection of user action logs.</returns>
    IEnumerable<UserActionLog> GetAllLogs();
    /// <summary>
    /// Retrieves action logs for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose logs are to be retrieved.</param>
    /// <returns>A collection of user action logs for the specified user.</returns>
    IEnumerable<UserActionLog> GetLogsByUserId(int userId);
}
