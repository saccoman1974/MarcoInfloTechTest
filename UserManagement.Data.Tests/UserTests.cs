using System;
using UserManagement.Models;

namespace UserManagement.Data.Tests
{
    public class UserTests
    {
        [Fact]
        public void User_Should_Have_DateOfBirth_Property()
        {
            // Arrange
            var user = new User();
            var dob = new DateTime(1990, 1, 1);

            // Act
            user.DateOfBirth = dob;

            // Assert
            user.DateOfBirth.Should().Be(dob);
        }
    }
}
