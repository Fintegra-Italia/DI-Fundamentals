using DataLayer.Interfaces;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ReservationRowParser : IReservationRowParser
    {
        private char separator;
        public ReservationRowParser(char separator)
        {
            this.separator = separator;
        }
        public Reservation Parse(string row)
        {
            if (string.IsNullOrEmpty(row) || string.IsNullOrWhiteSpace(row)) throw new ArgumentNullException("row");
            string[] splitted = row.Split(separator);
            if (splitted.Length != 8) throw new FormatException("invalid format: row");
            int Id, IdAcc, IdProd = -1;
            decimal Prezzo = 0M;
            bool Confermata, Evasa = false;
            DateTime Data;

            if (!int.TryParse(splitted[0], out Id)) throw new FormatException("invalid format: Id");
            if (!DateTime.TryParse(splitted[1], out Data)) throw new FormatException("invalid format: Data");
            if (!int.TryParse(splitted[2], out IdAcc)) throw new FormatException("invalid format: IdAccont");
            if (!int.TryParse(splitted[3], out IdProd)) throw new FormatException("invalid format: IdProdotto");
            if (string.IsNullOrEmpty(splitted[4]) || string.IsNullOrWhiteSpace(splitted[3])) throw new FormatException("invalid format: NomeProdotto");
            
            if (!decimal.TryParse(splitted[5], out Prezzo)) throw new FormatException("invalid format: Prezzo");
            if (!bool.TryParse(splitted[6], out Confermata)) throw new FormatException("invalid format: Confermata");
            if (!bool.TryParse(splitted[7], out Evasa)) throw new FormatException("invalid format: Evasa");

            if (Id < 0) throw new FormatException("invalid format: Negative Id");
            if (IdAcc < 0) throw new FormatException("invalid format: Negative IdAccount");
            if (Prezzo < 0) throw new FormatException("invalid format: Prezzo Negativo");
            return new Reservation()
            {
                Id = Id,
                Data = Data,
                IdAccount = IdAcc,
                IdProdotto = IdProd,
                NomeProdotto = splitted[4],
                Prezzo = Prezzo,
                Confermata = Confermata,
                Evasa = Evasa
            };
        }
    }
}