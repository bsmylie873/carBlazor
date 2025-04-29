using CarBlazor.DAL.Models;
using CarBlazor.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarBlazor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly LoanService _loanService;

    public LoansController(LoanService loanService)
    {
        _loanService = loanService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Loan>>> GetAll()
    {
        return await _loanService.GetLoansAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Loan>> GetById(int id)
    {
        var loan = await _loanService.GetLoanByIdAsync(id);
        if (loan == null) return NotFound();
        return loan;
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<Loan>>> Search([FromQuery] string term)
    {
        return await _loanService.SearchLoansAsync(term);
    }

    [HttpGet("statuses")]
    public async Task<ActionResult<List<LoanStatus>>> GetLoanStatuses()
    {
        return await _loanService.GetLoanStatusesAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Loan>> Create(Loan loan)
    {
        await _loanService.CreateLoanAsync(loan);
        return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, Loan loan)
    {
        if (id != loan.Id) return BadRequest();
        
        var result = await _loanService.UpdateLoanAsync(loan);
        if (result == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _loanService.DeleteLoanAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}