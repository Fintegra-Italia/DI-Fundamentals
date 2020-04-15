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
        private string fileTemp;
        public ProductWriter(string filePath, IProductSerializer serializer, IProductRowParser parser)
        {
            this.filePath = filePath ?? throw new ArgumentNullException("file path");
            this.serializer = serializer ?? throw new ArgumentNullException("Product Serializer");
            this.parser = parser ?? throw new ArgumentNullException("Product Row Parser");
            this.fileTemp = TempFileSetup(filePath);
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
            if (!File.Exists(filePath)) throw new FileLoadException("file doesn't exist");
            using (StreamReader reader = new StreamReader(filePath))
            using (StreamWriter writer = File.CreateText(fileTemp))
            {
                while (!reader.EndOfStream)
                {
                    Product product = parser.Parse(reader.ReadLine());
                    if (product.Id == Id) continue;
                    writer.WriteLine(serializer.Serialize(product));
                }
            }
            SwapAndErase();

        }

        public void Update(Product product)
        {
            if (product == null) throw new ArgumentNullException("product");
            if (!File.Exists(filePath)) throw new FileLoadException("file doesn't exist");
            using (StreamReader reader = new StreamReader(filePath))
            using (StreamWriter writer = File.CreateText(fileTemp))
            {
                while (!reader.EndOfStream)
                {

                    Product productReaded = parser.Parse(reader.ReadLine());
                    if (productReaded.Id == product.Id)
                    {
                        writer.WriteLine(serializer.Serialize(product));
                    }
                    else
                    {
                        writer.WriteLine(serializer.Serialize(productReaded));
                    }
                }
            }
            SwapAndErase();
        }

        //public void Reset(string filename)
        //{
        //    if (String.IsNullOrEmpty(filename) || String.IsNullOrWhiteSpace(filename))
        //        throw new ArgumentNullException("filename");
        //    File.Delete(filename);
        //}

        private void SwapAndErase()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                File.Copy(fileTemp, filePath);
            }
            else
            {
                File.Copy(fileTemp, filePath);
            }
        }
        private string TempFileSetup(string filePath)
        {
            string tempFile;
            string filename = filePath.Split('/').Last();
            string fileDir = filePath.Replace(filename, "");
            string file = filename.Split('.').First();
            string ext = filename.Split('.').Last();
            tempFile = $"{file}_temp.{ext}";
            return AppDomain.CurrentDomain.BaseDirectory + $"{fileDir}/{tempFile}";
        }
    }
}
