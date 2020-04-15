using DomainModel;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface IEntityPersistence<T> where T: Entity
    {
        T Get(int Id);
        IList<T> Get();
        void Update(T entity);
        void Delete(int Id);
        void Create(T entity);
    }
}
