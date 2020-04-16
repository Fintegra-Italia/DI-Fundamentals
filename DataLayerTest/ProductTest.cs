using System;
using DataLayer;
using DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataLayerTest
{
    [TestClass]
    public class ProductTest
    {

        [TestMethod]
        public void ProductParseTest()
        {
            var productParser = new ProductParser();
            string row = "1|SmartWatch 2|Smartwatch con funzioni fitness, contapassi etc.|smartwatch-android-display.jpg|120,50|True|";
            Product expected = new Product()
            {
                Id = 1,
                Nome = "SmartWatch 2",
                Descrizione = "Smartwatch con funzioni fitness, contapassi etc.",
                Immagine = "smartwatch-android-display.jpg",
                Prezzo = 120.5M,
                Attivo = true
            };


            Product actual = productParser.Parse(row);

            Assert.AreEqual(expected, actual, "Errore non sono identici");
        }
    }
}
