using DomainModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataLayer
{
    public class UserReader
    {
        public IList<Account> Read(string filename)
        {
            if (String.IsNullOrEmpty(filename) || String.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            var listaAccount = new List<Account>();
            using (StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    listaAccount.Add(parse(reader.ReadLine()));
                }
            }
            return listaAccount;
        }
        private Account parse(string stringToParse)
        {
            string[] splitted = stringToParse.Split(',');
            return new Account()
            {
                Id = int.Parse(splitted[0]),
                Nome = splitted[1],
                Cognome = splitted[2],
                Email = splitted[3],
                Password = splitted[4],
                Tipo = (int.Parse(splitted[5]) == 1)? Account.tipo.Normal : Account.tipo.Premium
            };
        }
    }
}