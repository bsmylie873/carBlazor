using CarBlazor.DAL.Data;
using CarBlazor.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CarBlazor.Services.Services;

public class WarrantyService
{
    private readonly CarBlazorContext _context;

    public WarrantyService(CarBlazorContext context)
    {
        _context = context;
    }
    
    public async Task<List<Warranty>> GetWarrantiesAsync()
    {
        return await _context.Warranty
            .Include(w => w.WarrantyType)
            .Include(w => w.Car)
            .ToListAsync();
    }
    
    public async Task<Warranty?> GetWarrantyByIdAsync(int id)
    {
        return await _context.Warranty
            .Include(w => w.WarrantyType)
            .Include(w => w.Car)
            .FirstOrDefaultAsync(w => w.Id == id);
    }
    
    public async Task<List<Warranty>> GetWarrantiesForCarAsync(int carId)
    {
        return await _context.Warranty
            .Include(w => w.WarrantyType)
            .Where(w => w.CarId == carId)
            .ToListAsync();
    }
    
    public async Task<List<WarrantyType>> GetWarrantyTypesAsync()
    {
        return await _context.WarrantyType.ToListAsync();
    }
    
    public async Task CreateWarrantyAsync(Warranty warranty)
    {
        _context.Warranty.Add(warranty);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Warranty?> UpdateWarrantyAsync(Warranty warranty)
    {
        var existingWarranty = await _context.Warranty.FindAsync(warranty.Id);
        if (existingWarranty == null) return null;

        _context.Entry(existingWarranty).CurrentValues.SetValues(warranty);
        await _context.SaveChangesAsync();
        return warranty;
    }
    
    public async Task<bool> DeleteWarrantyAsync(int id)
    {
        var warranty = await _context.Warranty.FindAsync(id);
        if (warranty == null) return false;

        _context.Warranty.Remove(warranty);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<List<Warranty>> SearchWarrantiesAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetWarrantiesAsync();

        var normalizedSearchTerm = searchTerm
            .Replace("$", "")
            .Replace(",", "")
            .Replace("%", "")
            .Trim();

        return await _context.Warranty
            .Where(w =>
                (w.Car != null && (
                    (w.Car.Make != null && w.Car.Make.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (w.Car.Model != null && w.Car.Model.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    w.Car.Year.ToString().Contains(searchTerm))) ||
                (w.WarrantyType != null && w.WarrantyType.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (w.Provider != null && w.Provider.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                w.StartDate.ToShortDateString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                w.EndDate.ToShortDateString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                w.Cost.ToString("C").Replace(",", "").Contains(normalizedSearchTerm, StringComparison.OrdinalIgnoreCase))
            .ToListAsync();
    }
}