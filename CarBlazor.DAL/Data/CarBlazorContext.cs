using System.Diagnostics.CodeAnalysis;
using CarBlazor.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CarBlazor.DAL.Data;

[ExcludeFromCodeCoverage]
public class CarBlazorContext : DbContext
{
    public CarBlazorContext(DbContextOptions<CarBlazorContext> options)
        : base(options)
    {
    }

    public DbSet<Car> Cars { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<CustomerCar> CustomerCars { get; set; } = null!;
    public DbSet<Loan> Loans { get; set; } = null!;
    public DbSet<LoanStatus> LoanStatuses { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Warranty> Warranties { get; set; } = null!;
    public DbSet<WarrantyType> WarrantyTypes { get; set; } = null!;
        
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    
        optionsBuilder.ConfigureWarnings(warnings => 
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
}