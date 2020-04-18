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
    public class ProductRepository : FileRepository<Product>, IRepository<Product>
    {
        public ProductRepository(string filePath) : base(filePath) {}
    }
}
