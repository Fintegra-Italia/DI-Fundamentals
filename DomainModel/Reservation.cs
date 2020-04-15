using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public int IdAccount { get; set; }
        public string NomeProdotto { get; set; }
        public int IdProdotto { get; set; }
        public decimal Prezzo { get; set; }
        public bool Confermata { get; set; }
        public bool Evasa { get; set; }
    }
}
