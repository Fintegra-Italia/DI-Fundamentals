using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class ReservationConfirmed : Entity
    {
        public int IdReservation { get; set; }
        public DateTime Data { get; set; }
        public int IdAccount { get; set; }
        public int IdProdotto { get; set; }
        public string NomeProdotto { get; set; }
        public decimal Prezzo { get; set; }
        public DateTime DataConferma { get; set; }
        public bool Evasa { get; set; }
        public DateTime DataEvasione { get; set; }
    }
}
