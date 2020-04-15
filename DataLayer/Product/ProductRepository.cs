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
    public class ProductRepository : IProductRepository
    {
        IProductWriter writer;
        IProductReader reader;
        public ProductRepository(IProductWriter writer, IProductReader reader)
        {
            this.writer = writer ?? throw new ArgumentNullException("writer");
            this.reader = reader ?? throw new ArgumentNullException("reader");
        }
        public void Delete(int Id)
        {
            writer.Delete(Id);
        }

        public IList<Product> Get()
        {
            return reader.Get();
        }

        public Product Get(int Id)
        {
            return reader.Get(Id);
        }

        public void Insert(Product product)
        {
            writer.Append(product);
        }

        public void Update(Product product)
        {
            writer.Update(product);
        }
    }
}
