using DataLayer.Interfaces;
using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Import
{
    public class ImportService : IImportService
    {
        private string productFilePath = null;
        private string productFileTemp = null;
        private IProductRowParser productParser;
        private IFileReaderByStream fileReader;
        private IProductSerializer productSerializer;
        public string ProductFilePath
        {
            get
            {
                return productFilePath;
            }
            set
            {
                if (productFilePath == null)
                {
                    if (value == null) throw new ArgumentNullException("FilePath");
                    productFilePath = AppDomain.CurrentDomain.BaseDirectory + value;
                    productFileTemp = TempFileSetup(value);
                }
            }
        }
        public ImportService(IProductRowParser productParser, IFileReaderByStream fileReader, IProductSerializer productSerializer)
        {
            this.productParser = productParser ?? throw new ArgumentNullException("Product Parser");
            this.fileReader = fileReader ?? throw new ArgumentNullException("File Reader");
            this.productSerializer = productSerializer ?? throw new ArgumentNullException("Product Serializer");
        }
        public void Import(Stream file, ContenutoFile contenuto)
        {
            IList<string> listaRighe = fileReader.ReadStream(file);
            switch (contenuto)
            {
                case ContenutoFile.Prodotti:
                    IList<Product> listaProdotti = listaRighe.Select(riga => productParser.Parse(riga)).ToList();
                    using (StreamWriter sw = File.CreateText(productFileTemp))
                    {
                        foreach(var row in listaProdotti)
                        {
                            try
                            {
                                sw.WriteLine(productSerializer.Serialize(row));
                            }catch(Exception exception)
                            {
                                ErrorManager(exception, productFileTemp);
                            }
                        }
                    }
                    SwapAndErase(productFilePath, productFileTemp);
                    break;
                default:
                    throw new InvalidOperationException("Importazione tipo file non consentita");
            }
        }
        private void ErrorManager(Exception exception, string fileToDelete)
        {
            File.Delete(fileToDelete);
            throw new Exception($"Errore scrittura file: {exception.Message}", exception.InnerException);
        }
        private void SwapAndErase(string filePathOrig, string filePathReplace)
        {
            if (File.Exists(filePathOrig))
            {
                File.Delete(filePathOrig);
                File.Copy(filePathReplace, filePathOrig);
            }
            else
            {
                File.Copy(filePathReplace, filePathOrig);
            }
            File.Delete(filePathReplace);
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
