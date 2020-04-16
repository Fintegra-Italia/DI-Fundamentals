using System;
using DataLayer;
using DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataLayerTests
{
    [TestClass]
    public class ProductParserTest
    {
        private Product productExpectedFactory(int Id) {
            Product product = null;
            switch (Id)
            {
                case 1:
                    product = new Product()
                    {
                        Id = 1,
                        Nome = "SmartWatch 2",
                        Descrizione = "Smartwatch con funzioni fitness, contapassi etc.",
                        Immagine = "smartwatch-android-display.jpg",
                        Prezzo = 120.50M,
                        Attivo = true
                    };
                    break;

                case 7:
                    product = new Product()
                    {
                        Id = 7,
                        Nome = "Mouse Pad mondo",
                        Descrizione = "Mouse pad con raffigurazione mappa mondiale",
                        Immagine = "mousepadmondo_.jpg",
                        Prezzo = 12M,
                        Attivo = false
                    };
                    break;
                case 8:
                    product = new Product()
                    {
                        Id = 8,
                        Nome = "Mini Cuffie Stereo",
                        Descrizione = "Mini cuffie stereo con microfono per chiamate e ascoltare musica",
                        Immagine = "cuffiefilo.jpg",
                        Prezzo = 16M,
                        Attivo = true
                    };
                    break;

            }
            return product;
        }
        [TestMethod]
        [DataRow("7|Mouse Pad mondo|Mouse pad con raffigurazione mappa mondiale|mousepadmondo_.jpg|12|False", 7 )]
        [DataRow("8|Mini Cuffie Stereo|Mini cuffie stereo con microfono per chiamate e ascoltare musica|cuffiefilo.jpg|16,00|True", 8)]
        [DataRow("1|SmartWatch 2|Smartwatch con funzioni fitness, contapassi etc.|smartwatch-android-display.jpg|120,50|True", 1 )]
        public void ProductParserShouldPass_Test(string row, int Id)
        {
            var parser = new ProductRowParser('|');
            var expected = productExpectedFactory(Id);
            var actual = parser.Parse(row);
            Assert.AreEqual(expected, actual, "Errore non sono identici");
        }
        [TestMethod]
        [DataRow("a|Mouse Pad mondo|Mouse pad con raffigurazione mappa mondiale|mousepadmondo_.jpg|12|False")]
        [DataRow("|Mini Cuffie Stereo|Mini cuffie stereo con microfono per chiamate e ascoltare musica|cuffiefilo.jpg|16,00|True")]
        [DataRow("-1|SmartWatch 2|Smartwatch con funzioni fitness, contapassi etc.|smartwatch-android-display.jpg|120,50|True")]
        [DataRow("8||Mini cuffie stereo con microfono per chiamate e ascoltare musica|cuffiefilo.jpg|16,00|True")]
        [DataRow("8| |Mini cuffie stereo con microfono per chiamate e ascoltare musica|cuffiefilo.jpg|16,00|True")]
        [DataRow("8|Mini Cuffie Stereo||cuffiefilo.jpg|16,00|True")]
        [DataRow("8|Mini Cuffie Stereo| |cuffiefilo.jpg|16,00|True")]
        [DataRow("8|Mini Cuffie Stereo|Mini cuffie stereo con microfono per chiamate e ascoltare musica||16,00|True")]
        [DataRow("8|Mini Cuffie Stereo|Mini cuffie stereo con microfono per chiamate e ascoltare musica|  |16,00|True")]
        [DataRow("8|Mini Cuffie Stereo|Mini cuffie stereo con microfono per chiamate e ascoltare musica|cuffiefilo.jpg||True")]
        [DataRow("8|Mini Cuffie Stereo|Mini cuffie stereo con microfono per chiamate e ascoltare musica|cuffiefilo.jpg|a|True")]
        [DataRow("8|Mini Cuffie Stereo|Mini cuffie stereo con microfono per chiamate e ascoltare musica|cuffiefilo.jpg|-1|True")]
        [DataRow("8|Mini Cuffie Stereo|Mini cuffie stereo con microfono per chiamate e ascoltare musica|cuffiefilo.jpg|16,00|")]
        [DataRow("8|Mini Cuffie Stereo|Mini cuffie stereo con microfono per chiamate e ascoltare musica|cuffiefilo.jpg|16,00| ")]
        [DataRow("8|Mini Cuffie Stereo|Mini cuffie stereo con microfono per chiamate e ascoltare musica|cuffiefilo.jpg|16,00|a")]
        [DataRow("8|Mini Cuffie Stereo|Mini cuffie stereo con microfono per chiamate e ascoltare musica|cuffiefilo.jpg|16,00|55")]
        [DataRow("8|Mini Cuffie Stereo|Mini cuffie stereo con microfono per chiamate e ascoltare musica|cuffiefilo.jpg|")]
        [DataRow("8|Mini Cuffie Stereo|Mini cuffie stereo con microfono per chiamate e ascoltare musica|cuffiefilo.jpg|16,00|55|ab")]
        public void ProductParser_ShouldRaiseAFormatException_Tests(string row)
        {
            var parser = new ProductRowParser('|');
            Assert.ThrowsException<FormatException>(() => parser.Parse(row));
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("  ")]
        [DataRow(null)]
        public void ProductParser_ShouldRaiseArgumentNullException_Test(string row)
        {
            var parser = new ProductRowParser('|');
            Assert.ThrowsException<ArgumentNullException>(() => parser.Parse(row));
        }

    }
}
