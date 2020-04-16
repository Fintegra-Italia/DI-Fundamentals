using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Interfaces
{
    public interface IProductRepository
    {
        IList<Product> Get();
        Product Get(int Id);
        void Insert(Product product);
        void Delete(int Id);
        void Update(Product product);
    }
}
