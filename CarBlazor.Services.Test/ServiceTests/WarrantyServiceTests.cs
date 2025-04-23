using CarBlazor.DAL.Data;
using CarBlazor.DAL.Models;
using CarBlazor.Services.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Assert = Xunit.Assert;

namespace CarBlazor.Services.Test.ServiceTests;

public class WarrantyServiceTests : IDisposable
{
    private readonly CarBlazorContext _context;
    private readonly WarrantyService _warrantyService;

    public WarrantyServiceTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<CarBlazorContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new CarBlazorContext(options);
        _warrantyService = new WarrantyService(_context);

        // Seed test data
        var warrantyTypes = new List<WarrantyType>
        {
            new() { Id = 1, Name = "Basic" },
            new() { Id = 2, Name = "Extended" }
        };

        var cars = new List<Car>
        {
            new() { Id = 1, Make = "Toyota", Model = "Corolla", Year = 2020 },
            new() { Id = 2, Make = "Honda", Model = "Civic", Year = 2019 }
        };

        var warranties = new List<Warranty>
        {
            new()
            {
                Id = 1,
                CarId = 1,
                WarrantyTypeId = 1,
                Provider = "Provider A",
                StartDate = DateTime.Now.AddMonths(-12),
                EndDate = DateTime.Now.AddMonths(12),
                Cost = 500
            },
            new()
            {
                Id = 2,
                CarId = 2,
                WarrantyTypeId = 2,
                Provider = "Provider B",
                StartDate = DateTime.Now.AddMonths(-6),
                EndDate = DateTime.Now.AddMonths(18),
                Cost = 800
            }
        };

        _context.WarrantyTypes.AddRange(warrantyTypes);
        _context.Cars.AddRange(cars);
        _context.Warranties.AddRange(warranties);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetWarrantiesAsync_ShouldReturnAllWarranties()
    {
        // Act
        var result = await _warrantyService.GetWarrantiesAsync();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetWarrantyByIdAsync_WithExistingId_ShouldReturnWarranty()
    {
        // Act
        var result = await _warrantyService.GetWarrantyByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetWarrantyByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Act
        var result = await _warrantyService.GetWarrantyByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetWarrantiesForCarAsync_ShouldReturnWarrantiesForSpecificCar()
    {
        // Act
        var result = await _warrantyService.GetWarrantiesForCarAsync(1);

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].CarId);
    }

    [Fact]
    public async Task GetWarrantyTypesAsync_ShouldReturnAllWarrantyTypes()
    {
        // Act
        var result = await _warrantyService.GetWarrantyTypesAsync();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task CreateWarrantyAsync_ShouldAddWarrantyToContext()
    {
        // Arrange
        var newWarranty = new Warranty
        {
            Id = 3,
            CarId = 1,
            WarrantyTypeId = 2,
            Provider = "Provider C",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddYears(1),
            Cost = 600
        };

        // Act
        await _warrantyService.CreateWarrantyAsync(newWarranty);

        // Assert
        var addedWarranty = await _context.Warranties.FindAsync(3);
        Assert.NotNull(addedWarranty);
        Assert.Equal("Provider C", addedWarranty.Provider);
        Assert.Equal(1, addedWarranty.CarId);
        Assert.Equal(2, addedWarranty.WarrantyTypeId);
        Assert.Equal(600, addedWarranty.Cost);
    }

    [Fact]
    public async Task UpdateWarrantyAsync_WithExistingWarranty_ShouldUpdateAndReturnWarranty()
    {
        // Arrange
        var updatedWarranty = new Warranty
        {
            Id = 1,
            CarId = 1,
            WarrantyTypeId = 1,
            Provider = "Updated Provider",
            StartDate = DateTime.Now.AddMonths(-12),
            EndDate = DateTime.Now.AddMonths(12),
            Cost = 550
        };

        // Act
        var result = await _warrantyService.UpdateWarrantyAsync(updatedWarranty);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Provider", result.Provider);
    }

    [Fact]
    public async Task UpdateWarrantyAsync_WithNonExistingWarranty_ShouldReturnNull()
    {
        // Arrange
        var nonExistingWarranty = new Warranty
        {
            Id = 99,
            CarId = 1,
            WarrantyTypeId = 1,
            Provider = "Nonexistent Provider",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddYears(1),
            Cost = 500
        };

        // Act
        var result = await _warrantyService.UpdateWarrantyAsync(nonExistingWarranty);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteWarrantyAsync_WithExistingId_ShouldReturnTrue()
    {
        // Act
        var result = await _warrantyService.DeleteWarrantyAsync(1);

        // Assert
        Assert.True(result);
        Assert.Null(await _context.Warranties.FindAsync(1));
    }

    [Fact]
    public async Task DeleteWarrantyAsync_WithNonExistingId_ShouldReturnFalse()
    {
        // Act
        var result = await _warrantyService.DeleteWarrantyAsync(99);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SearchWarrantiesAsync_WithValidSearchTerm_ShouldReturnMatchingWarranties()
    {
        // Act
        var result = await _warrantyService.SearchWarrantiesAsync("Toyota");

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].CarId);
    }

    [Fact]
    public async Task SearchWarrantiesAsync_WithEmptySearchTerm_ShouldReturnAllWarranties()
    {
        // Act
        var result = await _warrantyService.SearchWarrantiesAsync("");

        // Assert
        Assert.Equal(2, result.Count);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}