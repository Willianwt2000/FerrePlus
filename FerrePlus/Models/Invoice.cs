namespace FerrePlus.Models;

public class Invoice
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public bool IsCanceled { get; set; } = false;
    public List<InvoiceDetail> Details { get; set; } = new();
}