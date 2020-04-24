using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.EqualityComparer
{
    public class ProductEqualityComparer : IEqualityComparer<Product>
    {
        public bool Equals(Product x, Product y)
        {
            return x.Id == y.Id &&
                    x.Nome == y.Nome &&
                    x.Descrizione == y.Descrizione &&
                    x.Immagine == y.Immagine &&
                    x.Prezzo == y.Prezzo &&
                    x.Attivo == y.Attivo;
        }

        public int GetHashCode(Product obj)
        {
            int hash = 13;
            hash = (hash * 7) + obj.Id.GetHashCode();
            hash = (hash * 7) + obj.Nome.GetHashCode();
            hash = (hash * 7) + obj.Descrizione.GetHashCode();
            hash = (hash * 7) + obj.Immagine.GetHashCode();
            hash = (hash * 7) + obj.Prezzo.GetHashCode();
            hash = (hash * 7) + obj.Attivo.GetHashCode();
            return hash;
        }
    }
}
