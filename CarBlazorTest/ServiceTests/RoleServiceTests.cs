using CarBlazor.DAL.Data;
using CarBlazor.DAL.Models;
using CarBlazor.Services.Services;
using Microsoft.EntityFrameworkCore;

namespace CarBlazorTest.ServiceTests;

public class RoleServiceTests : IDisposable
{
    private readonly CarBlazorContext _context;
    private readonly RoleService _roleService;

    public RoleServiceTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<CarBlazorContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new CarBlazorContext(options);
        _roleService = new RoleService(_context);

        // Seed test data
        var roles = new List<Role>
        {
            new() { Id = "Admin", Name = "Administrator" },
            new() { Id = "User", Name = "User" },
            new() { Id = "Manager", Name = "Manager" }
        };

        _context.Roles.AddRange(roles);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetRolesAsync_WithoutIncludeAdmin_ShouldExcludeAdminRole()
    {
        // Act
        var result = await _roleService.GetRolesAsync(includeAdmin: false);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.DoesNotContain(result, r => r.Id == "Admin");
    }

    [Fact]
    public async Task GetRolesAsync_WithIncludeAdmin_ShouldIncludeAllRoles()
    {
        // Act
        var result = await _roleService.GetRolesAsync(includeAdmin: true);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains(result, r => r.Id == "Admin");
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}