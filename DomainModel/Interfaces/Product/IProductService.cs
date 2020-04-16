using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Interfaces
{
    public interface IProductService
    {
        Product Get(int Id);
        IList<Product> GetAll();
        ProductService For(Account user);
        ProductService IfAccountTypeIs(Account.tipo tipo);
        ProductService ApplyDiscount(Func<decimal, decimal> discountFunction);
    }
}
