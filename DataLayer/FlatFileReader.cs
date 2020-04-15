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
            this.filePath = filePath ?? throw new ArgumentNullException("file path");
        }
        public IList<string> Read()
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException("filepath");
            IList<string> stringList = new List<string>();
            using(StreamReader reader = new StreamReader(filePath))
            {
                stringList.Add(reader.ReadLine());
            }
            return stringList;
        }
    }
}
