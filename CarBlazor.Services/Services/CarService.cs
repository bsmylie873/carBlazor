using CarBlazor.DAL.Data;
using CarBlazor.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CarBlazor.Services.Services;

public class CarService
{
    private readonly CarBlazorContext _context;

    public CarService(CarBlazorContext context)
    {
        _context = context;
    }
    
    public async Task<List<Car>> GetCarsAsync()
    {
        return await _context.Cars.ToListAsync();
    }
    
    public async Task<Car?> GetCarByIdAsync(int id)
    {
        return await _context.Cars.FindAsync(id);
    }
    
    public async Task<List<Car>> GetCarsNotAssociatedWithCustomerAsync(List<int> customerCarIds)
    {
        return await _context.Cars
            .Where(c => !customerCarIds.Contains(c.Id))
            .ToListAsync();
    }
    
    public async Task CreateCarAsync(Car car)
    {
        if (car.ProductionDate != null) car.ProductionDate = car.ProductionDate.Value.ToUniversalTime();
        _context.Cars.Add(car);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Car?> UpdateCarAsync(Car car)
    {
        var existingCar = await _context.Cars.FindAsync(car.Id);
        if (existingCar == null) return null;

        if (car.ProductionDate != null) car.ProductionDate = car.ProductionDate.Value.ToUniversalTime();
        _context.Entry(existingCar).CurrentValues.SetValues(car);
        await _context.SaveChangesAsync();
        return car;
    }
    
    public async Task<bool> DeleteCarAsync(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car == null) return false;

        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<List<Car>> SearchCarsAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetCarsAsync();

        return await _context.Cars
            .Where(c => 
                c.Make.Contains(searchTerm) || 
                c.Model.Contains(searchTerm) || 
                c.Color.Contains(searchTerm) ||
                c.Year.ToString().Contains(searchTerm))
            .ToListAsync();
    }
}