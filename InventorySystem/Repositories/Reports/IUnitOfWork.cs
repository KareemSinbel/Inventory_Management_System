using InventorySystem.Models;
using System.Threading.Tasks;

namespace InventorySystem.Repositories
{
    public interface IUnitOfWork
    {
        IReportRepo<AlertReport> AlertLevelReports { get; }
        IReportRepo<Product> Products { get; }
        IReportRepo<Supplier> Suppliers { get; }
        Task CompleteAsync();
    }
}
