using DataLayer.Interfaces;
using DomainModel;
using System;


namespace DataLayer
{
    public class ProductSerializer : IProductSerializer
    {
        private char separator;
        public ProductSerializer(char separator)
        {
            this.separator = separator;
        }
        public string Serialize(Product product)
        {
            if (product == null) throw new ArgumentNullException("objInstance");
            var oi = product;
            var s = separator;
            return $"{oi.Id}{s}{oi.Nome}{s}{oi.Descrizione}{s}{oi.Immagine}{s}{oi.Prezzo}{s}{oi.Attivo}";
        }
    }
}
