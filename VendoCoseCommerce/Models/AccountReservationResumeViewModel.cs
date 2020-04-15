using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendoCoseCommerce.Models
{
    public class AccountReservationResumeViewModel
    {
        public int NProdotti { get; set; }
        public decimal Totale { get; set; }
        public IList<AccountReservationViewModel> ListaPrenotazioni { get; set; }
    }
} 