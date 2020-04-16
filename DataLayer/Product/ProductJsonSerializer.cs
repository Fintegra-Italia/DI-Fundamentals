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
    public class ProductJsonSerializer: IProductSerializer
    {
        public string Serialize(Product product)
        {
            if (product == null) throw new ArgumentNullException("product");
            return JsonConvert.SerializeObject(product);
        }
    }
}
