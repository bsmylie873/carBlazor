using System.ComponentModel.DataAnnotations.Schema;

namespace CarBlazor.DAL.Models;

[Table("warranty_types")]
public class WarrantyType
{
    [Column("id")]
    public int Id { get; set; }
    [Column("name")]
    public string Name { get; set; } = string.Empty;
}