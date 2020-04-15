using DomainModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataLayer
{
    public class ProductReader
    {
        public List<Product> Read(string filename)
        {
            if (String.IsNullOrEmpty(filename) || String.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            List<Product> productList = new List<Product>();
            using (StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    productList.Add(parse(reader.ReadLine()));
                }
            }
            return productList;
        }
        private Product parse(string stringToParse)
        {
            string[] splitted = stringToParse.Split('|');
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
