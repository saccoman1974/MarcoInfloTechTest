using System;

namespace UserManagement.Web.Models.Users
{
    public class LogDetailsViewModel
    {
        public int Id { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public string? UserEmail { get; set; }
        public string ActionType { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
