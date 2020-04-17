using DataLayer.Interfaces;
using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ProductRepository<Product> : IRepository<Product>
        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public Product Get(int Id)
        {
            throw new NotImplementedException();
        }

        public IList<Product> Get()
        {
            throw new NotImplementedException();
        }

        public void Insert(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
