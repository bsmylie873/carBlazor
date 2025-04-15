using CarBlazor.DAL.Data;
using CarBlazor.DAL.Models;
using CarBlazor.Services.Services;
using CarBlazorTest.ServiceTests.Utilities;
using Microsoft.EntityFrameworkCore;
using static CarBlazor.DAL.Utilities.Authentication;

namespace CarBlazorTest.ServiceTests;

public class AuthServiceTests : IDisposable
{
    private readonly CarBlazorContext _context;
    private readonly TestHttpContextAccessor _httpContextAccessor;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        // Set up in-memory database
        var options = new DbContextOptionsBuilder<CarBlazorContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new CarBlazorContext(options);
        _httpContextAccessor = new TestHttpContextAccessor();
        _authService = new AuthService(_httpContextAccessor, _context);

        // Seed test data
        SeedDatabase();
    }

    private void SeedDatabase()
    {
        var role = new Role { Id = "admin", Name = "Administrator" };
        _context.Roles.Add(role);

        var (hash, salt) = HashPassword("password123");
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            PasswordHash = hash,
            Salt = salt,
            RoleId = "admin",
            Role = role,
            ForcePasswordReset = false
        };

        var resetUser = new User
        {
            Id = 2,
            Username = "resetuser",
            PasswordHash = hash,
            Salt = salt,
            RoleId = "admin",
            Role = role,
            ForcePasswordReset = true
        };

        _context.Users.Add(user);
        _context.Users.Add(resetUser);
        _context.SaveChanges();
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnSuccess()
    {
        // Act
        var result = await _authService.LoginAsync("testuser", "password123", false);

        // Assert
        Assert.True(result.Success);
        Assert.False(result.PasswordResetRequired);
        Assert.Equal(1, result.UserId);
        Assert.True(_httpContextAccessor.SignInCalled);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidUsername_ShouldReturnFailure()
    {
        // Act
        var result = await _authService.LoginAsync("nonexistentuser", "password123", false);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0, result.UserId);
        Assert.False(_httpContextAccessor.SignInCalled);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ShouldReturnFailure()
    {
        // Act
        var result = await _authService.LoginAsync("testuser", "wrongpassword", false);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0, result.UserId);
        Assert.False(_httpContextAccessor.SignInCalled);
    }

    [Fact]
    public async Task LoginAsync_WithPasswordResetRequired_ShouldReturnSuccess()
    {
        // Act
        var result = await _authService.LoginAsync("resetuser", "password123", false);

        // Assert
        Assert.True(result.Success);
        Assert.True(result.PasswordResetRequired);
        Assert.Equal(2, result.UserId);
        Assert.True(_httpContextAccessor.SignInCalled);
    }

    [Fact]
    public async Task LogoutAsync_ShouldCallSignOut()
    {
        // Act
        await _authService.LogoutAsync();

        // Assert
        Assert.True(_httpContextAccessor.SignOutCalled);
    }

    [Fact]
    public async Task ChangePasswordAsync_WithValidCurrentPassword_ShouldSucceed()
    {
        // Act
        var result = await _authService.ChangePasswordAsync(1, "password123", "newpassword123");

        // Assert
        Assert.True(result.Success);

        // Verify the password was actually changed
        var user = await _context.Users.FindAsync(1);
        Assert.True(VerifyPassword("newpassword123", user!.PasswordHash, user.Salt));
    }

    [Fact]
    public async Task ChangePasswordAsync_WithInvalidCurrentPassword_ShouldFail()
    {
        // Act
        var result = await _authService.ChangePasswordAsync(1, "wrongpassword", "newpassword123");

        // Assert
        Assert.False(result.Success);
        Assert.Contains("incorrect", result.Message);
    }

    [Fact]
    public async Task RegisterUserAsync_WithNewUsername_ShouldSucceed()
    {
        // Act
        var result = await _authService.RegisterUserAsync("newuser", "password123", "admin");

        // Assert
        Assert.True(result);

        // Verify user was added to database
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "newuser");
        Assert.NotNull(user);
    }

    [Fact]
    public async Task RegisterUserAsync_WithExistingUsername_ShouldFail()
    {
        // Act
        var result = await _authService.RegisterUserAsync("testuser", "password123", "admin");

        // Assert
        Assert.False(result);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}