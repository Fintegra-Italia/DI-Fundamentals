﻿using System;
using System.Collections.Generic;
using System.IO;
using DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataLayerTests
{
    [TestClass]
    public class FileTest
    {

        string fileTestPath = @"FileForTest\Prodotti.txt";
        string fileInsistente = "fileinesistente";
        string fileVuoto = @"FileForTest\fileVuoto.txt";
        public FileTest()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory.Replace(@"bin\Debug", "");
            fileTestPath = baseDir + fileTestPath;
            fileVuoto = baseDir + fileVuoto;
        }

        [TestMethod]
        public void FireReaderShouldPass_Test()
        {
            var reader = new FlatFileReader(fileTestPath);
            IList<string> expected = new List<string>()
            {
                "1|SmartWatch 2|Smartwatch con funzioni fitness, contapassi etc.|smartwatch-android-display.jpg|120,50|True",
                "7|Mouse Pad mondo|Mouse pad con raffigurazione mappa mondiale |mousepadmondo_.jpg|12|True",
                "8|Mini Cuffie Stereo|Mini cuffie stereo con microfono per chiamate e ascoltare musica|cuffiefilo.jpg|16,00|True",
                "9|Cavetto USB 1mt|Cavo USB nero lunghezza 1mt|cavousb1mt.jpg|2,50|True",
                "2|Mouse Longitex|Mouse senza fili, wifi con lettura laser|mouse_laser.jpg|22,00|True",
                "4|Mouse Initial B|Mouse bluethoot, funziona su ogni superfice|mouse203.jpg|25,50|True",
                "5|Smarthpone Android AK-47|Smarthphone con doppia fotocamera,memoria 64GB|smartphone-android.jpg|235,00|True",
                "6|Smart Band|Smart Band con funzioni fitness, contapassi etc.|smartband.jpg|98,50|False",
                "3|Alimentatore Smartphone|Caricabatteria 5W a carica rapida connettore micro USB|alimentatore_usb_Bianco.jpg|12|True"
            };
            var actual = reader.Read();
            CollectionAssert.AreEqual((List<string>)expected, (List<string>)actual, "Errore liste non identiche");
        }
        public void FileReader_EmptyFile_ShouldPass_Test()
        {
            var reader = new FlatFileReader(fileVuoto);
            IList<string> expected = new List<string>();
            var actual = reader.Read();
            CollectionAssert.AreEqual((List<string>)expected, (List<string>)actual, "Errore liste non identiche");
        }
        public void FileReader_FileNotFound_ShoulThrowAnExcetpion_Test()
        {
            var reader = new FlatFileReader(fileInsistente);
            Assert.ThrowsException<FileNotFoundException>(() => reader.Read());
        }
    }
}
