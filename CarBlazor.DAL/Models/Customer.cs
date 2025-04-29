using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CarBlazor.DAL.Models;

[Table("customers")]
public class Customer
{
    [Column("id")]
    public int Id { get; set; }
    [Column("first_name")]
    public string? FirstName { get; set; }
    [Column("last_name")]
    public string? LastName { get; set; }
    [Column("email")]
    public string? Email { get; set; }
    [Column("phone_number")]
    public string? PhoneNumber { get; set; }
    [Column("address")]
    public string? Address { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    [JsonIgnore]
    public ICollection<CustomerCar>? CustomerCars { get; set; }
}