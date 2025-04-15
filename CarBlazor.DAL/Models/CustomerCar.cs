namespace CarBlazor.DAL.Models;
    
    public class CustomerCar
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int CarId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool IsPrimaryOwner { get; set; }
        
        // Navigation properties
        public Customer? Customer { get; set; }
        public Car? Car { get; set; }
    }