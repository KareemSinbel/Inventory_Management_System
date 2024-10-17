namespace InventorySystem.Repositories
{
    public interface IFactoryRepository
    {
        IGenericRepo<T>? CreateRepositoryMethod<T>();
    }
}
