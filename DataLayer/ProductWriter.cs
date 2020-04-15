using DataLayer.Interfaces;
using DomainModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ProductWriter : IProductWriter
    {
        private IProductSerializer serializer;
        private IProductRowParser parser;
        private string filePath;
        public ProductWriter(string filePath, IProductSerializer serializer, IProductRowParser parser)
        {
            this.filePath = filePath ?? throw new ArgumentNullException("file path");
            this.serializer = serializer ?? throw new ArgumentNullException("Product Serializer");
            this.parser = parser ?? throw new ArgumentNullException("Product Row Parser");
        }
        //public void Append(string filename, string riga)
        //{
        //    if (String.IsNullOrEmpty(filename) || String.IsNullOrWhiteSpace(filename))
        //        throw new ArgumentNullException("filename");

        //    if (String.IsNullOrEmpty(riga) || String.IsNullOrWhiteSpace(riga))
        //        throw new ArgumentNullException("contenuto riga");
        //    if (!File.Exists(filename))
        //    {
        //        using (StreamWriter sw = File.CreateText(filename))
        //        {
        //            sw.WriteLine(riga);
        //        }
        //    }
        //    else
        //    {
        //        using (StreamWriter sw = File.AppendText(filename))
        //        {
        //            sw.WriteLine(riga);
        //        }
        //    }
        //}

        public void Append(Product product)
        {
            if(product == null) throw new ArgumentNullException("product");
            string row = serializer.Serialize(product);
            if (!File.Exists(filePath))
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(row);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(row);
                }
            }
        }

        public void Delete(int Id)
        {
            using(StreamReader reader = new StreamReader(filePath))
            
        }

        public void Update(Product product)
        {
            throw new NotImplementedException();
        }

        //public void Reset(string filename)
        //{
        //    if (String.IsNullOrEmpty(filename) || String.IsNullOrWhiteSpace(filename))
        //        throw new ArgumentNullException("filename");
        //    File.Delete(filename);
        //}
    }
}
