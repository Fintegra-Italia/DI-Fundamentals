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
    public class FilePersistence<T> : IEntityPersistence<T> where T: Entity
    {
        private string filePath;
        private string fileTemp;
        private IRowParser<T> rowParser;
        private IEntitySerializer<T> entitySerializer;
        private bool? skipFirstRow;
        public bool SkipFirstRow
        {
            set
            {
                if (skipFirstRow == null)
                {
                    skipFirstRow = value;
                }
            }
            get
            {
                if (skipFirstRow == null) return false;
                return (bool)skipFirstRow;
            }
        }
        public FilePersistence(string filePath, IRowParser<T> rowParser, IEntitySerializer<T> rowSerialize)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException("FilePath");
            }
            this.filePath = AppDomain.CurrentDomain.BaseDirectory +  filePath;
            this.rowParser = rowParser ?? throw new ArgumentNullException("Row Parser");
            this.entitySerializer = rowSerialize ?? throw new ArgumentNullException("Entity Serializer");
            fileTemp = TempFileSetup(filePath);
        }
        public void Create(T entity)
        {
            if (entity == null) throw new ArgumentNullException("value");
            if (File.Exists(filePath)){
                using (StreamWriter writer = File.AppendText(filePath))
                {
                    string row = entitySerializer.Serialize(entity);
                    writer.WriteLine(row);
                }
            }
            else
            {
                using (StreamWriter writer = File.CreateText(filePath))
                {
                    string row = entitySerializer.Serialize(entity);
                    writer.WriteLine(row);
                }
            }
        }
        public void Delete(int Id)
        {
            if (!File.Exists(filePath)) throw new FileLoadException("file doesn't exist");
            int pos = -1;
            using (StreamReader reader = new StreamReader(filePath))
            using(StreamWriter writer = File.CreateText(fileTemp))
            {
                while (!reader.EndOfStream)
                {
                    pos++;
                    if (SkipFirstRow && pos == 0)
                    {
                        reader.ReadLine();
                        continue;
                    }

                    T entityReaded = rowParser.Parse(reader.ReadLine());
                    if (entityReaded.Id == Id) continue;
                    writer.WriteLine(entitySerializer.Serialize(entityReaded));
                }
            }
            SwapAndErase();
        }
        public T Get(int Id)
        {
            if (!File.Exists(filePath)) throw new FileLoadException("file doesn't exist");
            T entityReaded = null;
            int pos = -1;
            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    pos++;
                    if (SkipFirstRow && pos == 0)
                    {
                        reader.ReadLine();
                        continue;
                    }

                     entityReaded = rowParser.Parse(reader.ReadLine());
                    if (entityReaded.Id == Id)
                        break;
                }
            }
            return entityReaded;
        }
        public IList<T> Get()
        {
            if (!File.Exists(filePath)) throw new FileLoadException("file doesn't exist");
            IList<T> entityList = new List<T>();
            int pos = -1;
            using(StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    pos++;
                    if (SkipFirstRow && pos == 0)
                    {
                        reader.ReadLine();
                        continue;
                    }

                    T entityReaded = rowParser.Parse(reader.ReadLine());
                    entityList.Add(entityReaded);
                }
            }
            return entityList;
        }
        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            if (!File.Exists(filePath)) throw new FileLoadException("file doesn't exist");
            int pos = -1;
            using (StreamReader reader = new StreamReader(filePath))
            using (StreamWriter writer = File.CreateText(fileTemp))
            {
                while (!reader.EndOfStream)
                {
                    pos++;
                    if (SkipFirstRow && pos == 0)
                    {
                        reader.ReadLine();
                        continue;
                    }

                    T entityReaded = rowParser.Parse(reader.ReadLine());
                    if (entityReaded.Id == entity.Id)
                    {
                        writer.WriteLine(entitySerializer.Serialize(entity));
                    }
                    else
                    {
                        writer.WriteLine(entitySerializer.Serialize(entityReaded));
                    }
                }
            }
            SwapAndErase();
        }

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
