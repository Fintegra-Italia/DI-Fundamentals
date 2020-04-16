using DataLayer.Interfaces;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ProductParser : IProductParser
    {
        public Product Parse(string row)
        {
            string[] splitted = row.Split('|');
            return new Product()
            {
                Id = int.Parse(splitted[0]),
                Nome = splitted[1],
                Descrizione = splitted[2],
                Immagine = splitted[3],
                Prezzo = decimal.Parse(splitted[4]),
                Attivo = bool.Parse(splitted[5])
            };
        }
    }
}
