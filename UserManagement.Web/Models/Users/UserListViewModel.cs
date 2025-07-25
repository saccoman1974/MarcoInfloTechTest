﻿using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Web.Models.Users;

public class UserListViewModel
{
    public List<UserListItemViewModel> Items { get; set; } = new();
}

public class UserListItemViewModel
{
    public long Id { get; set; }
    public string? Forename { get; set; }
    public string? Surname { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateOfBirth { get; set; }
    public IEnumerable<UserActionLog>? Logs { get; set; } // Add logs property
}
