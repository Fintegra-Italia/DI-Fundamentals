using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class AccountRepository : FileRepository<Account>, IRepository<Account>
    {
        public AccountRepository(string filePath) : base(filePath) { }
    }
}
