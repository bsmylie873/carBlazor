using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CarBlazor.DAL.Models;

[Table("roles")]
public class Role
{
    [Column("id")]
    public string Id { get; set; } = string.Empty;
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    [Column("description")]
    public string Description { get; set; } = string.Empty;
    
    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}