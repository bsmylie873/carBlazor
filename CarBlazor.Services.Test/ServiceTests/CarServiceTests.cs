using CarBlazor.DAL.Data;
using CarBlazor.DAL.Models;
using CarBlazor.Services.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Assert = Xunit.Assert;

namespace CarBlazor.Services.Test.ServiceTests;

public class CarServiceTests : IDisposable
{
    private readonly CarBlazorContext _context;
    private readonly CarService _carService;
    private readonly List<Car> _testCars;

    public CarServiceTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<CarBlazorContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new CarBlazorContext(options);
        _carService = new CarService(_context);

        // Setup test data
        _testCars = new List<Car>
        {
            new Car { Id = 1, Make = "Toyota", Model = "Corolla", Year = 2020, Color = "Blue" },
            new Car { Id = 2, Make = "Honda", Model = "Civic", Year = 2019, Color = "Red" },
            new Car { Id = 3, Make = "Ford", Model = "Mustang", Year = 2021, Color = "Black" }
        };

        // Seed database
        _context.Cars.AddRange(_testCars);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetCarsAsync_ShouldReturnAllCars()
    {
        // Act
        var result = await _carService.GetCarsAsync();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains(result, c => c.Id == 1);
        Assert.Contains(result, c => c.Id == 2);
        Assert.Contains(result, c => c.Id == 3);
    }

    [Fact]
    public async Task GetCarByIdAsync_WithExistingId_ShouldReturnCar()
    {
        // Act
        var result = await _carService.GetCarByIdAsync(2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Id);
        Assert.Equal("Honda", result.Make);
        Assert.Equal("Civic", result.Model);
    }

    [Fact]
    public async Task GetCarByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Act
        var result = await _carService.GetCarByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetCarsNotAssociatedWithCustomerAsync_ShouldReturnFilteredCars()
    {
        // Arrange
        var customerCarIds = new List<int> { 1, 3 };

        // Act
        var result = await _carService.GetCarsNotAssociatedWithCustomerAsync(customerCarIds);

        // Assert
        Assert.Single(result);
        Assert.Equal(2, result[0].Id);
        Assert.Equal("Honda", result[0].Make);
    }

    [Fact]
    public async Task CreateCarAsync_ShouldAddCarToContext()
    {
        // Arrange
        var newCar = new Car { Id = 4, Make = "Tesla", Model = "Model 3", Year = 2022, Color = "White" };

        // Act
        await _carService.CreateCarAsync(newCar);

        // Assert
        var addedCar = await _context.Cars.FindAsync(4);
        Assert.NotNull(addedCar);
        Assert.Equal("Tesla", addedCar.Make);
        Assert.Equal("Model 3", addedCar.Model);
        Assert.Equal(2022, addedCar.Year);
        Assert.Equal("White", addedCar.Color);
    }

    [Fact]
    public async Task UpdateCarAsync_WithExistingCar_ShouldUpdateAndReturnCar()
    {
        // Arrange
        var updatedCar = new Car { Id = 1, Make = "Toyota", Model = "Corolla", Year = 2022, Color = "Yellow" };

        // Act
        var result = await _carService.UpdateCarAsync(updatedCar);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Yellow", result.Color);
            
        // Verify in database
        var dbCar = await _context.Cars.FindAsync(1);
        Assert.Equal("Yellow", dbCar.Color);
        Assert.Equal(2022, dbCar.Year);
    }

    [Fact]
    public async Task UpdateCarAsync_WithNonExistingCar_ShouldReturnNull()
    {
        // Arrange
        var nonExistingCar = new Car { Id = 99, Make = "Unknown", Model = "Model", Year = 2020, Color = "Pink" };

        // Act
        var result = await _carService.UpdateCarAsync(nonExistingCar);

        // Assert
        Assert.Null(result);
            
        // Verify not in database
        Assert.Null(await _context.Cars.FindAsync(99));
    }

    [Fact]
    public async Task DeleteCarAsync_WithExistingId_ShouldReturnTrue()
    {
        // Act
        var result = await _carService.DeleteCarAsync(2);

        // Assert
        Assert.True(result);
        Assert.Null(await _context.Cars.FindAsync(2));
    }

    [Fact]
    public async Task DeleteCarAsync_WithNonExistingId_ShouldReturnFalse()
    {
        // Act
        var result = await _carService.DeleteCarAsync(99);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SearchCarsAsync_WithValidSearchTerm_ShouldReturnMatchingCars()
    {
        // Arrange
        var searchTerm = "Toyota";

        // Act
        var result = await _carService.SearchCarsAsync(searchTerm);

        // Assert
        Assert.Single(result);
        Assert.Equal("Toyota", result[0].Make);
    }

    [Fact]
    public async Task SearchCarsAsync_WithEmptySearchTerm_ShouldReturnAllCars()
    {
        // Act
        var result = await _carService.SearchCarsAsync("");

        // Assert
        Assert.Equal(3, result.Count);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}