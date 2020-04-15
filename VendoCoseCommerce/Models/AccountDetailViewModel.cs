using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendoCoseCommerce.Models
{
    public class AccountDetailViewModel
    {
        public AccountViewModel Account { get; set; }
        public AccountReservationResumeViewModel RiepilogoPrenotazioni { get; set; }
    }
}