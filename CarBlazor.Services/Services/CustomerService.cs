using CarBlazor.DAL.Data;
using CarBlazor.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CarBlazor.Services.Services;

public class CustomerService
{
    private readonly CarBlazorContext _context;

    public CustomerService(CarBlazorContext context)
    {
        _context = context;
    }
    
    public async Task<List<Customer>> GetCustomersAsync()
    {
        return await _context.Customer
            .Include(c => c.CustomerCars)
            .ToListAsync();
    }
    
    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        return await _context.Customer
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<List<CustomerCar>> GetCustomerCarsAsync(int customerId)
    {
        return await _context.CustomerCar
            .Include(cc => cc.Car)
            .Where(cc => cc.CustomerId == customerId)
            .ToListAsync();
    }
    
    public async Task CreateCustomerAsync(Customer customer)
    {
        _context.Customer.Add(customer);
        await _context.SaveChangesAsync();
    }
    
    public async Task AddCarToCustomerAsync(CustomerCar customerCar)
    {
        _context.CustomerCar.Add(customerCar);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Customer?> UpdateCustomerAsync(Customer customer)
    {
        var existingCustomer = await _context.Customer.FindAsync(customer.Id);
        if (existingCustomer == null) return null;

        _context.Entry(existingCustomer).CurrentValues.SetValues(customer);
        await _context.SaveChangesAsync();
        return customer;
    }
    
    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customer = await _context.Customer.FindAsync(id);
        if (customer == null) return false;

        _context.Customer.Remove(customer);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> RemoveCarFromCustomerAsync(int customerCarId)
    {
        var customerCar = await _context.CustomerCar.FindAsync(customerCarId);
        if (customerCar == null) return false;

        _context.CustomerCar.Remove(customerCar);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<List<Customer>> SearchCustomersAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetCustomersAsync();

        return await _context.Customer
            .Include(c => c.CustomerCars)
            .Where(c => 
                c.FirstName.Contains(searchTerm) ||
                c.LastName.Contains(searchTerm) ||
                c.Email.Contains(searchTerm) ||
                c.PhoneNumber.Contains(searchTerm) ||
                c.Address.Contains(searchTerm))
            .ToListAsync();
    }
}