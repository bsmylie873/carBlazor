using CarBlazor.DAL.Data;
using CarBlazor.DAL.Models;
using CarBlazor.DAL.Utilities;
using Microsoft.EntityFrameworkCore;

namespace CarBlazor.Services.Services;

public class UserService
{
    private readonly CarBlazorContext _context;

    public UserService(CarBlazorContext context)
    {
        _context = context;
    }
    
    public async Task<List<User>> GetUsersAsync()
    {
        return await _context.Users
            .Include(u => u.Role)
            .ToListAsync();
    }
    
    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    
    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }
    
    public async Task<User> CreateUserAsync(string username, string password, string roleId)
    {
        var (hash, salt) = Authentication.HashPassword(password);

        var user = new User
        {
            Username = username,
            RoleId = roleId,
            PasswordHash = hash,
            Salt = salt,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
    
    public async Task<User?> UpdateUserAsync(User user)
    {
        var existingUser = await _context.Users.FindAsync(user.Id);
        if (existingUser == null) return null;

        existingUser.Username = user.Username;
        existingUser.RoleId = user.RoleId;
        
        await _context.SaveChangesAsync();
        return existingUser;
    }
    
    public async Task ResetPasswordAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.ForcePasswordReset = true;
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<List<User>> SearchUsersAsync(string searchTerm)
    {
        var users = await _context.Users
            .Include(u => u.Role)
            .ToListAsync();
        
        return users.Where(u => 
                u.Username.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                u.Role.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                u.CreatedAt.ToString("g").Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}