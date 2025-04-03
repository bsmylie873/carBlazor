namespace CarBlazor.Models;

public class Customer
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    // Navigation property
    public ICollection<CustomerCar>? CustomerCars { get; set; }
}