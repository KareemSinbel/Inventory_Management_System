﻿namespace InventorySystem.Repositories
{
    public interface IGenericRepo<T>
    {
        public T GetAll();
        public T GetById(int id);

        public void Update(T obj);
        public void DeleteById(int id);
    }
}
