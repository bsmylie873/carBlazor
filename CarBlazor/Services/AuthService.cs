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

    public AuthService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> LoginAsync(string username, string password, bool rememberMe)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null) return false;

        // Verify password hash
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