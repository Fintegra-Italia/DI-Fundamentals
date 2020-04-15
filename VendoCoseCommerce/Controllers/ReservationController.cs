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
        IProductRepository productRepo;
        public ReservationController(IProductRepository productRepo)
        {
            this.productRepo = productRepo ?? throw new ArgumentNullException("Product Repository");
        }
        public ActionResult Index()
        {
            if (Session["loggedAdmin"] == null) return RedirectToAction("Index", "Home");
            var reservationReader = new ReservationReader();
            string filepathPrenotazioni = Server.MapPath("~/App_Data/Prenotazioni.txt");
            IList<Reservation> listaPrenotazioni = reservationReader.Read(filepathPrenotazioni);

            string filepathAccount = Server.MapPath("~/App_Data/Utenti.txt");
            var accountReader = new UserReader();
            IList<Account> listaAccount = accountReader.Read(filepathAccount);

            string filepathConfermato = Server.MapPath("~/App_Data/Confermato.txt");
            var confirmedReader = new ReservationConfirmedReader();
            IList<ReservationConfirmed> prenotazioniConfermate = confirmedReader.Read(filepathConfermato);

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
            var User = (Account)Session["user"];

            string filePath = Server.MapPath(@"/App_Data/Prenotazioni.txt");
            var reader = new ReservationReader();
            IList<Reservation> listaPrenotazioni = reader.Read(filePath);
            var daEliminare = listaPrenotazioni.FirstOrDefault(e => e.Id == Id);
            if (daEliminare != null)
            {
                var writer = new ReservationWriter();
                listaPrenotazioni.Remove(daEliminare);
                writer.Reset(filePath);
                if(listaPrenotazioni.Count > 0)
                {
                    foreach (var preno in listaPrenotazioni)
                    {
                        string linea = $"{preno.Id}|{preno.Data}|{preno.IdAccount}|{preno.IdProdotto}|{preno.NomeProdotto}|{preno.Prezzo}|{preno.Confermata}|{preno.Evasa}";
                        writer.Append(filePath, linea);
                    }
                }
                else
                {
                    writer.Reset(filePath);
                }

            }
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
            var reservationReader = new ReservationReader();
            var reservationWriter = new ReservationWriter();
            string fileReservation = Server.MapPath("~/App_Data/Prenotazioni.txt");
            var reservationConfirmReader = new ReservationConfirmedReader();
            var reservationConfirmWriter = new ReservationConfirmedWriter();
            string fileReservationConfirmed = Server.MapPath("~/App_Data/Confermato.txt");

            IList<Reservation> prenotazioni = reservationReader.Read(fileReservation);
            IList<ReservationConfirmed> confermate = reservationConfirmReader.Read(fileReservationConfirmed);

            ReservationConfirmed evasa = confermate.First(e => e.Id == Id);
            evasa.Evasa = true;
            evasa.DataEvasione = DateTime.Now;
            Reservation daAggiornare = prenotazioni.First(e => e.Id == evasa.IdReservation);
            daAggiornare.Evasa = true;

            reservationWriter.Reset(fileReservation);
            foreach(var pre in prenotazioni)
            {
                string l = $"{pre.Id}|{pre.Data}|{pre.IdAccount}|{pre.IdProdotto}|{pre.NomeProdotto}|{pre.Prezzo}|{pre.Confermata}|{pre.Evasa}";
                reservationWriter.Append(fileReservation, l);
            }

            reservationConfirmWriter.Reset(fileReservationConfirmed);
            foreach(var prenoConf in confermate)
            {
                string linea = $"{prenoConf.Id}|{prenoConf.IdReservation}|{prenoConf.Data}|{prenoConf.IdAccount}|{prenoConf.IdProdotto}|{prenoConf.NomeProdotto}|{prenoConf.Prezzo}|{prenoConf.DataConferma}|{prenoConf.Evasa}|{prenoConf.DataEvasione}";
                reservationConfirmWriter.Append(fileReservationConfirmed, linea);
            }

            return RedirectToAction("Index", "Reservation");

        }
    }
}
