using DataLayer.Interfaces;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ProductEntitySerializer : IEntitySerializer<Product>
    {
        private char separator;
        public ProductEntitySerializer(char separator)
        {
            this.separator = separator;
        }
        public string Serialize(Product objInstance)
        {
            if (objInstance == null) throw new ArgumentNullException("objInstance");
            var oi = objInstance;
            var s = separator;
            return $"{oi.Id}{s}{oi.Nome}{s}{oi.Descrizione}{s}{oi.Immagine}{s}{oi.Prezzo}{s}{oi.Attivo}";
        }
    }
}
