using DataLayer.Interfaces;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ReservationConfirmedRowParser : IReservationConfirmedRowParser
    {
        private char separator;
        public ReservationConfirmedRowParser(char separator)
        {
            this.separator = separator;
        }
        public ReservationConfirmed Parse(string row)
        {
            if (string.IsNullOrEmpty(row) || string.IsNullOrWhiteSpace(row)) throw new ArgumentNullException("row");
            string[] splitted = row.Split(separator);
            if (splitted.Length != 10) throw new FormatException("invalid format: row");
            int Id, IdRes, IdAcc, IdProd;
            decimal Prezzo = 0M;
            bool Evasa = false;
            DateTime Data, DataConferma, DataEvasione;

            if (!int.TryParse(splitted[0], out Id)) throw new FormatException("invalid format: Id");
            if (!int.TryParse(splitted[1], out IdRes)) throw new FormatException("invalid format: IdReservation");
            if (!DateTime.TryParse(splitted[2], out Data)) throw new FormatException("invalid format: Data");
            if (!int.TryParse(splitted[3], out IdAcc)) throw new FormatException("invalid format: IdAccount");
            if (!int.TryParse(splitted[4], out IdProd)) throw new FormatException("invalid format: IdProdotto");
            if (string.IsNullOrEmpty(splitted[5]) || string.IsNullOrWhiteSpace(splitted[5])) throw new FormatException("invalid format: NomeProdotto");
            if (!decimal.TryParse(splitted[6], out Prezzo)) throw new FormatException("invalid format: Prezzo");
            if (!DateTime.TryParse(splitted[7], out DataConferma)) throw new FormatException("invalid format: DataConferma");
            if (!bool.TryParse(splitted[8], out Evasa)) throw new FormatException("invalid format: Evasa");
            if (!DateTime.TryParse(splitted[9], out DataEvasione)) throw new FormatException("invalid format: DataEvasione");          
            
            if (Id < 0) throw new FormatException("invalid format: Negative Id");
            if (IdRes < 0) throw new FormatException("invalid format: Negative Id");
            if (Prezzo < 0) throw new FormatException("invalid format: Prezzo Negativo");
            return new ReservationConfirmed()
            {
                Id = Id,
                IdReservation = IdRes,
                Data = Data,
                IdAccount = IdAcc,
                IdProdotto = IdProd,
                NomeProdotto = splitted[5],
                Prezzo = Prezzo,
                DataConferma = DataConferma,
                Evasa = Evasa,
                DataEvasione = DataEvasione
            };
        }
    }
}
