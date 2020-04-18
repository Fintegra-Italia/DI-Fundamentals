using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ManagerRepository : FileRepository<Manager>, IRepository<Manager>
    {
        public ManagerRepository(string filePath) : base(filePath) { }
    }
}
