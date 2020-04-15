using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendoCoseCommerce.Models
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string TipoAccount { get; set; }
        public string Email { get; set; }
    }
}