using System.Security.Claims;
using CarBlazor.Data;
using CarBlazor.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using static CarBlazor.Utilities.Authentication;

namespace CarBlazor.Services;

public class AuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly CarBlazorContext _context;

    public AuthService(IHttpContextAccessor httpContextAccessor, CarBlazorContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    public async Task<bool> LoginAsync(string username, string password, bool rememberMe)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null) return false;
        
        if (!VerifyPassword(password, user.PasswordHash, user.Salt)) 
            return false;

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role.Id),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = rememberMe,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(rememberMe ? 7 : 1)
        };

        await _httpContextAccessor.HttpContext!.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return true;
    }
    public async Task LogoutAsync()
    {
        await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
    
    public async Task<(bool Success, string Message)> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return (false, "User not found.");

            if (!VerifyPassword(currentPassword, user.PasswordHash, user.Salt))
                return (false, "Current password is incorrect.");
            
            (user.PasswordHash, user.Salt) = HashPassword(newPassword);
        
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        
            return (true, "Password changed successfully.");
        }
        catch (Exception ex)
        {
            return (false, $"Error changing password: {ex.Message}");
        }
    }
    
    public async Task<bool> RegisterUserAsync(string username, string password, string roleId)
    {
        if (await _context.Users.AnyAsync(u => u.Username == username))
            return false;

        var (hash, salt) = HashPassword(password);

        var user = new User
        {
            Username = username,
            PasswordHash = hash,
            Salt = salt,
            RoleId = roleId
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }
}