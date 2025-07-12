using UserManagement.Services.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
// UserActionLog is defined in the global namespace (no namespace)

public class UserActionLogService : IUserActionLogService
{
    private readonly List<UserActionLog> _logs = new();
    private int _nextId = 1;

    public void LogAction(UserActionLog logEntry)
    {
        logEntry.Id = _nextId++;
        _logs.Add(logEntry);
    }

    public IEnumerable<UserActionLog> GetAllLogs()
    {
        return _logs;
    }

    public IEnumerable<UserActionLog> GetLogsByUserId(int userId)
    {
        return _logs.Where(l => l.UserId == userId);
    }
}
