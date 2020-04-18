using DataLayer;
using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendoCoseCommerce.Models;

namespace VendoCoseCommerce.Controllers
{
    public class ReservationController : Controller
    {
        IRepository<Product> productRepo;
        IRepository<Reservation> reservationRepo;
        IRepository<ReservationConfirmed> reservationConfirmedRepo;
        IRepository<Account> accountRepo;
        public ReservationController(IRepository<Product> productRepo, IRepository<Reservation> reservationRepo,
                                        IRepository<ReservationConfirmed> reservationConfirmedRepo, IRepository<Account> accountRepo)
        {
            this.productRepo = productRepo ?? throw new ArgumentNullException("Product Repository");
            this.reservationRepo = reservationRepo ?? throw new ArgumentNullException("Reservation Repository");
            this.reservationConfirmedRepo = reservationConfirmedRepo ?? throw new ArgumentNullException("Reservation Confirmed Repository");
            this.accountRepo = accountRepo ?? throw new ArgumentNullException("Account Repository");
        }
        public ActionResult Index()
        {
            if (Session["loggedAdmin"] == null) return RedirectToAction("Index", "Home");

            IList<Reservation> listaPrenotazioni = reservationRepo.Get();
            IList<ReservationConfirmed> prenotazioniConfermate = reservationConfirmedRepo.Get();
            IList<Account> listaAccount = accountRepo.Get();

            IList<ReservationViewModel> listaPrenotazioneViewModel = listaPrenotazioni.Where(k=>k.Confermata==false)
                .Select(pre => new ReservationViewModel()
            {
                Id = pre.Id,
                Data = pre.Data,
                NomeProdotto = pre.NomeProdotto,
                Cliente = $"{listaAccount.First(e => e.Id == pre.IdAccount).Nome} {listaAccount.First(e => e.Id == pre.IdAccount).Cognome}",
                Prezzo = pre.Prezzo,
                Confermata = pre.Confermata
            }).ToList();

            IList<ReservationConfirmedViewModel> prenotazioniConfermateViewModel = prenotazioniConfermate.Where(e => e.Evasa == false)
                .Select(pre => new ReservationConfirmedViewModel()
                {
                    Id = pre.Id,
                    Data = pre.Data,
                    IdReservation = pre.IdReservation,
                    Cliente = $"{listaAccount.First(e => e.Id == pre.IdAccount).Nome} {listaAccount.First(e => e.Id == pre.IdAccount).Cognome}",
                    NomeProdotto = pre.NomeProdotto,
                    Prezzo = pre.Prezzo,
                    DataConferma = pre.DataConferma
                }).ToList();

            IList<ReservationConfirmedViewModel> prenotazioniEvaseViewModel = prenotazioniConfermate.Where(e => e.Evasa == true)
                    .Select(pre => new ReservationConfirmedViewModel()
                    {
                        Id = pre.Id,
                        IdReservation = pre.IdReservation,
                        Data = pre.Data,
                        Cliente = $"{listaAccount.First(e => e.Id == pre.IdAccount).Nome} {listaAccount.First(e => e.Id == pre.IdAccount).Cognome}",
                        NomeProdotto = pre.NomeProdotto,
                        Prezzo = pre.Prezzo,
                        Evasa = pre.Evasa,
                        DataEvasione= pre.DataEvasione
                    }).ToList();

            var reservationResume = new ReservationResumeViewModel();
            reservationResume.listaPrenotazioniAttive = listaPrenotazioneViewModel;
            reservationResume.listaPrenotazioniConfermate = prenotazioniConfermateViewModel;
            reservationResume.listaPrenotazioniEvase = prenotazioniEvaseViewModel;
            return View(reservationResume);
        }
        public ActionResult Remove(int Id)
        {
            if (Session["logged"] == null) return RedirectToAction("Index", "Home");
            reservationRepo.Delete(Id);
            return RedirectToAction("Detail", "Account");
        }

        [HttpPost]
        public ActionResult Add(int IdProdotto)
        {
            if (Session["logged"] == null) return RedirectToAction("Index", "Home");
            var User = (Account)Session["user"];
            string fileProdotti = Server.MapPath(@"/App_Data/Prodotti.txt");
            string fileIndicePrenotazioni = Server.MapPath(@"/App_Data/Prenotazioni_Last_Id.txt");
            string filePrenotazioni = Server.MapPath(@"/App_Data/Prenotazioni.txt");
            //var productReader = new ProductReader();
            var indexManager = new IndexManager();
            var reservationWriter = new ReservationWriter();
            IList<Product> listaProdotti = productRepo.Get();//productReader.Read(fileProdotti);

            Product prodotto = listaProdotti.FirstOrDefault(e => e.Id == IdProdotto);
            if (prodotto != null)
            {
                int indicePrenotazione = indexManager.Read(fileIndicePrenotazioni) + 1;
                string linea = $"{indicePrenotazione}|{DateTime.Now}|{User.Id}|{prodotto.Id}|{prodotto.Nome}|{prodotto.Prezzo}|{false}|{false}";
                reservationWriter.Append(filePrenotazioni, linea);
                indexManager.Write(fileIndicePrenotazioni, indicePrenotazione);
            }

            return RedirectToAction("Detail", "Account");
        }
        public ActionResult Confirm(int Id)
        {
            if (Session["loggedAdmin"] == null) return RedirectToAction("Index", "Home");
            var reservationReader = new ReservationReader();
            var reservationWriter = new ReservationWriter();
            var reservationConfirmedWriter = new ReservationConfirmedWriter();
            string filepathPrenotazioniConfermate = Server.MapPath("~/App_Data/Confermato.txt");
            string filepathPrenotazioni = Server.MapPath("~/App_Data/Prenotazioni.txt");
            string fileindexPrenotazioniConfermate = Server.MapPath("~/App_Data/Confermato_Last_Id.txt");
            IList<Reservation> listaPrenotazioni = reservationReader.Read(filepathPrenotazioni);
            var indexManager = new IndexManager();
            int index = indexManager.Read(fileindexPrenotazioniConfermate);

            ReservationConfirmed prenoConf = listaPrenotazioni.Where(e => e.Id == Id)
                .Select(pre => new ReservationConfirmed()
                {
                    Id = index + 1,
                    IdReservation = pre.Id,
                    Data = pre.Data,
                    IdAccount = pre.IdAccount,
                    IdProdotto = pre.IdProdotto,
                    NomeProdotto = pre.NomeProdotto,
                    Prezzo = pre.Prezzo,
                    DataConferma = DateTime.Now,
                    Evasa = false,
                    DataEvasione = new DateTime(2000,1,1,0,0,0)
                }).First();

            string linea = $"{prenoConf.Id}|{prenoConf.IdReservation}|{prenoConf.Data}|{prenoConf.IdAccount}|{prenoConf.IdProdotto}|{prenoConf.NomeProdotto}|{prenoConf.Prezzo}|{prenoConf.DataConferma}|{prenoConf.Evasa}|{prenoConf.DataEvasione}";
            reservationConfirmedWriter.Append(filepathPrenotazioniConfermate, linea);
            indexManager.Write(fileindexPrenotazioniConfermate, index + 1);

            listaPrenotazioni.First(e => e.Id == Id).Confermata = true;
            reservationWriter.Reset(filepathPrenotazioni);
            foreach(var pre in listaPrenotazioni)
            {
                string l = $"{pre.Id}|{pre.Data}|{pre.IdAccount}|{pre.IdProdotto}|{pre.NomeProdotto}|{pre.Prezzo}|{pre.Confermata}|{false}";
                reservationWriter.Append(filepathPrenotazioni, l);
            }

            return RedirectToAction("Index", "Reservation");
        }
        public ActionResult Complete(int Id)
        {
            if (Session["loggedAdmin"] == null) return RedirectToAction("Index", "Home");

            ReservationConfirmed evasa = reservationConfirmedRepo.Get(Id);
            evasa.Evasa = true;
            evasa.DataEvasione = DateTime.Now;
            reservationConfirmedRepo.Update(evasa);

            Reservation daAggiornare = reservationRepo.Get(evasa.IdReservation);
            daAggiornare.Evasa = true;
            reservationRepo.Update(daAggiornare);
            return RedirectToAction("Index", "Reservation");

        }
    }
}
