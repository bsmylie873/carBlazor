using CarBlazor.DAL.Models;
using CarBlazor.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarBlazor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomersController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Customer>>> GetAll()
    {
        return await _customerService.GetCustomersAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetById(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null) return NotFound();
        return customer;
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<Customer>>> Search([FromQuery] string term)
    {
        return await _customerService.SearchCustomersAsync(term);
    }

    [HttpGet("{customerId}/cars")]
    public async Task<ActionResult<List<CustomerCar>>> GetCustomerCars(int customerId)
    {
        return await _customerService.GetCustomerCarsAsync(customerId);
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> Create(Customer customer)
    {
        await _customerService.CreateCustomerAsync(customer);
        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
    }

    [HttpPost("{customerId}/cars")]
    public async Task<ActionResult> AddCarToCustomer(CustomerCar customerCar)
    {
        await _customerService.AddCarToCustomerAsync(customerCar);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, Customer customer)
    {
        if (id != customer.Id) return BadRequest();
        
        var result = await _customerService.UpdateCustomerAsync(customer);
        if (result == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _customerService.DeleteCustomerAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("cars/{customerCarId}")]
    public async Task<ActionResult> RemoveCarFromCustomer(int customerCarId)
    {
        var result = await _customerService.RemoveCarFromCustomerAsync(customerCarId);
        if (!result) return NotFound();
        return NoContent();
    }
}