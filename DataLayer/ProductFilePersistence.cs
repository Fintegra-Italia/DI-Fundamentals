using DataLayer.Interfaces;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ProductFilePersistence: FilePersistence<Product>
    {
        private string filePath;
        private IRowParser<Product> rowParser;
        private IEntitySerializer<Product> entitySerializer;
        public ProductFilePersistence(string filePath, IRowParser<Product> rowParser, IEntitySerializer<Product> rowSerializer):base(filePath, rowParser, rowSerializer)
        {
            this.filePath = filePath ?? throw new ArgumentNullException("filePath");
            this.rowParser = rowParser ?? throw new ArgumentNullException("Row Parser");
            this.entitySerializer = rowSerializer ?? throw new ArgumentNullException("Row Serializer");
        }
    }
}
