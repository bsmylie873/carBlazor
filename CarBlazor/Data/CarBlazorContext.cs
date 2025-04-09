using Microsoft.EntityFrameworkCore;
using CarBlazor.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CarBlazor.Data
{
    public class CarBlazorContext : DbContext
    {
        public CarBlazorContext(DbContextOptions<CarBlazorContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Car { get; set; } = null!;
        public DbSet<Customer> Customer { get; set; } = null!;
        public DbSet<CustomerCar> CustomerCar { get; set; } = null!;
        public DbSet<Loan> Loan { get; set; } = null!;
        public DbSet<LoanStatus> LoanStatus { get; set; } = null!;
        
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Warranty> Warranty { get; set; } = null!;
        public DbSet<WarrantyType> WarrantyType { get; set; } = null!;
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
    
            optionsBuilder.ConfigureWarnings(warnings => 
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<LoanStatus>().HasData(
                new LoanStatus { Id = 1, Name = "Active" },
                new LoanStatus { Id = 2, Name = "Paid" },
                new LoanStatus { Id = 3, Name = "Defaulted" },
                new LoanStatus { Id = 4, Name = "Refinanced" }
            );  
            
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = "Admin", Name = "Administrator", Description = "Full access to all features" },
                new Role { Id = "User", Name = "Standard User", Description = "Limited access to features" },
                new Role { Id = "Guest", Name = "Guest User", Description = "Read-only access to features" }
            );
            
            var (hash, salt) = Utilities.Authentication.HashPassword("password");
            
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 1, 
                    Username = "admin", 
                    PasswordHash = hash, 
                    Salt = salt, 
                    RoleId = "Admin",
                    ForcePasswordReset = false,
                    CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
            
            modelBuilder.Entity<WarrantyType>().HasData(
                new WarrantyType { Id = 1, Name = "Complete" },
                new WarrantyType { Id = 2, Name = "Partial" },
                new WarrantyType { Id = 3, Name = "Minimum" }
            );
        }
    }
}
