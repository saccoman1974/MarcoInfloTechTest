using System.Linq;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

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
        var model = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive
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
        return RedirectToAction("ListUsersToBeDeleted");
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
}
