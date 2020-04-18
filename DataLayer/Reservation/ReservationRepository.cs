using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ReservationRepository : FileRepository<Reservation>, IRepository<Reservation>
    {
        public ReservationRepository(string filePath) : base(filePath) { }
    }
}
