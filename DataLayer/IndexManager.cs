using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class IndexManager
    {
        public int Read(string filename)
        {
            if (String.IsNullOrEmpty(filename) || String.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            int index = -1;
            using (StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    index = int.Parse(reader.ReadLine());
                }
            }
            return index;
        }
        public void Write(string filename, int value)
        {
            using (StreamWriter sw = File.CreateText(filename))
            {
                sw.WriteLine(value.ToString());
            }
        }
    }
}
