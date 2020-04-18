using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;

namespace DataLayer.Interfaces
{
    public interface IReservationConfirmedRowParser
    {
        ReservationConfirmed Parse(string row);
    }
}
