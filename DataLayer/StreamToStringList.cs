using DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class StreamToStringList : IFileReaderByStream
    {
        public IList<string> ReadStream(Stream file)
        {
            var fileByte = new byte[file.Length];
            file.Read(fileByte, 0, fileByte.Length);
            List<string> righe = new List<string>();

            string singleString = Encoding.UTF8.GetString(fileByte, 0, fileByte.Length);
            var stringhe = singleString.Split('\n');
            for (var i = 0; i < stringhe.Length; i++)
            {
                if (!String.IsNullOrEmpty(stringhe[i]) && !String.IsNullOrWhiteSpace(stringhe[i]))
                {
                    righe.Add(stringhe[i].Replace("\r", ""));
                }
            }
            return righe;
        }
    }
}
