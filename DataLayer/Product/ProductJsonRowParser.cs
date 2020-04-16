using DataLayer.Interfaces;
using DomainModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ProductJsonRowParser : IProductRowParser
    {
        public Product Parse(string row)
        {
            if (string.IsNullOrEmpty(row) || string.IsNullOrWhiteSpace(row)) throw new ArgumentNullException("row");
            Product product = null;
            try
            {
                product = JsonConvert.DeserializeObject<Product>(row);
            }catch(Exception exception)
            {
                throw new Exception($"Errore conversione stringa in entità prodotto: {exception.Message}", exception.InnerException);
            }
            return product;
        }
    }
}
