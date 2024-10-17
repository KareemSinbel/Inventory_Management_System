namespace InventorySystem.Repositories
{
    public interface IGenericRepo<T>
    {
        public IEnumerable<T>? GetAll();
        public T? GetById(int id);

        public void Update(T obj);
        public void DeleteById(int id);

        public Task AddAsync(T obj);
    }
}
