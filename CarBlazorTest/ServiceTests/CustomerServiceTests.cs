using CarBlazor.DAL.Data;
using CarBlazor.DAL.Models;
using CarBlazor.Services.Services;
using Microsoft.EntityFrameworkCore;

namespace CarBlazorTest.ServiceTests;

public class CustomerServiceTests : IDisposable
{
    private readonly CarBlazorContext _context;
    private readonly CustomerService _customerService;
    private readonly List<Customer> _testCustomers;
    private readonly List<Car> _testCars;
    private readonly List<CustomerCar> _testCustomerCars;

    public CustomerServiceTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<CarBlazorContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new CarBlazorContext(options);
        _customerService = new CustomerService(_context);

        // Setup test data
        _testCars =
        [
            new Car { Id = 1, Make = "Toyota", Model = "Corolla", Year = 2020, Color = "Blue" },
            new Car { Id = 2, Make = "Honda", Model = "Civic", Year = 2019, Color = "Red" },
            new Car { Id = 3, Make = "Ford", Model = "Mustang", Year = 2021, Color = "Black" }
        ];

        _testCustomers =
        [
            new Customer
            {
                Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com",
                PhoneNumber = "123-456-7890", Address = "123 Main St"
            },
            new Customer
            {
                Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com",
                PhoneNumber = "234-567-8901", Address = "456 Oak Ave"
            },
            new Customer
            {
                Id = 3, FirstName = "Bob", LastName = "Johnson", Email = "bob@example.com",
                PhoneNumber = "345-678-9012", Address = "789 Pine Dr"
            }
        ];

        _testCustomerCars =
        [
            new CustomerCar { Id = 1, CustomerId = 1, CarId = 1},
            new CustomerCar { Id = 2, CustomerId = 1, CarId = 2},
            new CustomerCar { Id = 3, CustomerId = 2, CarId = 3}
        ];

        // Seed database
        _context.Car.AddRange(_testCars);
        _context.Customer.AddRange(_testCustomers);
        _context.CustomerCar.AddRange(_testCustomerCars);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetCustomersAsync_ShouldReturnAllCustomersWithCars()
    {
        // Act
        var result = await _customerService.GetCustomersAsync();

        // Assert
        Assert.Equal(3, result.Count);
            
        // Check customer 1 has 2 cars
        var customer1 = result.FirstOrDefault(c => c.Id == 1);
        Assert.NotNull(customer1);
        Assert.Equal(2, customer1.CustomerCars!.Count);
            
        // Check customer 2 has 1 car
        var customer2 = result.FirstOrDefault(c => c.Id == 2);
        Assert.NotNull(customer2);
        Assert.Single(customer2.CustomerCars!);
            
        // Check customer 3 has 0 cars
        var customer3 = result.FirstOrDefault(c => c.Id == 3);
        Assert.NotNull(customer3);
        Assert.Empty(customer3.CustomerCars!);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_WithExistingId_ShouldReturnCustomer()
    {
        // Act
        var result = await _customerService.GetCustomerByIdAsync(2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Id);
        Assert.Equal("Jane", result.FirstName);
        Assert.Equal("Smith", result.LastName);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Act
        var result = await _customerService.GetCustomerByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetCustomerCarsAsync_ShouldReturnCustomerCars()
    {
        // Act
        var result = await _customerService.GetCustomerCarsAsync(1);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, cc => cc.CarId == 1);
        Assert.Contains(result, cc => cc.CarId == 2);
            
        // Check car details are included
        Assert.Equal("Toyota", result.FirstOrDefault(cc => cc.CarId == 1)?.Car?.Make);
        Assert.Equal("Honda", result.FirstOrDefault(cc => cc.CarId == 2)?.Car?.Make);
    }

    [Fact]
    public async Task CreateCustomerAsync_ShouldAddCustomerToContext()
    {
        // Arrange
        var newCustomer = new Customer 
        { 
            Id = 4, 
            FirstName = "Alice", 
            LastName = "Brown", 
            Email = "alice@example.com", 
            PhoneNumber = "456-789-0123", 
            Address = "101 Maple St" 
        };

        // Act
        await _customerService.CreateCustomerAsync(newCustomer);

        // Assert
        var addedCustomer = await _context.Customer.FindAsync(4);
        Assert.NotNull(addedCustomer);
        Assert.Equal("Alice", addedCustomer.FirstName);
        Assert.Equal("Brown", addedCustomer.LastName);
    }

    [Fact]
    public async Task AddCarToCustomerAsync_ShouldAddCarToCustomer()
    {
        // Arrange
        var customerCar = new CustomerCar 
        { 
            Id = 4, 
            CustomerId = 3, 
            CarId = 1, 
        };

        // Act
        await _customerService.AddCarToCustomerAsync(customerCar);

        // Assert
        var addedCustomerCar = await _context.CustomerCar.FindAsync(4);
        Assert.NotNull(addedCustomerCar);
        Assert.Equal(3, addedCustomerCar.CustomerId);
        Assert.Equal(1, addedCustomerCar.CarId);
    }

    [Fact]
    public async Task UpdateCustomerAsync_WithExistingCustomer_ShouldUpdateAndReturnCustomer()
    {
        // Arrange
        var updatedCustomer = new Customer 
        { 
            Id = 1, 
            FirstName = "John", 
            LastName = "Doe", 
            Email = "john.updated@example.com", 
            PhoneNumber = "987-654-3210", 
            Address = "Updated Address" 
        };

        // Act
        var result = await _customerService.UpdateCustomerAsync(updatedCustomer);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("john.updated@example.com", result.Email);
            
        // Verify in database
        var dbCustomer = await _context.Customer.FindAsync(1);
        Assert.Equal("john.updated@example.com", dbCustomer!.Email);
        Assert.Equal("987-654-3210", dbCustomer.PhoneNumber);
        Assert.Equal("Updated Address", dbCustomer.Address);
    }

    [Fact]
    public async Task UpdateCustomerAsync_WithNonExistingCustomer_ShouldReturnNull()
    {
        // Arrange
        var nonExistingCustomer = new Customer 
        { 
            Id = 99, 
            FirstName = "Nobody", 
            LastName = "NoName", 
            Email = "nobody@example.com" 
        };

        // Act
        var result = await _customerService.UpdateCustomerAsync(nonExistingCustomer);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteCustomerAsync_WithExistingId_ShouldReturnTrue()
    {
        // Act
        var result = await _customerService.DeleteCustomerAsync(3);

        // Assert
        Assert.True(result);
        Assert.Null(await _context.Customer.FindAsync(3));
    }

    [Fact]
    public async Task DeleteCustomerAsync_WithNonExistingId_ShouldReturnFalse()
    {
        // Act
        var result = await _customerService.DeleteCustomerAsync(99);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task RemoveCarFromCustomerAsync_WithExistingId_ShouldReturnTrue()
    {
        // Act
        var result = await _customerService.RemoveCarFromCustomerAsync(2);

        // Assert
        Assert.True(result);
        Assert.Null(await _context.CustomerCar.FindAsync(2));
    }

    [Fact]
    public async Task RemoveCarFromCustomerAsync_WithNonExistingId_ShouldReturnFalse()
    {
        // Act
        var result = await _customerService.RemoveCarFromCustomerAsync(99);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SearchCustomersAsync_WithValidSearchTerm_ShouldReturnMatchingCustomers()
    {
        // Arrange
        var searchTerm = "Smith";

        // Act
        var result = await _customerService.SearchCustomersAsync(searchTerm);

        // Assert
        Assert.Single(result);
        Assert.Equal("Jane", result[0].FirstName);
        Assert.Equal("Smith", result[0].LastName);
    }

    [Fact]
    public async Task SearchCustomersAsync_WithEmailSearchTerm_ShouldReturnMatchingCustomers()
    {
        // Arrange
        var searchTerm = "bob@example";

        // Act
        var result = await _customerService.SearchCustomersAsync(searchTerm);

        // Assert
        Assert.Single(result);
        Assert.Equal("Bob", result[0].FirstName);
    }

    [Fact]
    public async Task SearchCustomersAsync_WithEmptySearchTerm_ShouldReturnAllCustomers()
    {
        // Act
        var result = await _customerService.SearchCustomersAsync("");

        // Assert
        Assert.Equal(3, result.Count);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}