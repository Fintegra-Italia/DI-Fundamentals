using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface IProductWriter
    {
        void Append(Product product);
        void Delete(int Id);
        void Update(Product product);
    }
}
