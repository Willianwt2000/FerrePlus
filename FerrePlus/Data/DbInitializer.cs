using FerrePlus.Models;

namespace FerrePlus.Data;

public static class DbInitializer
{
    public static void Seed(FerrePlusDbContext context)
    {
        // Agregar empleados si no existen
        if (!context.Employees.Any())
        {
            context.Employees.AddRange(
                new Employee { Name = "John Smith", Role = "Manager" },
                new Employee { Name = "Lisa Johnson", Role = "Sales Clerk" }
            );
        }

        // Agregar productos si no existen
        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product { Name = "Hammer", UnitPrice = 250, Stock = 15 },
                new Product { Name = "Screwdriver", UnitPrice = 150, Stock = 20 },
                new Product { Name = "Wrench", UnitPrice = 300, Stock = 10 }
            );
        }

        context.SaveChanges();
    }
}