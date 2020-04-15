using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomainModel
{
    public class Account
    {
        public enum tipo {
            Normal = 1,
            Premium = 2
        }
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public tipo Tipo { get; set; }
    }
}