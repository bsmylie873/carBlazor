using System.ComponentModel.DataAnnotations.Schema;

namespace CarBlazor.DAL.Models;

[Table("loan_statuses")]
public class LoanStatus
{
    [Column("id")]
    public int Id { get; set; }
    [Column("name")]
    public string Name { get; set; } = string.Empty;
}