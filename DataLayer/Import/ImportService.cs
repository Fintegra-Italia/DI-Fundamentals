using DataLayer.Interfaces;
using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Import
{
    public class ImportService : IImportService
    {
        private string productFilePath = null;
        private string reservationFilePath = null;
        private string reservationConfirmedFilePath = null;
        private string accountFilePath = null;
        private string managerFilePath = null;
        private string tempFilePath = null;
        private IProductRowParser productParser;
        private IReservationRowParser reservationParser;
        private IReservationConfirmedRowParser reservationConfirmedParser;
        private IAccountRowParser accountParser;
        private IManagerRowParser managerParser;
        private IFileReaderByStream fileReader;
        private IRepository<Product> productRepo;
        private IRepository<Reservation> reservationRepo;
        private IRepository<ReservationConfirmed> reservationConfirmedRepo;
        private IRepository<Account> accountRepo;
        private IRepository<Manager> managerRepo;
        public string ProductFilePath
        {
            get
            {
                return productFilePath;
            }
            set
            {
                if (productFilePath == null)
                {
                    if (value == null) throw new ArgumentNullException("FilePath");
                    productFilePath = AppDomain.CurrentDomain.BaseDirectory + value;
                    tempFilePath = TempFileSetup(value);
                }
            }
        }
        public string ReservationFilePath
        {
            get
            {
                return reservationFilePath;
            }
            set
            {
                if (reservationFilePath == null)
                {
                    if (value == null) throw new ArgumentNullException("FilePath");
                    reservationFilePath = AppDomain.CurrentDomain.BaseDirectory + value;
                    tempFilePath = TempFileSetup(value);
                }
            }
        }
        public string ReservationConfirmedFilePath
        {
            get
            {
                return reservationConfirmedFilePath;
            }
            set
            {
                if (reservationConfirmedFilePath == null)
                {
                    if (value == null) throw new ArgumentNullException("FilePath");
                    reservationConfirmedFilePath = AppDomain.CurrentDomain.BaseDirectory + value;
                    tempFilePath = TempFileSetup(value);
                }
            }
        }
        public string AccountFilePath
        {
            get
            {
                return accountFilePath;
            }
            set
            {
                if (accountFilePath == null)
                {
                    if (value == null) throw new ArgumentNullException("FilePath");
                    accountFilePath = AppDomain.CurrentDomain.BaseDirectory + value;
                    tempFilePath = TempFileSetup(value);
                }
            }
        }
        public string ManagerFilePath
        {
            get
            {
                return managerFilePath;
            }
            set
            {
                if (managerFilePath == null)
                {
                    if (value == null) throw new ArgumentNullException("FilePath");
                    managerFilePath = AppDomain.CurrentDomain.BaseDirectory + value;
                    tempFilePath = TempFileSetup(value);
                }
            }
        }
        public ImportService(IProductRowParser productParser,
                    IReservationRowParser reservationParser,
                    IReservationConfirmedRowParser reservationConfirmedParser,
                    IAccountRowParser accountParser,
                    IManagerRowParser managerParser,
                    IRepository<Product> productRepo,
                    IRepository<Reservation> reservationRepo,
                    IRepository<ReservationConfirmed> reservationConfirmedRepo,
                    IRepository<Account> accountRepo,
                    IRepository<Manager> managerRepo,
                    IFileReaderByStream fileReader)
        {
            this.productParser = productParser ?? throw new ArgumentNullException("Product Parser");
            this.reservationParser = reservationParser ?? throw new ArgumentNullException("Reservation Parser");
            this.reservationConfirmedParser = reservationConfirmedParser ?? throw new ArgumentNullException("Reservation Confirmed Parser");
            this.accountParser = accountParser ?? throw new ArgumentNullException("Account Parser");
            this.managerParser = managerParser ?? throw new ArgumentNullException("Manager Parser");
            this.productRepo = productRepo ?? throw new ArgumentNullException("Product Repo");
            this.reservationRepo = reservationRepo ?? throw new ArgumentNullException("Reservation Repo");
            this.reservationConfirmedRepo = reservationConfirmedRepo ?? throw new ArgumentNullException("Reservation Confirmed Repo");
            this.accountRepo = accountRepo ?? throw new ArgumentNullException("Account Repo");
            this.managerRepo = managerRepo ?? throw new ArgumentNullException("Manager Repo");
            this.fileReader = fileReader ?? throw new ArgumentNullException("File Reader");

        }
        public void Import(Stream file, ContenutoFile contenuto)
        {
            IList<string> listaRighe = fileReader.ReadStream(file);
            switch (contenuto)
            {
                case ContenutoFile.Prodotti:
                    IList<Product> listaP = listaRighe.Select(riga => productParser.Parse(riga)).ToList();
                    productRepo.Update(listaP);
                    break;
                case ContenutoFile.Prenotazioni:
                    IList<Reservation> listaR = listaRighe.Select(riga => reservationParser.Parse(riga)).ToList();
                    reservationRepo.Update(listaR);
                    break;
                case ContenutoFile.PrenotazioneConfermate:
                    IList<ReservationConfirmed> listaRC = listaRighe.Select(riga => reservationConfirmedParser.Parse(riga)).ToList();
                    reservationConfirmedRepo.Update(listaRC);
                    break;
                case ContenutoFile.Account:
                    IList<Account> listaA = listaRighe.Select(riga => accountParser.Parse(riga)).ToList();
                    accountRepo.Update(listaA);
                    break;
                case ContenutoFile.Manager:
                    IList<Manager> listaM = listaRighe.Select(riga => managerParser.Parse(riga)).ToList();
                    managerRepo.Update(listaM);
                    break;
                default:
                    throw new InvalidOperationException("Importazione tipo file non consentita");
            }
        }
        private void ErrorManager(Exception exception, string fileToDelete)
        {
            File.Delete(fileToDelete);
            throw new Exception($"Errore scrittura file: {exception.Message}", exception.InnerException);
        }
        private void SwapAndErase(string filePathOrig, string filePathReplace)
        {
            if (File.Exists(filePathOrig))
            {
                File.Delete(filePathOrig);
                File.Copy(filePathReplace, filePathOrig);
            }
            else
            {
                File.Copy(filePathReplace, filePathOrig);
            }
            File.Delete(filePathReplace);
        }
        private string TempFileSetup(string filePath)
        {
            string tempFile;
            string filename = filePath.Split('/').Last();
            string fileDir = filePath.Replace(filename, "");
            string file = filename.Split('.').First();
            string ext = filename.Split('.').Last();
            tempFile = $"{file}_temp.{ext}";
            return AppDomain.CurrentDomain.BaseDirectory + $"{fileDir}/{tempFile}";
        }
    }
}
