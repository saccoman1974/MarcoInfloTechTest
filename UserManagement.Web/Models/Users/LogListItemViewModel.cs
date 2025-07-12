using System;

namespace UserManagement.Web.Models.Users
{
    public class LogListItemViewModel
    {
        public int Id { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public string ActionType { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
