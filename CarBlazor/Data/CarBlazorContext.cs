using Microsoft.EntityFrameworkCore;
using CarBlazor.Models;

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
        public DbSet<Warranty> Warranty { get; set; } = null!;
        public DbSet<WarrantyType> WarrantyType { get; set; } = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<LoanStatus>().HasData(
                new LoanStatus { Id = 1, Name = "Active" },
                new LoanStatus { Id = 2, Name = "Paid" },
                new LoanStatus { Id = 3, Name = "Defaulted" },
                new LoanStatus { Id = 4, Name = "Refinanced" }
            );  
            
            modelBuilder.Entity<WarrantyType>().HasData(
                new WarrantyType { Id = 1, Name = "Complete" },
                new WarrantyType { Id = 2, Name = "Partial" },
                new WarrantyType { Id = 3, Name = "Minimum" }
            );
        }
    }
}
