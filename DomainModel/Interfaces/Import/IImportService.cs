using System;
using System.IO;

namespace DomainModel.Interfaces
{
    public enum ContenutoFile
    {
        Prodotti = 1,
        Prenotazioni = 2,
        PrenotazioneConfermate = 3
    }
    public interface IImportService
    {
        void Import(Stream file, ContenutoFile contenuto);
    }
}