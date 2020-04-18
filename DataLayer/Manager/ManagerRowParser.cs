using DataLayer.Interfaces;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ManagerRowParser : IManagerRowParser
    {
        private char separator;
        public ManagerRowParser(char separator)
        {
            this.separator = separator;
        }
        public Manager Parse(string row)
        {
            if (string.IsNullOrEmpty(row) || string.IsNullOrWhiteSpace(row)) throw new ArgumentNullException("row");
            string[] splitted = row.Split(separator);
            if (splitted.Length != 6) throw new FormatException("invalid format: row");
            int Id, t = -1;

            if (!int.TryParse(splitted[0].Trim(), out Id)) throw new FormatException("invalid format: Id");
            if (string.IsNullOrEmpty(splitted[1]) || string.IsNullOrWhiteSpace(splitted[1])) throw new FormatException("invalid format: Nome");
            if (string.IsNullOrEmpty(splitted[2]) || string.IsNullOrWhiteSpace(splitted[2])) throw new FormatException("invalid format: Cognome");
            if (string.IsNullOrEmpty(splitted[3]) || string.IsNullOrWhiteSpace(splitted[3])) throw new FormatException("invalid format: Email");
            if (string.IsNullOrEmpty(splitted[4]) || string.IsNullOrWhiteSpace(splitted[4])) throw new FormatException("invalid format: Password");
            if (!int.TryParse(splitted[5], out t)) throw new FormatException("invalid format: Ruolo");
            Manager.ruolo ruolo = Manager.ruolo.Guest;
            switch (int.Parse(splitted[5]))
            {
                case 1:
                    ruolo = Manager.ruolo.Amministratore;
                    break;
                case 2:
                    ruolo = Manager.ruolo.SuperUser;
                    break;
                case 3:
                    ruolo = Manager.ruolo.Guest;
                    break;
            }
            if (Id < 0) throw new FormatException("invalid format: Negative Id");

            return new Manager()
            {
                Id = Id,
                Nome = splitted[1],
                Cognome = splitted[2],
                Email = splitted[3],
                Password = splitted[4],
                Ruolo = ruolo
            };
        }
    }
}
