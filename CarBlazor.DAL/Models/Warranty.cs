using System.ComponentModel.DataAnnotations.Schema;

namespace CarBlazor.DAL.Models;

[Table("warranties")]
public class Warranty
{
    [Column("id")]
    public int Id { get; set; }
    [Column("car_id")]
    public int CarId { get; set; }
    [Column("warranty_type_id")]
    public int WarrantyTypeId { get; set; }
    [Column("start_date")]
    public DateTime StartDate { get; set; }
    [Column("end_date")]
    public DateTime EndDate { get; set; }
    [Column("coverage_details")]
    public string? CoverageDetails { get; set; }
    [Column("provider")]
    public string? Provider { get; set; }
    [Column("cost")]
    public decimal Cost { get; set; }

    // Navigation property
    public Car? Car { get; set; }
    public WarrantyType? WarrantyType { get; set; }
}