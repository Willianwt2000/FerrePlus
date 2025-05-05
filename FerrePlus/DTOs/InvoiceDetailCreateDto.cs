namespace FerrePlus.DTOs;

public class InvoiceDetailCreateDto
{
    public int InvoiceId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}