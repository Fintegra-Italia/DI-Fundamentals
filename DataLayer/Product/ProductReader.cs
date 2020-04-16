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
    }
}
