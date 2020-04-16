using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class ProductService : IProductService
    {
        private IProductRepository productRepository;
        private Account user;
        private Account.tipo tipoAccount;
        private Func<decimal, decimal> discount;
        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository ?? throw new ArgumentNullException("Product Respository");
            this.user = null;
            this.tipoAccount = Account.tipo.Normal;
            this.discount = null;
        }
        public ProductService For(Account user)
        {
            this.user = user;
            return this;
        }
        public ProductService IfAccountTypeIs(Account.tipo tipo)
        {
            this.tipoAccount = tipo;
            return this;
        }
        public ProductService ApplyDiscount(Func<decimal, decimal> discountFunction)
        {
            if (discountFunction == null) throw new ArgumentNullException("Discount Function");
            this.discount = discountFunction;
            return this;
        }
        public Product Get(int Id)
        {
            Product product =  productRepository.Get(Id);
            if (user?.Tipo != tipoAccount) return product;
            if (discount == null) return product;
            product.Prezzo = discount(product.Prezzo);
            return product;
        }

        public IList<Product> GetAll()
        {
            IList<Product> productList = productRepository.Get();
            if (user?.Tipo != tipoAccount) return productList;
            if (discount == null) return productList;
            foreach(var product in productList)
            {
                product.Prezzo = discount(product.Prezzo);
            }
            return productList;

        }
    }
}
