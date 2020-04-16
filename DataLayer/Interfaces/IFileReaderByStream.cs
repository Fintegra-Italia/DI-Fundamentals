using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface IFileReaderByStream
    {
        IList<string> ReadStream(Stream file);
    }
}
