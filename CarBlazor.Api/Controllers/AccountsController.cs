using CarBlazor.Api.ViewModels;
using CarBlazor.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarBlazor.Api.Controllers;

[Route("api/admin/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly AuthService _authService;

    public AccountsController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        var (success, passwordResetRequired, userId) =
            await _authService.LoginAsync(model.Username, model.Password, model.RememberMe);

        if (!success)
            return Unauthorized(new { message = "Invalid username or password" });

        return Ok(new
        {
            success = true,
            passwordResetRequired,
            userId
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return Ok(new { success = true });
    }

    [HttpPost("{userId}/change-password")]
    public async Task<IActionResult> ChangePassword([FromRoute] int userId, [FromBody] ChangePasswordModel model)
    {
        var (success, message) =
            await _authService.ChangePasswordAsync(userId, model.CurrentPassword, model.NewPassword);

        if (!success)
        {
            return BadRequest(new { success = false, message });
        }

        return Ok(new { success = true, message });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserViewModel model)
    {
        var result = await _authService.RegisterUserAsync(model.Username, model.Password, model.RoleId);

        if (!result)
        {
            return BadRequest(new { success = false, message = "Username already exists" });
        }

        return Ok(new { success = true, message = "User registered successfully" });
    }
}