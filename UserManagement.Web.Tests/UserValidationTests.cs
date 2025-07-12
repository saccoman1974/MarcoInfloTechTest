using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using UserManagement.Web.Models.Users;

namespace UserManagement.Data.Tests;

public class UserValidationTests
{
    [Fact]
    public void UserListItemViewModel_InvalidEmail_ShouldFailValidation()
    {
        // Arrange
        var model = new UserListItemViewModel
        {
            Forename = "Test",
            Surname = "User",
            Email = "not-an-email", // Invalid email
            DateOfBirth = DateTime.Today.AddYears(-20),
            IsActive = true
        };

        var validationContext = new ValidationContext(model);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(model, validationContext, results, true);

        // Assert
        isValid.Should().BeFalse();
        results.Should().Contain(r => r.MemberNames.Contains("Email"));
    }
}
