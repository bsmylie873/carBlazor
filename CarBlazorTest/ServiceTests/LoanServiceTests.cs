using CarBlazor.DAL.Data;
using CarBlazor.DAL.Models;
using CarBlazor.Services.Services;
using Microsoft.EntityFrameworkCore;

namespace CarBlazorTest.ServiceTests;

public class LoanServiceTests : IDisposable
{
    private readonly CarBlazorContext _context;
    private readonly LoanService _loanService;

    public LoanServiceTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<CarBlazorContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new CarBlazorContext(options);
        _loanService = new LoanService(_context);

        // Setup test data
        List<Car> testCars = [
            new() { Id = 1, Make = "Toyota", Model = "Corolla", Year = 2020, Color = "Blue" },
            new() { Id = 2, Make = "Honda", Model = "Civic", Year = 2019, Color = "Red" },
            new() { Id = 3, Make = "Ford", Model = "Mustang", Year = 2021, Color = "Black" }
        ];

        List<Customer> testCustomers = [
            new() { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" },
            new() { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com" }
        ];
            
        List<LoanStatus> loanStatuses = [
            new() { Id = 1, Name = "Active" },
            new() { Id = 2, Name = "Paid Off" },
            new() { Id = 3, Name = "Defaulted" }
        ];
            
        List<Loan> testLoans = [
            new()
            {
                Id = 1,
                CustomerId = 1,
                CarId = 1,
                Amount = 20000,
                StartDate = DateTime.Now.AddMonths(-6),
                Status = loanStatuses[0],
                StatusId = 1
            },

            new()
            {
                Id = 2,
                CustomerId = 1,
                CarId = 2,
                Amount = 15000,
                StartDate = DateTime.Now.AddMonths(-12),
                Status = loanStatuses[1],
                StatusId = 2
            },

            new()
            {
                Id = 3,
                CustomerId = 2,
                CarId = 3,
                Amount = 35000,
                StartDate = DateTime.Now.AddMonths(-3),
                Status = loanStatuses[2],
                StatusId = 3
            }
        ];

        // Seed database
        _context.Car.AddRange(testCars);
        _context.Customer.AddRange(testCustomers);
        _context.Loan.AddRange(testLoans);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetLoansAsync_ShouldReturnAllLoans()
    {
        // Act
        var result = await _loanService.GetLoansAsync();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains(result, l => l.Id == 1);
        Assert.Contains(result, l => l.Id == 2);
        Assert.Contains(result, l => l.Id == 3);
    }

    [Fact]
    public async Task GetLoanByIdAsync_WithExistingId_ShouldReturnLoan()
    {
        // Act
        var result = await _loanService.GetLoanByIdAsync(2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Id);
        Assert.Equal(15000, result.Amount);
    }

    [Fact]
    public async Task GetLoanByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Act
        var result = await _loanService.GetLoanByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateLoanAsync_ShouldAddLoanToContext()
    {
        // Arrange
        var newLoan = new Loan
        {
            Id = 4,
            CustomerId = 2,
            CarId = 2,
            Amount = 18000,
            StartDate = DateTime.Now,
            StatusId = 1
        };

        // Act
        await _loanService.CreateLoanAsync(newLoan);

        // Assert
        var addedLoan = await _context.Loan.FindAsync(4);
        Assert.NotNull(addedLoan);
        Assert.Equal(2, addedLoan.CustomerId);
        Assert.Equal(2, addedLoan.CarId);
        Assert.Equal(18000, addedLoan.Amount);
    }

    [Fact]
    public async Task UpdateLoanAsync_WithExistingLoan_ShouldUpdateAndReturnLoan()
    {
        // Arrange
        var updatedLoan = new Loan
        {
            Id = 1,
            CustomerId = 1,
            CarId = 1,
            Amount = 22000,
            StartDate = DateTime.Now.AddMonths(-6),
            StatusId = 1
        };

        // Act
        var result = await _loanService.UpdateLoanAsync(updatedLoan);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(22000, result.Amount);

        // Verify in database
        var dbLoan = await _context.Loan.FindAsync(1);
        Assert.Equal(22000, dbLoan!.Amount);
    }

    [Fact]
    public async Task UpdateLoanAsync_WithNonExistingLoan_ShouldReturnNull()
    {
        // Arrange
        var nonExistingLoan = new Loan
        {
            Id = 99,
            CustomerId = 1,
            CarId = 1,
            Amount = 10000
        };

        // Act
        var result = await _loanService.UpdateLoanAsync(nonExistingLoan);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteLoanAsync_WithExistingId_ShouldReturnTrue()
    {
        // Act
        var result = await _loanService.DeleteLoanAsync(3);

        // Assert
        Assert.True(result);
        Assert.Null(await _context.Loan.FindAsync(3));
    }

    [Fact]
    public async Task DeleteLoanAsync_WithNonExistingId_ShouldReturnFalse()
    {
        // Act
        var result = await _loanService.DeleteLoanAsync(99);

        // Assert
        Assert.False(result);
    }
        
    [Fact]
    public async Task SearchLoansAsync_WithAmountSearchTerm_ShouldReturnMatchingLoans()
    {
        // Arrange
        var searchTerm = "35000";

        // Act
        var result = await _loanService.SearchLoansAsync(searchTerm);

        // Assert
        Assert.Single(result);
        Assert.Equal(3, result[0].Id);
    }

    [Fact]
    public async Task SearchLoansAsync_WithStatusSearchTerm_ShouldReturnMatchingLoans()
    {
        // Arrange
        var searchTerm = "Active";

        // Act
        var result = await _loanService.SearchLoansAsync(searchTerm);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task SearchLoansAsync_WithEmptySearchTerm_ShouldReturnAllLoans()
    {
        // Act
        var result = await _loanService.SearchLoansAsync("");

        // Assert
        Assert.Equal(3, result.Count);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}