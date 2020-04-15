using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendoCoseCommerce.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descrizione { get; set; }
        public string Immagine { get; set; }
        public decimal Prezzo { get; set; }
        public bool Attivo { get; set; }
    }
}