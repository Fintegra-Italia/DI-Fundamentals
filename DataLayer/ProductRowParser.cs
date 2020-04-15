using DataLayer.Interfaces;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ProductRowParser : IRowParser<Product>
    {
        private char separator;
        public ProductRowParser(char separator)
        {
            this.separator = separator;
        }
        public Product Parse(string row)
        {
            if (string.IsNullOrEmpty(row) || string.IsNullOrWhiteSpace(row)) throw new ArgumentNullException("row");
            string[] splitted = row.Split(separator);
            if (splitted.Length != 6) throw new FormatException("invalid format: row");
            int Id = -1;
            decimal Prezzo = 0M;
            bool Attivo = false;

            if (!int.TryParse(splitted[0], out Id)) throw new FormatException("invalid format: Id");
            if (string.IsNullOrEmpty(splitted[1]) || string.IsNullOrWhiteSpace(splitted[1]))throw new FormatException("invalid format: Nome");
            if (string.IsNullOrEmpty(splitted[2]) || string.IsNullOrWhiteSpace(splitted[2])) throw new FormatException("invalid format: Descrizione");
            if (string.IsNullOrEmpty(splitted[3]) || string.IsNullOrWhiteSpace(splitted[3])) throw new FormatException("invalid format: Immagine");
            if (!decimal.TryParse(splitted[4], out Prezzo)) throw new FormatException("invalid format: Prezzo");
            if(!bool.TryParse(splitted[5], out Attivo)) throw new FormatException("invalid format: Attivo");

            return new Product() {
                Id = Id,
                Nome = splitted[1],
                Descrizione = splitted[2],
                Immagine = splitted[3],
                Prezzo = Prezzo,
                Attivo = Attivo
            };
        }
    }
}