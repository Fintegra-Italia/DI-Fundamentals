using DomainModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataLayer
{
    public class ReservationReader
    {
        public IList<Reservation> Read(string filename)
        {
            if (String.IsNullOrEmpty(filename) || String.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");
             
            var listaReservation = new List<Reservation>();
            if (!File.Exists(filename)) return listaReservation;
            using (StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    var parsed = parse(reader.ReadLine());
                    if (parsed != null)
                    {
                        listaReservation.Add(parsed);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return listaReservation;
        }
        private Reservation parse(string stringToParse)
        {
            if (string.IsNullOrWhiteSpace(stringToParse)) return null;
            string[] splitted = stringToParse.Split('|');
            return new Reservation()
            {
                Id = int.Parse(splitted[0]),
                Data = DateTime.Parse(splitted[1]),
                IdAccount = int.Parse(splitted[2]),
                IdProdotto = int.Parse(splitted[3]),
                NomeProdotto = splitted[4],
                Prezzo = decimal.Parse(splitted[5]),
                Confermata = bool.Parse(splitted[6]),
                Evasa = bool.Parse(splitted[7])
            };
        }
    }
}