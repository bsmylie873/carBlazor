using CarBlazor.DAL.Data;
using CarBlazor.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CarBlazor.Services.Services;

public class LoanService
{
    private readonly CarBlazorContext _context;

    public LoanService(CarBlazorContext context)
    {
        _context = context;
    }
    
    public async Task<List<Loan>> GetLoansAsync()
    {
        return await _context.Loans
            .Include(l => l.Car)
            .Include(l => l.Customer)
            .Include(l => l.Status)
            .ToListAsync();
    }
    
    public async Task<Loan?> GetLoanByIdAsync(int id)
    {
        return await _context.Loans
            .Include(l => l.Car)
            .Include(l => l.Customer)
            .Include(l => l.Status)
            .FirstOrDefaultAsync(l => l.Id == id);
    }
    
    public async Task CreateLoanAsync(Loan loan)
    {
        loan.StartDate = loan.StartDate.ToUniversalTime();
        loan.EndDate = loan.EndDate.ToUniversalTime();
        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Loan?> UpdateLoanAsync(Loan loan)
    {
        var existingLoan = await _context.Loans.FindAsync(loan.Id);
        if (existingLoan == null) return null;
        
        loan.StartDate = loan.StartDate.ToUniversalTime();
        loan.EndDate = loan.EndDate.ToUniversalTime();
        _context.Entry(existingLoan).CurrentValues.SetValues(loan);
        await _context.SaveChangesAsync();
        return loan;
    }
    
    public async Task<bool> DeleteLoanAsync(int id)
    {
        var loan = await _context.Loans.FindAsync(id);
        if (loan == null) return false;
        
        _context.Loans.Remove(loan);
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

        return await _context.Loans
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
        return await _context.LoanStatuses.ToListAsync();
    }
}