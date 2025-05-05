using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FerrePlus.Data;
using FerrePlus.Models;
using FerrePlus.DTOs;

namespace FerrePlus.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly FerrePlusDbContext _context;

    public InvoiceController(FerrePlusDbContext context)
    {
        _context = context;
    }

    // GET: api/invoice
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
    {
        return await _context.Invoices
            .Where(i => !i.IsCanceled)
            .Include(i => i.Employee)
            .Include(i => i.Details)
            .ThenInclude(d => d.Product)
            .ToListAsync();
    }

    // GET: api/invoice/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Invoice>> GetInvoice(int id)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Employee)
            .Include(i => i.Details)
            .ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
        {
            return NotFound();
        }

        return invoice;
    }

    // POST: api/invoice
    // [HttpPost]
    // public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
    // {
    //     _context.Invoices.Add(invoice);
    //     await _context.SaveChangesAsync();

    //     return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, invoice);
    // }



[HttpPost]
public async Task<ActionResult<Invoice>> PostInvoice(InvoiceCreateDto dto)
{
    var invoice = new Invoice
    {
        EmployeeId = dto.EmployeeId,
        Date = DateTime.Now
    };

    _context.Invoices.Add(invoice);
    await _context.SaveChangesAsync();

    // Si quieres devolver la factura con el empleado cargado:
    await _context.Entry(invoice).Reference(i => i.Employee).LoadAsync();

    return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, invoice);
}

    // PUT: api/invoice/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutInvoice(int id, Invoice invoice)
    {
        if (id != invoice.Id)
        {
            return BadRequest();
        }

        _context.Entry(invoice).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Invoices.Any(i => i.Id == id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/invoice/5 (soft delete)
    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelInvoice(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        invoice.IsCanceled = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
