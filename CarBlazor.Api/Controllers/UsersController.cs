using CarBlazor.Api.ViewModels;
using CarBlazor.DAL.Models;
using CarBlazor.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarBlazor.Api.Controllers;

[ApiController]
[Route("api/admin/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        return await _userService.GetUsersAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound();
        return user;
    }

    [HttpGet("exists/{username}")]
    public async Task<ActionResult<bool>> UsernameExists(string username)
    {
        return await _userService.UsernameExistsAsync(username);
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<User>>> Search([FromQuery] string term)
    {
        return await _userService.SearchUsersAsync(term);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create([FromBody] CreateUserViewModel request)
    {
        var user = await _userService.CreateUserAsync(request.Username, request.Password, request.RoleId);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<User>> Update(int id, User user)
    {
        if (id != user.Id) return BadRequest();
        
        var updatedUser = await _userService.UpdateUserAsync(user);
        if (updatedUser == null) return NotFound();
        
        return updatedUser;
    }

    [HttpPost("{id}/reset-password")]
    public async Task<ActionResult> ResetPassword(int id)
    {
        await _userService.ResetPasswordAsync(id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}