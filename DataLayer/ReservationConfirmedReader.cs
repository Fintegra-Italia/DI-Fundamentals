using DomainModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataLayer
{
    public class ReservationConfirmedReader
    {
        public IList<ReservationConfirmed> Read(string filename)
        {
            if (String.IsNullOrEmpty(filename) || String.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            var listaPrenotazioneConfermate = new List<ReservationConfirmed>();
            if (!File.Exists(filename)) return listaPrenotazioneConfermate;
            using (StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    var parsed = parse(reader.ReadLine());
                    if (parsed != null)
                    {
                        listaPrenotazioneConfermate.Add(parsed);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return listaPrenotazioneConfermate;
        }
        private ReservationConfirmed parse(string stringToParse)
        {
            if (string.IsNullOrWhiteSpace(stringToParse)) return null;
            string[] splitted = stringToParse.Split('|');
            return new ReservationConfirmed()
            {
                Id = int.Parse(splitted[0]),
                IdReservation = int.Parse(splitted[1]),
                Data = DateTime.Parse(splitted[2]),
                IdAccount = int.Parse(splitted[3]),
                IdProdotto = int.Parse(splitted[4]),
                NomeProdotto = splitted[5],
                Prezzo = decimal.Parse(splitted[6]),
                DataConferma = DateTime.Parse(splitted[7]),
                Evasa = bool.Parse(splitted[8]),
                DataEvasione = DateTime.Parse(splitted[9])
            };
        }
    }
}
