using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendoCoseCommerce.Models
{
    public class ReservationViewModel
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Cliente { get; set; }
        public string NomeProdotto { get; set; }
        public decimal Prezzo { get; set; }
        public bool Confermata { get; set; }
    }
}