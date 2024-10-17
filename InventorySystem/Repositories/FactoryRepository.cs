using InventorySystem.Models;
using InventorySystem.Repositories;


namespace InventorySystem.Repositories
{
    public class FactoryRepository : IFactoryRepository
    {
        private readonly IServiceProvider _serviceProvider;

        public FactoryRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IGenericRepo<T>? CreateRepositoryMethod<T>()
        {
            if (typeof(T) == typeof(Supplier))
            { 
                return (IGenericRepo<T>?)_serviceProvider.GetRequiredService<IGenericRepo<Supplier>>();
            }
            else if (typeof(T) == typeof(Product))
            {
                return (IGenericRepo<T>?)_serviceProvider.GetRequiredService<IGenericRepo<Product>>();
            }
            else if (typeof(T) == typeof(Category))
            {
                return (IGenericRepo<T>?)_serviceProvider.GetRequiredService<IGenericRepo<Category>>();
            }
            else if (typeof(T) == typeof(Employee))
            {
                return (IGenericRepo<T>?)_serviceProvider.GetRequiredService<IGenericRepo<Employee>>();
            }
            return null;
        }
    }
}
