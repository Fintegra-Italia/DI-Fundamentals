using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendoCoseCommerce.Models
{
    public class AccountReservationViewModel
    {
        public int IdPrenotazione { get; set; }
        public int IdProdotto { get; set; }
        public string NomeProdotto { get; set; }
        public decimal Prezzo { get; set; }
        public bool Confermata { get; set; }
        public bool Evasa { get; set; }
    }
}