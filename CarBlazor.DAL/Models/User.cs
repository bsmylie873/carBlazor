using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CarBlazor.DAL.Models;

[Table("users")]
public class User
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("username")]    
    [Required]
    public string Username { get; set; } = string.Empty;
        
    [Column("password_hash")]
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
        
    [Column("salt")]
    public string Salt { get; set; } = string.Empty;
       
    [Column("role_id")]
    [Required]
    public string RoleId { get; set; } = "User";
    
    [Column("force_password_reset")]
    public bool ForcePasswordReset { get; set; }
        
    [ForeignKey("RoleId")]
    public virtual Role Role { get; set; } = null!;
        
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}