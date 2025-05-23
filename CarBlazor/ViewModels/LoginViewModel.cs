using System.ComponentModel.DataAnnotations;

namespace CarBlazor.ViewModels;

public class LoginViewModel
{
    [Required]
    public string Username { get; set; } = string.Empty;
        
    [Required]
    public string Password { get; set; } = string.Empty;
        
    public bool RememberMe { get; set; }
}