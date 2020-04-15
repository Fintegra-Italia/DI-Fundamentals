using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class Product : Entity
    {
        public string Nome { get; set; }
        public string Descrizione { get; set; }
        public string Immagine { get; set; }
        public decimal Prezzo { get; set; }
        public bool Attivo { get; set; }
    }
}
