namespace CarBlazor.Models;
    
    public class Warranty
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int WarrantyTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? CoverageDetails { get; set; }
        public string? Provider { get; set; }
        public decimal Cost { get; set; }
        
        // Navigation property
        public Car? Car { get; set; }
        public WarrantyType? WarrantyType { get; set; }
    }