using System.Collections.Generic;
using DomainModel;

namespace DataLayer
{
    public interface IProductReader
    {
        List<Product> Read(string filename);
    }
}