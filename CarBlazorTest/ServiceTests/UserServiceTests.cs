using CarBlazor.DAL.Data;
using CarBlazor.DAL.Models;
using CarBlazor.Services.Services;
using Microsoft.EntityFrameworkCore;

namespace CarBlazorTest.ServiceTests;

public class UserServiceTests : IDisposable
{
    private readonly CarBlazorContext _context;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<CarBlazorContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new CarBlazorContext(options);
        _userService = new UserService(_context);

        // Seed test data
        var roles = new List<Role>
        {
            new() { Id = "Admin", Name = "Administrator" },
            new() { Id = "User", Name = "User" }
        };

        var users = new List<User>
        {
            new() { Id = 1, Username = "john_doe", RoleId = "Admin", CreatedAt = DateTime.UtcNow },
            new() { Id = 2, Username = "jane_doe", RoleId = "User", CreatedAt = DateTime.UtcNow }
        };

        _context.Roles.AddRange(roles);
        _context.Users.AddRange(users);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetUsersAsync_ShouldReturnAllUsers()
    {
        // Act
        var result = await _userService.GetUsersAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, u => u.Username == "john_doe");
        Assert.Contains(result, u => u.Username == "jane_doe");
    }

    [Fact]
    public async Task GetUserByIdAsync_WithExistingId_ShouldReturnUser()
    {
        // Act
        var result = await _userService.GetUserByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("john_doe", result.Username);
    }

    [Fact]
    public async Task GetUserByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Act
        var result = await _userService.GetUserByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UsernameExistsAsync_WithExistingUsername_ShouldReturnTrue()
    {
        // Act
        var result = await _userService.UsernameExistsAsync("john_doe");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UsernameExistsAsync_WithNonExistingUsername_ShouldReturnFalse()
    {
        // Act
        var result = await _userService.UsernameExistsAsync("nonexistent_user");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldAddUserToContext()
    {
        // Act
        var newUser = await _userService.CreateUserAsync("new_user", "password123", "User");

        // Assert
        var addedUser = await _context.Users.FindAsync(newUser.Id);
        Assert.NotNull(addedUser);
        Assert.Equal("new_user", addedUser.Username);
    }

    [Fact]
    public async Task UpdateUserAsync_WithExistingUser_ShouldUpdateAndReturnUser()
    {
        // Arrange
        var updatedUser = new User { Id = 1, Username = "updated_user", RoleId = "User" };

        // Act
        var result = await _userService.UpdateUserAsync(updatedUser);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("updated_user", result.Username);
    }

    [Fact]
    public async Task UpdateUserAsync_WithNonExistingUser_ShouldReturnNull()
    {
        // Arrange
        var nonExistingUser = new User { Id = 99, Username = "nonexistent_user", RoleId = "User" };

        // Act
        var result = await _userService.UpdateUserAsync(nonExistingUser);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteUserAsync_WithExistingId_ShouldReturnTrue()
    {
        // Act
        var result = await _userService.DeleteUserAsync(1);

        // Assert
        Assert.True(result);
        Assert.Null(await _context.Users.FindAsync(1));
    }

    [Fact]
    public async Task DeleteUserAsync_WithNonExistingId_ShouldReturnFalse()
    {
        // Act
        var result = await _userService.DeleteUserAsync(99);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SearchUsersAsync_WithValidSearchTerm_ShouldReturnMatchingUsers()
    {
        // Act
        var result = await _userService.SearchUsersAsync("john");

        // Assert
        Assert.Single(result);
        Assert.Equal("john_doe", result[0].Username);
    }

    [Fact]
    public async Task SearchUsersAsync_WithEmptySearchTerm_ShouldReturnAllUsers()
    {
        // Act
        var result = await _userService.SearchUsersAsync("");

        // Assert
        Assert.Equal(2, result.Count);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}