using System.ComponentModel.DataAnnotations.Schema;

namespace CarBlazor.DAL.Models;

[Table("cars")]
public class Car
{
    [Column("id")]
    public int Id { get; set; }
    [Column("make")]
    public string? Make { get; set; }
    [Column("model")]
    public string? Model { get; set; }
    [Column("year")]
    public int Year { get; set; }
    [Column("color")]
    public string? Color { get; set; }
    [Column("production_date")]
    public DateTime? ProductionDate { get; set; }
}