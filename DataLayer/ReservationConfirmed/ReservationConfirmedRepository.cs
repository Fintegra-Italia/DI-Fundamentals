using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ReservationConfirmedRepository : FileRepository<ReservationConfirmed>, IRepository<ReservationConfirmed>
    {
        public ReservationConfirmedRepository(string filePath) : base(filePath) { }
    }
}
