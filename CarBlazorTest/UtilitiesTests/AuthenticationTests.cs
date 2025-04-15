using CarBlazor.DAL.Utilities;

namespace CarBlazorTest.UtilitiesTests;

public class AuthenticationTests
{
    [Fact]
    public void HashPassword_ShouldGenerateHashAndSalt()
    {
        // Arrange
        var password = "TestPassword123";

        // Act
        var (hash, salt) = Authentication.HashPassword(password);

        // Assert
        Assert.False(string.IsNullOrEmpty(hash), "Hash should not be null or empty.");
        Assert.False(string.IsNullOrEmpty(salt), "Salt should not be null or empty.");
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrueForValidPassword()
    {
        // Arrange
        var password = "TestPassword123";
        var (hash, salt) = Authentication.HashPassword(password);

        // Act
        var isValid = Authentication.VerifyPassword(password, hash, salt);

        // Assert
        Assert.True(isValid, "Password verification should return true for a valid password.");
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalseForInvalidPassword()
    {
        // Arrange
        var password = "TestPassword123";
        var (hash, salt) = Authentication.HashPassword(password);
        var invalidPassword = "WrongPassword";

        // Act
        var isValid = Authentication.VerifyPassword(invalidPassword, hash, salt);

        // Assert
        Assert.False(isValid, "Password verification should return false for an invalid password.");
    }
}