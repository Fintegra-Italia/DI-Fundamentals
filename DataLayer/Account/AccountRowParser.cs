using DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;

namespace DataLayer
{
    public class AccountRowParser : IAccountRowParser
    {
        private char separator;
        public AccountRowParser(char separator)
        {
            this.separator = separator;
        }
        public Account Parse(string row)
        {
            if (string.IsNullOrEmpty(row) || string.IsNullOrWhiteSpace(row)) throw new ArgumentNullException("row");
            string[] splitted = row.Split(separator);
            if (splitted.Length != 6) throw new FormatException("invalid format: row");
            int Id, t = -1;
             if (!int.TryParse(splitted[0], out Id)) throw new FormatException("invalid format: Id");
            if (string.IsNullOrEmpty(splitted[1]) || string.IsNullOrWhiteSpace(splitted[1])) throw new FormatException("invalid format: Nome");
            if (string.IsNullOrEmpty(splitted[2]) || string.IsNullOrWhiteSpace(splitted[2])) throw new FormatException("invalid format: Cognome");
            if (string.IsNullOrEmpty(splitted[3]) || string.IsNullOrWhiteSpace(splitted[3])) throw new FormatException("invalid format: Email");
            if (string.IsNullOrEmpty(splitted[4]) || string.IsNullOrWhiteSpace(splitted[4])) throw new FormatException("invalid format: Password");
            if (!int.TryParse(splitted[5], out t)) throw new FormatException("invalid format: tipo");
            if (Id < 0) throw new FormatException("invalid format: Negative Id");
            return new Account()
            {
                Id = Id,
                Nome = splitted[1],
                Cognome = splitted[2],
                Email = splitted[3],
                Password = splitted[4],
                Tipo = (t == 1) ? Account.tipo.Normal : Account.tipo.Premium
            };
        }
    }
}
