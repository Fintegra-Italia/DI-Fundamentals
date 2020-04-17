using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Interfaces
{
    public interface IRepository<T> where T : Entity
    {
        T Get(int Id);
        IList<T> Get();
        void Update(T entity);
        void Insert(T entity);
        void Delete(int Id);
    }
}
