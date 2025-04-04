namespace CarBlazor.Models;

public class Car
{
    public int Id { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public int Year { get; set; }
    public string? Color { get; set; }
    public DateTime? ProductionDate { get; set; }
}