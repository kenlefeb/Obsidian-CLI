using System.Collections.Generic;

namespace Obsidian.Domain.Abstractions
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(string id);
        void Add(T entity);
        void Update(T entity);
        void Delete(string id);
    }
}