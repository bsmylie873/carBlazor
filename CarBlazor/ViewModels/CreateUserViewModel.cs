using System.ComponentModel.DataAnnotations;

namespace CarBlazor.ViewModels;

public class CreateUserViewModel
{
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required")]
    public string RoleId { get; set; } = string.Empty;
}