using System.ComponentModel.DataAnnotations.Schema;

namespace CarBlazor.DAL.Models;

[Table("loans")]
public class Loan
{
    [Column("id")]
    public int Id { get; set; }
    [Column("car_id")]
    public int CarId { get; set; }
    [Column("customer_id")]
    public int CustomerId { get; set; }
    [Column("amount")]
    public decimal Amount { get; set; }
    [Column("interest_rate")]
    public decimal InterestRate { get; set; }
    [Column("start_date")]
    public DateTime StartDate { get; set; }
    [Column("end_date")]
    public DateTime EndDate { get; set; }
    [Column("monthly_payment")]
    public decimal MonthlyPayment { get; set; }
    [Column("remaining_balance")]
    public decimal RemainingBalance { get; set; }
    [Column("status_id")]
    public int StatusId { get; set; }
    
    public Car? Car { get; set; }
    public Customer? Customer { get; set; }
    public LoanStatus? Status { get; set; }
}