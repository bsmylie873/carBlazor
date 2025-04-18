using CarBlazor.DAL.Data;
using CarBlazor.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CarBlazor.Services.Services;

public class RoleService
{
    private readonly CarBlazorContext _context;

    public RoleService(CarBlazorContext context)
    {
        _context = context;
    }
    
    public async Task<List<Role>> GetRolesAsync(bool includeAdmin = false)
    {
        var query = _context.Roles.AsQueryable();
        
        if (!includeAdmin)
        {
            query = query.Where(r => r.Id != "Admin");
        }
        
        return await query.ToListAsync();
    }
}