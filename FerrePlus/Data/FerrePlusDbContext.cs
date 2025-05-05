using Microsoft.EntityFrameworkCore;
using FerrePlus.Models;

namespace FerrePlus.Data;

public class FerrePlusDbContext: DbContext
{
    public FerrePlusDbContext(DbContextOptions<FerrePlusDbContext> options) : base(options) { } //Hint

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceDetail> InvoiceDetails => Set<InvoiceDetail>();
}