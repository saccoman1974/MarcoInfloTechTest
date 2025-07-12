using System;
using UserManagement.Models;

public class UserActionLog
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    // Navigation property to the User entity
    public virtual User User { get; set; } = null!;
    // Snapshot fields for user details
    public string? UserForename { get; set; }
    public string? UserSurname { get; set; }
    public string? UserEmail { get; set; }
}
