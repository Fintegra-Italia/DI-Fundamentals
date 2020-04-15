using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface IProductReader
    {
        Product Get(int Id);
        IList<Product> Get();
    }
}
