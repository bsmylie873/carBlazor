using CarBlazor.DAL.Models;
using CarBlazor.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarBlazor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarrantiesController : ControllerBase
{
    private readonly WarrantyService _warrantyService;

    public WarrantiesController(WarrantyService warrantyService)
    {
        _warrantyService = warrantyService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Warranty>>> GetWarranties()
    {
        return await _warrantyService.GetWarrantiesAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Warranty>> GetWarranty(int id)
    {
        var warranty = await _warrantyService.GetWarrantyByIdAsync(id);
        
        if (warranty == null)
        {
            return NotFound();
        }
        
        return warranty;
    }

    [HttpGet("car/{carId}")]
    public async Task<ActionResult<List<Warranty>>> GetWarrantiesForCar(int carId)
    {
        return await _warrantyService.GetWarrantiesForCarAsync(carId);
    }

    [HttpGet("types")]
    public async Task<ActionResult<List<WarrantyType>>> GetWarrantyTypes()
    {
        return await _warrantyService.GetWarrantyTypesAsync();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<Warranty>> CreateWarranty(Warranty warranty)
    {
        await _warrantyService.CreateWarrantyAsync(warranty);
        return CreatedAtAction(nameof(GetWarranty), new { id = warranty.Id }, warranty);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> UpdateWarranty(int id, Warranty warranty)
    {
        if (id != warranty.Id)
        {
            return BadRequest();
        }

        var updatedWarranty = await _warrantyService.UpdateWarrantyAsync(warranty);
        
        if (updatedWarranty == null)
        {
            return NotFound();
        }
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> DeleteWarranty(int id)
    {
        var result = await _warrantyService.DeleteWarrantyAsync(id);
        
        if (!result)
        {
            return NotFound();
        }
        
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<Warranty>>> SearchWarranties([FromQuery] string searchTerm)
    {
        return await _warrantyService.SearchWarrantiesAsync(searchTerm);
    }
}