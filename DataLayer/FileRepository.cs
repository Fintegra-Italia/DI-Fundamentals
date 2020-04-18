using DomainModel;
using DomainModel.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class FileRepository<T> : IRepository<T> where T : Entity
    {
        private string filePath = null;
        private string tempFilePath = null;
        private class DataBlock
        {
            public class Data
            {
                public int lastId { get; set; }
                public IList<T> EntityList { get; set; }
            }
            public DataBlock()
            {
                lastId = -1;
                EntityList = new List<T>();
                entityTemp = new List<T>();
            }
            private int lastId { get; set; } = -1;
            private IList<T> EntityList { get; set; }
            private IList<T> entityTemp { get; set; }
            public void Add(T entity)
            {
                lastId++;
                entity.Id = lastId;
                EntityList.Add(entity);
            }
            public void Add(IList<T> entities)
            {
                foreach(var entity in entities)
                {
                    lastId++;
                    entity.Id = lastId;
                    EntityList.Add(entity);
                }
            }
            public IList<T> Get()
            {
                return EntityList;
            }
            public T Get(int Id)
            {
                return EntityList.FirstOrDefault(e => e.Id == Id);
            }
            public bool Remove(int Id)
            {
                var daR = EntityList.FirstOrDefault(e => e.Id == Id);
                if (daR == null) return false;
                EntityList.Remove(daR);
                return true;
            }
            public void Update(T entity)
            {
                var daS = EntityList.FirstOrDefault(e => e.Id == entity.Id);
                if (daS != null)
                {
                    EntityList.Remove(daS);
                    EntityList.Add(entity);
                }
                else
                {
                    Add(entity);
                }
                
            }
            private void insertEntity()
            {
                foreach(var entity in entityTemp)
                {
                    Add(entity);
                }
                entityTemp = new List<T>();
            }
            public void Update(IList<T> entities)
            {
                foreach(var entity in entities)
                {
                    var daS = EntityList.FirstOrDefault(e => e.Id == entity.Id);
                    if (daS != null)
                    {
                        EntityList.Remove(daS);
                        EntityList.Add(entity);
                    }
                    else
                    {
                        entityTemp.Add(entity);
                    }
                }
                insertEntity();
            }
            public Data Datum()
            {
                return new Data { lastId = this.lastId, EntityList = this.EntityList };
            }
            public void Datum(Data data)
            {
                this.lastId = data.lastId;
                this.EntityList = data.EntityList;
            }
        }
        public FileRepository(string filePath){
            this.filePath = AppDomain.CurrentDomain.BaseDirectory + filePath;
            tempFilePath = TempFileSetup(filePath);
        }
        public T Get(int Id)
        {
            if (File.Exists(filePath))
            {
                var Block = new DataBlock();
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string serialized = reader.ReadToEnd();
                    Block.Datum(JsonConvert.DeserializeObject<DataBlock.Data>(serialized));
                }
                if (Block.Get() == null) return null;
                return Block.Get(Id);
            }
            return null;
        }
        public IList<T> Get()
        {
            var Block = new DataBlock();
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string serialized = reader.ReadToEnd();
                    Block.Datum(JsonConvert.DeserializeObject<DataBlock.Data>(serialized));
                }
            }
            return Block.Get(); ;
        }
        public void Insert(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            var Block = new DataBlock();
            if (File.Exists(filePath))
            {
                using(StreamReader reader = new StreamReader(filePath))
                {
                    string serialized = reader.ReadToEnd();
                     Block.Datum(JsonConvert.DeserializeObject<DataBlock.Data>(serialized));
                }
            }
            Block.Add(entity);
            using(StreamWriter writer = getWriter(false, tempFilePath))
            {
                string serialized = JsonConvert.SerializeObject(Block.Datum());
                writer.Write(serialized);
            }
            SwapAndErase();
        }
        public void Insert(IList<T> entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            if (File.Exists(filePath))
            {
                Update(entities);
            }
            else
            {
                var Block = new DataBlock();
                Block.Add(entities);
                using (StreamWriter writer = getWriter(false, tempFilePath))
                {
                    string serialized = JsonConvert.SerializeObject(Block.Datum());
                    writer.Write(serialized);
                }
                SwapAndErase();
            }
        }
        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            var Block = new DataBlock();
            if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);
            using (StreamReader reader = new StreamReader(filePath))
            {
                string serialized = reader.ReadToEnd();
                Block.Datum(JsonConvert.DeserializeObject<DataBlock.Data>(serialized));
            }
            Block.Update(entity);

            using (StreamWriter writer = getWriter(false, tempFilePath))
            {
                string serialized = JsonConvert.SerializeObject(Block.Datum());
                writer.Write(serialized);
            }
            SwapAndErase();

        }
        public void Update(IList<T> entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            var Block = new DataBlock();
            if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);
            using (StreamReader reader = new StreamReader(filePath))
            {
                string serialized = reader.ReadToEnd();
                Block.Datum(JsonConvert.DeserializeObject<DataBlock.Data>(serialized));
            }
            Block.Update(entities);
            using (StreamWriter writer = getWriter(false, tempFilePath))
            {
                string serialized = JsonConvert.SerializeObject(Block.Datum());
                writer.Write(serialized);
            }
            SwapAndErase();
        }
        public void Delete(int Id)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);
            var Block = new DataBlock();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string serialized = reader.ReadToEnd();
                Block.Datum(JsonConvert.DeserializeObject<DataBlock.Data>(serialized));
            }

            if (Block.Remove(Id))
            {
                using (StreamWriter writer = getWriter(false, tempFilePath))
                {
                    string serialized = JsonConvert.SerializeObject(Block.Datum());
                    writer.Write(serialized);
                }
                SwapAndErase();
            }
        }

        private void SwapAndErase()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                File.Copy(tempFilePath, filePath);
            }
            else
            {
                File.Copy(tempFilePath, filePath);
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
        private StreamWriter getWriter(bool append, string path)
        {
            return (append) ? File.AppendText(path) : File.CreateText(path);
        }
    }
}
