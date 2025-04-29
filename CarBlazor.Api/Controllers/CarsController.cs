using CarBlazor.DAL.Models;
using CarBlazor.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarBlazor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly ILogger<CarsController> _logger;
    private readonly CarService _carService;

    public CarsController(ILogger<CarsController> logger, CarService carService)
    {
        _logger = logger;
        _carService = carService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Car>>> GetCars()
    {
        return await _carService.GetCarsAsync();
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<Car>>> SearchCars([FromQuery] string searchTerm)
    {
        return await _carService.SearchCarsAsync(searchTerm);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Car>> GetCar(int id)
    {
        var car = await _carService.GetCarByIdAsync(id);
        if (car == null) return NotFound();
        return car;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCar(Car car)
    {
        await _carService.CreateCarAsync(car);
        return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCar(int id, Car car)
    {
        if (id != car.Id) return BadRequest();

        var updatedCar = await _carService.UpdateCarAsync(car);
        if (updatedCar == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCar(int id)
    {
        var result = await _carService.DeleteCarAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}