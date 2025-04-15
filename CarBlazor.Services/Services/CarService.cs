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
        return await _context.Car.ToListAsync();
    }
    
    public async Task<Car?> GetCarByIdAsync(int id)
    {
        return await _context.Car.FindAsync(id);
    }
    
    public async Task<List<Car>> GetCarsNotAssociatedWithCustomerAsync(List<int> customerCarIds)
    {
        return await _context.Car
            .Where(c => !customerCarIds.Contains(c.Id))
            .ToListAsync();
    }
    
    public async Task CreateCarAsync(Car car)
    {
        _context.Car.Add(car);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Car?> UpdateCarAsync(Car car)
    {
        var existingCar = await _context.Car.FindAsync(car.Id);
        if (existingCar == null) return null;

        _context.Entry(existingCar).CurrentValues.SetValues(car);
        await _context.SaveChangesAsync();
        return car;
    }
    
    public async Task<bool> DeleteCarAsync(int id)
    {
        var car = await _context.Car.FindAsync(id);
        if (car == null) return false;

        _context.Car.Remove(car);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<List<Car>> SearchCarsAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetCarsAsync();

        return await _context.Car
            .Where(c => 
                c.Make.Contains(searchTerm) || 
                c.Model.Contains(searchTerm) || 
                c.Color.Contains(searchTerm) ||
                c.Year.ToString().Contains(searchTerm))
            .ToListAsync();
    }
}