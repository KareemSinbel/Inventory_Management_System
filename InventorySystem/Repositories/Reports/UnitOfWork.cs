using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.Repositories;
using Microsoft.EntityFrameworkCore;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IReportRepo<AlertReport> StockReports { get; private set; }
    public IReportRepo<Product> Products { get; private set; }
    public IReportRepo<Supplier> Suppliers { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        StockReports = new ReportRepo<AlertReport>(context);
        Products = new ReportRepo<Product>(context);
        Suppliers = new ReportRepo<Supplier>(context);
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}
