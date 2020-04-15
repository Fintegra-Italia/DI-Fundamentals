using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class Manager
    {
        public enum ruolo
        {
            Amministratore = 1,
            SuperUser = 2,
            Guest = 3
        }
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ruolo Ruolo { get; set; }
    }
}
