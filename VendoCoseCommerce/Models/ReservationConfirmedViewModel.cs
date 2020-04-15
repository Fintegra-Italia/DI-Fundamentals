using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendoCoseCommerce.Models
{
    public class ReservationConfirmedViewModel
    {
        public int Id { get; set; }
        public int IdReservation { get; set; }
        public DateTime Data { get; set; }
        public string Cliente { get; set; }
        public string NomeProdotto { get; set; }
        public decimal Prezzo { get; set; }
        public DateTime DataConferma { get; set; }
        public bool Evasa { get; set; }
        public DateTime DataEvasione { get; set; }
    }
}