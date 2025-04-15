namespace CarBlazor.DAL.Models;
public class Loan
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    public decimal InterestRate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal MonthlyPayment { get; set; }
    public decimal RemainingBalance { get; set; }
    public int StatusId { get; set; }
    
    // Navigation properties
    public Car? Car { get; set; }
    public Customer? Customer { get; set; }
    public LoanStatus? Status { get; set; }
}