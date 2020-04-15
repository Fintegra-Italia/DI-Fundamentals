using DataLayer.Interfaces;
using DomainModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataLayer
{
    public class ProductReader : IProductReader
    {
        IProductRowParser productRowParser;
        IFlatFileReader fileReader;
        public ProductReader(IProductRowParser productRowParser, IFlatFileReader fileReader)
        {
            this.productRowParser = productRowParser ?? throw new ArgumentNullException("Product Row Parser");
            this.fileReader = fileReader ?? throw new ArgumentNullException("File Reader");
        }
        public IList<Product> Get()
        {
            IList<string> fileRows = fileReader.Read();
            IList<Product> productList = fileRows.Select(row => productRowParser.Parse(row)).ToList();
            return productList;
        }
        public Product Get(int Id)
        {
            IList<string> fileRows = fileReader.Read();
            Product product = fileRows.Where(row => {
                Product prod = productRowParser.Parse(row);
                return prod.Id == Id;
                }).Select(row=> productRowParser.Parse(row)).Single();

            return product;
        }
        //public List<Product> Read(string filename)
        //{
        //    if (String.IsNullOrEmpty(filename) || String.IsNullOrWhiteSpace(filename))
        //        throw new ArgumentNullException("filename");

        //    List<Product> productList = new List<Product>();
        //    using (StreamReader reader = new StreamReader(filename))
        //    {
        //        while (!reader.EndOfStream)
        //        {
        //            productList.Add(productRowParser.Parse(reader.ReadLine()));
        //        }
        //    }
        //    return productList;
        //}
        //private Product parse(string stringToParse)
        //{
        //    string[] splitted = stringToParse.Split('|');
        //    return new Product()
        //    {
        //        Id = int.Parse(splitted[0]),
        //        Nome = splitted[1],
        //        Descrizione = splitted[2],
        //        Immagine = splitted[3],
        //        Prezzo = decimal.Parse(splitted[4]),
        //        Attivo = bool.Parse(splitted[5])
        //    };
        //}
    }
}
