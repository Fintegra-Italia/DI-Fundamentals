using DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class FlatFileReader : IFlatFileReader
    {
        private string filePath;
        public FlatFileReader(string filePath)
        {
            if (filePath == null) throw new ArgumentNullException("file path");
            this.filePath = AppDomain.CurrentDomain.BaseDirectory + filePath ;
        }
        public IList<string> Read()
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException("filepath");
            IList<string> stringList = new List<string>();
            using(StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    stringList.Add(reader.ReadLine());
                }
                
            }
            return stringList;
        }
    }
}
