using DomainModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ManagerReader
    {
        public IList<Manager> Read(string filename)
        {
            if (String.IsNullOrEmpty(filename) || String.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            var listaManager = new List<Manager>();
            using (StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    listaManager.Add(parse(reader.ReadLine()));
                }
            }
            return listaManager;
        }
        private Manager parse(string stringToParse)
        {
            string[] splitted = stringToParse.Split(',');
            Manager.ruolo ruolo = Manager.ruolo.Guest;
            switch (int.Parse(splitted[5])) {
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
            return new Manager()
            {
                Id = int.Parse(splitted[0]),
                Nome = splitted[1],
                Cognome = splitted[2],
                Email = splitted[3],
                Password = splitted[4],
                Ruolo = ruolo
            };
        }
    }
}
