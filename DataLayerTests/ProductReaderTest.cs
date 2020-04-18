using System;
using System.Collections.Generic;
using DataLayer;
using DataLayer.Interfaces;
using DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DataLayerTests
{
    [TestClass]
    public class ProductReaderTest
    {
       private Product productFactory(string row)
        {
            var splitted = row.Split('|');
            return new Product()
            {
                Id = int.Parse(splitted[0]),
                Nome = splitted[1],
                Descrizione = splitted[2],
                Immagine = splitted[3],
                Prezzo = decimal.Parse(splitted[4]),
                Attivo = bool.Parse(splitted[5])
            };
        }
        [TestMethod]
        public void ProductReader_ShoudPass_Test()
        {
            var productRowParser = new Mock<IProductRowParser>();
            var fileReader = new Mock<IFlatFileReader>();
            fileReader.Setup(e => e.Read()).Returns(new List<string>()
            {
                "1|SmartWatch 2|Smartwatch con funzioni fitness, contapassi etc.|smartwatch-android-display.jpg|120,50|True"
            });
            productRowParser.Setup(
                e => e.Parse(It.Is<string>(k => k == "1|SmartWatch 2|Smartwatch con funzioni fitness, contapassi etc.|smartwatch-android-display.jpg|120,50|True")))
                .Returns(productFactory("1|SmartWatch 2|Smartwatch con funzioni fitness, contapassi etc.|smartwatch-android-display.jpg|120,50|True"));
            IList<Product> expected = new List<Product>()
            {
                productFactory("1|SmartWatch 2|Smartwatch con funzioni fitness, contapassi etc.|smartwatch-android-display.jpg|120,50|True"),
            };
            Assert.IsTrue(false, "TEst da modificare o eliminare");
            //var productReader = new ProductReader(productRowParser.Object, fileReader.Object);

            //IList<Product> actual = productReader.Get();

            //CollectionAssert.AreEqual((List<Product>)expected, (List<Product>)actual, "Errore non sono identici");
            //productRowParser.Verify(m => m.Parse(It.IsAny<string>()), Times.Once);
        }
    }
}
