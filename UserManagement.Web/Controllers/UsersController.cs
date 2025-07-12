using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly IUserActionLogService _logService;

    public UsersController(IUserService userService, IUserActionLogService logService)
    {
        _userService = userService;
        _logService = logService;
    }

    [HttpGet]
    public ViewResult List()
    {
        var items = _userService.GetAll().Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            DateOfBirth = p.DateOfBirth,
            Email = p.Email,
            IsActive = p.IsActive
        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }

    [HttpGet("active")]
    public ViewResult ListActive()
    {
        var items = _userService.FilterByActive(true).Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive
        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        // Always return the main List view
        return View("List", model);
    }

    [HttpGet("nonactive")]
    public ViewResult ListNonActive()
    {
        var items = _userService.FilterByActive(false).Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive
        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        // Always return the main List view
        return View("List", model);
    }

    [HttpGet("view/{id}")]
    public IActionResult ViewUser(long id)
    {
        var user = _userService.GetAll().FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();
        // Log the view action
        _logService.LogAction(new UserActionLog
        {
            UserId = (int)user.Id,
            ActionType = "View",
            Timestamp = DateTime.UtcNow,
            UserForename = user.Forename,
            UserSurname = user.Surname,
            UserEmail = user.Email
        });
        var logs = _logService.GetLogsByUserId((int)user.Id);
        var model = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive,
            Logs = logs
        };
        return View("~/Views/ViewUser/ViewUser.cshtml", model);
    }

    [HttpGet("edit/{id}")]
    public IActionResult EditUser(long id)
    {
        var user = _userService.GetAll().FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();
        var model = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive
        };
        return View("~/Views/EditUser/EditUser.cshtml", model);
    }

    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult EditUser(long id, UserListItemViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/EditUser/EditUser.cshtml", model);
        }

        var user = _userService.GetAll().FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();

        // Use null-coalescing operator to handle possible null values
        user.Forename = model.Forename ?? string.Empty;
        user.Surname = model.Surname ?? string.Empty;
        user.Email = model.Email ?? string.Empty;
        user.DateOfBirth = model.DateOfBirth;
        user.IsActive = model.IsActive;

        _userService.Update(user);

        // Log the edit action
        _logService.LogAction(new UserActionLog
        {
            UserId = (int)user.Id,
            ActionType = "Edit",
            Timestamp = DateTime.UtcNow,
            UserForename = user.Forename,
            UserSurname = user.Surname,
            UserEmail = user.Email
        });

        return RedirectToAction("List");
    }

    [HttpGet("delete/{id}")]
    public IActionResult DeleteUser(long id)
    {
        var user = _userService.GetAll().FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();
        var model = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive
        };
        return View("~/Views/DeleteUser/DeleteUser.cshtml", model);
    }

    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteUserConfirmed(long id)
    {
        var user = _userService.GetAll().FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();
        _userService.Delete(user);
        // Log the delete action
        _logService.LogAction(new UserActionLog
        {
            UserId = (int)user.Id,
            ActionType = "Delete",
            Timestamp = DateTime.UtcNow,
            UserForename = user.Forename,
            UserSurname = user.Surname,
            UserEmail = user.Email
        });
        // Check if the request came from the Delete Users view
        if (Request.Form["returnToDeleteList"] == "true")
            return RedirectToAction("ListUsersToBeDeleted");
        return RedirectToAction("List");
    }

    [HttpGet("listuserstobedeleted")]
    public IActionResult ListUsersToBeDeleted()
    {
        var items = _userService.GetAll().Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            DateOfBirth = p.DateOfBirth,
            Email = p.Email,
            IsActive = p.IsActive
        });
        var model = new UserListViewModel
        {
            Items = items.ToList()
        };
        return View("~/Views/DeleteUser/DeleteUsers.cshtml", model);
    }

    [HttpGet("adduser")]
    public IActionResult ShowAddUserView(UserListItemViewModel user)
    {
        var model = new UserListItemViewModel();
        return View("~/Views/AddUser/AddUser.cshtml", model);
    }

    [HttpPost("adduser")]
    [ValidateAntiForgeryToken]
    public IActionResult AddUser(UserListItemViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/CreateUser/CreateUser.cshtml", model);
        }
        var user = new User
        {
            Forename = model.Forename ?? string.Empty,
            Surname = model.Surname ?? string.Empty,
            Email = model.Email ?? string.Empty,
            DateOfBirth = model.DateOfBirth,
            IsActive = model.IsActive
        };
        _userService.AddUser(user);
        return RedirectToAction("List");
    }

    [HttpGet("logs")]
    public IActionResult Logs()
    {
        var logs = _logService.GetAllLogs()
            .OrderByDescending(l => l.Timestamp)
            .Select(l => new LogListItemViewModel
            {
                Id = l.Id,
                UserFullName = !string.IsNullOrEmpty(l.UserForename) && !string.IsNullOrEmpty(l.UserSurname)
                    ? l.UserForename + " " + l.UserSurname
                    : "Unknown User",
                ActionType = l.ActionType,
                Timestamp = l.Timestamp
            })
            .ToList();
        return View("~/Views/Logs/Logs.cshtml", logs);
    }

    [HttpGet("logdetails/{id}")]
    public IActionResult LogDetails(int id)
    {
        var log = _logService.GetAllLogs().FirstOrDefault(l => l.Id == id);
        if (log == null) return NotFound();
        var detailsModel = new LogDetailsViewModel
        {
            Id = log.Id,
            UserFullName = !string.IsNullOrEmpty(log.UserForename) && !string.IsNullOrEmpty(log.UserSurname)
                ? log.UserForename + " " + log.UserSurname
                : "Unknown User",
            UserEmail = log.UserEmail,
            ActionType = log.ActionType,
            Timestamp = log.Timestamp
        };
        return View("~/Views/Logs/LogDetails.cshtml", detailsModel);
    }
}
