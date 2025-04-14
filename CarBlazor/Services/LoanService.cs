using CarBlazor.Data;
using CarBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace CarBlazor.Services;

public class LoanService
{
    private readonly CarBlazorContext _context;

    public LoanService(CarBlazorContext context)
    {
        _context = context;
    }
    
    public async Task<List<Loan>> GetLoansAsync()
    {
        return await _context.Loan
            .Include(l => l.Car)
            .Include(l => l.Customer)
            .Include(l => l.Status)
            .ToListAsync();
    }
    
    public async Task<Loan?> GetLoanByIdAsync(int id)
    {
        return await _context.Loan
            .Include(l => l.Car)
            .Include(l => l.Customer)
            .Include(l => l.Status)
            .FirstOrDefaultAsync(l => l.Id == id);
    }
    
    public async Task CreateLoanAsync(Loan loan)
    {
        _context.Loan.Add(loan);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Loan?> UpdateLoanAsync(Loan loan)
    {
        var existingLoan = await _context.Loan.FindAsync(loan.Id);
        if (existingLoan == null) return null;
        
        _context.Entry(existingLoan).CurrentValues.SetValues(loan);
        await _context.SaveChangesAsync();
        return loan;
    }
    
    public async Task<bool> DeleteLoanAsync(int id)
    {
        var loan = await _context.Loan.FindAsync(id);
        if (loan == null) return false;
        
        _context.Loan.Remove(loan);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<List<Loan>> SearchLoansAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await GetLoansAsync();
        }

        var normalizedSearchTerm = searchTerm
            .Replace("$", "")
            .Replace(",", "")
            .Replace("%", "")
            .Trim();

        return await _context.Loan
            .Where(l =>
                l.Status != null && l.Status.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                l.StartDate.ToShortDateString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                l.EndDate.ToShortDateString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                l.Amount.ToString("C").Replace(",", "").Contains(normalizedSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                l.InterestRate.ToString("P2").Replace("%", "").Contains(normalizedSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                l.MonthlyPayment.ToString("C").Replace(",", "").Contains(normalizedSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                l.RemainingBalance.ToString("C").Replace(",", "").Contains(normalizedSearchTerm, StringComparison.OrdinalIgnoreCase))
            .ToListAsync();
    }
    
    public async Task<List<LoanStatus>> GetLoanStatusesAsync()
    {
        return await _context.LoanStatus.ToListAsync();
    }
}