using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FerrePlus.DTOs;
using FerrePlus.Data;
using FerrePlus.Models;


namespace FerrePlus.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceDetailController : ControllerBase
{
    private readonly FerrePlusDbContext _context;

    public InvoiceDetailController(FerrePlusDbContext context)
    {
        _context = context;
    }

    // GET: api/invoicedetail/invoice/1
    [HttpGet("invoice/{invoiceId}")]
    public async Task<ActionResult<IEnumerable<InvoiceDetail>>> GetDetailsByInvoice(int invoiceId)
    {
        return await _context.InvoiceDetails
            .Where(d => d.InvoiceId == invoiceId)
            .Include(d => d.Product)
            .ToListAsync();
    }

    // POST: api/invoicedetail
    // [HttpPost]
    // public async Task<ActionResult<InvoiceDetail>> PostDetail(InvoiceDetail detail)
    // {
    //     var product = await _context.Products.FindAsync(detail.ProductId);
    //     if (product == null)
    //         return NotFound("Product not found.");

    //     if (product.Stock < detail.Quantity)
    //         return BadRequest("Insufficient stock.");

    //     detail.UnitPrice = product.UnitPrice;
    //     product.Stock -= detail.Quantity;

    //     _context.InvoiceDetails.Add(detail);
    //     await _context.SaveChangesAsync();

    //     return CreatedAtAction(nameof(GetDetailsByInvoice), new { invoiceId = detail.InvoiceId }, detail);
    // }


    [HttpPost]
public async Task<ActionResult<InvoiceDetail>> PostDetail(InvoiceDetailCreateDto dto)
{
    var product = await _context.Products.FindAsync(dto.ProductId);
    if (product == null)
        return NotFound("Product not found.");

    if (product.Stock < dto.Quantity)
        return BadRequest("Insufficient stock.");

    var detail = new InvoiceDetail
    {
        InvoiceId = dto.InvoiceId,
        ProductId = dto.ProductId,
        Quantity = dto.Quantity,
        UnitPrice = product.UnitPrice
    };

    product.Stock -= dto.Quantity;

    _context.InvoiceDetails.Add(detail);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetDetailsByInvoice), new { invoiceId = dto.InvoiceId }, detail);
}


    // PUT: api/invoicedetail/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDetail(int id, InvoiceDetail updatedDetail)
    {
        var existingDetail = await _context.InvoiceDetails.FindAsync(id);
        if (existingDetail == null)
            return NotFound();

        var product = await _context.Products.FindAsync(existingDetail.ProductId);
        if (product == null)
            return NotFound("Product not found.");

        // Devolver el stock anterior
        product.Stock += existingDetail.Quantity;

        // Verifica si hay suficiente stock para el nuevo valor
        if (product.Stock < updatedDetail.Quantity)
            return BadRequest("Insufficient stock for update.");

        // Resta la nueva cantidad
        product.Stock -= updatedDetail.Quantity;

        existingDetail.Quantity = updatedDetail.Quantity;
        existingDetail.UnitPrice = updatedDetail.UnitPrice;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/invoicedetail/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDetail(int id)
    {
        var detail = await _context.InvoiceDetails.FindAsync(id);
        if (detail == null)
            return NotFound();

        var product = await _context.Products.FindAsync(detail.ProductId);
        if (product != null)
        {
            product.Stock += detail.Quantity;
        }

        _context.InvoiceDetails.Remove(detail);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}