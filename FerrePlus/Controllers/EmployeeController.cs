using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FerrePlus.Data;
using FerrePlus.Models;
using Microsoft.AspNetCore.Authorization;

namespace FerrePlus.Controllers;

[ApiController]
[Route("api/[controller]")]

public class EmployeeController : ControllerBase
{
  private readonly FerrePlusDbContext _context;

  public EmployeeController(FerrePlusDbContext context)
  {
    _context = context;
  }

  // GET: api/employee
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
  {
    return await _context.Employees.ToListAsync();
  }

  // GET: api/employee/5
  [HttpGet("{id}")]
  public async Task<ActionResult<Employee>> GetEmployee(int id)
  {
    var employee = await _context.Employees.FindAsync(id);

    if (employee == null)
    {
      return NotFound();
    }

    return employee;
  }

  // POST: api/employee
  [HttpPost]
  public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
  {
    _context.Employees.Add(employee);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
  }

  // PUT: api/employee/5
  [HttpPut("{id}")]
  
  public async Task<IActionResult> PutEmployee(int id, Employee employee)
  {
    if (id != employee.Id)
    {
      return BadRequest();
    }

    _context.Entry(employee).State = EntityState.Modified;

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!_context.Employees.Any(e => e.Id == id))
      {
        return NotFound();
      }
      throw;
    }

    return NoContent();
  }


  
  [HttpDelete("{id}")]
  [Authorize]
  public async Task<IActionResult> DeleteEmployee(int id)
  {
    var employee = await _context.Employees.FindAsync(id);
    if (employee == null)
    {
      return NotFound();
    }

    _context.Employees.Remove(employee);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}
