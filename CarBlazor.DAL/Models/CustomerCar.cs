using System.ComponentModel.DataAnnotations.Schema;

namespace CarBlazor.DAL.Models;

[Table("customer_cars")]
public class CustomerCar
{
    [Column("id")]
    public int Id { get; set; }
    [Column("customer_id")]
    public int CustomerId { get; set; }
    [Column("car_id")]
    public int CarId { get; set; }
    [Column("purchase_date")]
    public DateTime PurchaseDate { get; set; }
    [Column("is_primary_owner")]
    public bool IsPrimaryOwner { get; set; }

    // Navigation properties
    public Customer? Customer { get; set; }
    public Car? Car { get; set; }
}