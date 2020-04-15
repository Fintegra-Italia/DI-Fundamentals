using DataLayer;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendoCoseCommerce.Models;

namespace VendoCoseCommerce.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            if (Session["loggedAdmin"] == null) return RedirectToAction("Index", "Home");
            string filePath = Server.MapPath(@"/App_Data/Utenti.txt");
            var reader = new UserReader();
            IList<Account> listaAccount = reader.Read(filePath);
            IList<AccountViewModel> listaAccountViewModel = listaAccount.Select(account => new AccountViewModel()
                                                                            {
                                                                               Id = account.Id,
                                                                               Nome = account.Nome,
                                                                               Cognome = account.Cognome,
                                                                               Email = account.Email,
                                                                               TipoAccount = (Account.tipo.Normal == account.Tipo)?"Normale":"Premium"
                                                                            }).ToList();
            return View(listaAccountViewModel);
        }
        public ActionResult LoginAdmin()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginExec(AccountLoginViewModel accountLogin) {
            if (ModelState.IsValid)
            {
                string filePath = Server.MapPath(@"/App_Data/Utenti.txt");
                var reader = new UserReader();
                IList<Account> listaAccount = reader.Read(filePath);
                Account verificaEmail = listaAccount.FirstOrDefault(e => e.Email == accountLogin.Email);
                if (verificaEmail == null)
                {
                    ModelState.AddModelError("Email", "Email non presente");
                    return View("Login");
                }
                Account verificaAccount = listaAccount.FirstOrDefault(e => e.Email == accountLogin.Email && e.Password == accountLogin.Password);
                if(verificaAccount == null)
                {
                    ModelState.AddModelError("", "Credenziali Errate");
                    return View("Login");
                }
                Session["user"] = verificaAccount;
                Session["logged"] = true;
                return RedirectToAction("Index", "Home");
            }
            return View("Login");
        }
        [HttpPost]
        public ActionResult LoginAdminExec(AccountLoginViewModel accountLogin)
        {
            if (ModelState.IsValid)
            {
                string filePath = Server.MapPath(@"/App_Data/Gestori.txt");
                var reader = new ManagerReader();
                IList<Manager> listaManager = reader.Read(filePath);
                Manager verificaEmail = listaManager.FirstOrDefault(e => e.Email == accountLogin.Email);
                if (verificaEmail == null)
                {
                    ModelState.AddModelError("Email", "Email non presente");
                    return View("LoginAdmin");
                }
                Manager verificaAccount = listaManager.FirstOrDefault(e => e.Email == accountLogin.Email && e.Password == accountLogin.Password);
                if (verificaAccount == null)
                {
                    ModelState.AddModelError("", "Credenziali Errate");
                    return View("LoginAdmin");
                }
                Session["admin"] = verificaAccount;
                Session["loggedAdmin"] = true;
                return RedirectToAction("Index", "Home");
            }
            return View("LoginAdmin");
        }
        public ActionResult Logout()
        {
            Session["user"] = null;
            Session["logged"] = null;
            Session["admin"] = null;
            Session["loggedAdmin"] = null;
            return View();
        }
        public ActionResult Detail()
        {
            if (Session["logged"] == null) return RedirectToAction("Index", "Home");
            var User = (Account)Session["user"];

            string filePath = Server.MapPath(@"/App_Data/Prenotazioni.txt");
            var reader = new ReservationReader();
            IList<Reservation> listaPrenotazioni = reader.Read(filePath);
            IList<AccountReservationViewModel> listaPrenotazioniAccount = listaPrenotazioni.Where(e => e.IdAccount == User.Id)
                                                                                            .Select(res => new AccountReservationViewModel()
                                                                                                {
                                                                                                    IdPrenotazione = res.Id,
                                                                                                    IdProdotto = res.IdProdotto,
                                                                                                    NomeProdotto = res.NomeProdotto,
                                                                                                    Prezzo = res.Prezzo,
                                                                                                    Confermata = res.Confermata,
                                                                                                    Evasa = res.Evasa
                                                                                                }).ToList();

            var riepilogoPrenotazioni = new AccountReservationResumeViewModel()
            {
                NProdotti = listaPrenotazioniAccount.Count,
                Totale = listaPrenotazioniAccount.Sum(e => e.Prezzo),
                ListaPrenotazioni = listaPrenotazioniAccount
            };
            var AccountDetail = new AccountDetailViewModel()
            {
                Account = new AccountViewModel()
                {
                    Nome = User.Nome,
                    Cognome = User.Cognome,
                    Email = User.Email,
                    TipoAccount = (User.Tipo == Account.tipo.Normal)?"Normale":"Premium"
                },
                RiepilogoPrenotazioni = riepilogoPrenotazioni

            };
            return View(AccountDetail);
        }
        public ActionResult Edit(int Id)
        {
            if (Session["loggedAdmin"] == null) return RedirectToAction("Index", "Home");
            string filePath = Server.MapPath(@"/App_Data/Utenti.txt");
            var reader = new UserReader();
            IList<Account> listaAccount = reader.Read(filePath);
            AccountViewModel account = listaAccount.Where(e => e.Id == Id).Select(acc => new AccountViewModel()
            {
                Id = acc.Id,
                Nome = acc.Nome,
                Cognome = acc.Cognome,
                Email = acc.Email,
                TipoAccount = (acc.Tipo == Account.tipo.Normal) ? "Normal" : "Premium"
            }).First();
            return View(account);
        }
        [HttpPost]
        public ActionResult Edit(int Id, string tipo)
        {
            if (Session["loggedAdmin"] == null) return RedirectToAction("Index", "Home");
            string filePath = Server.MapPath(@"/App_Data/Utenti.txt");
            var reader = new UserReader();
            var writer = new UserWriter();
            IList<Account> listaAccount = reader.Read(filePath);
            var daAggiornare = listaAccount.First(e => e.Id == Id);
            daAggiornare.Tipo = (tipo == "Normal") ? Account.tipo.Normal : Account.tipo.Premium;
            writer.Reset(filePath);
            foreach(var account in listaAccount)
            {
                int Tipo = (account.Tipo == Account.tipo.Normal) ? 1 : 2;
                string linea = $"{account.Id},{account.Nome},{account.Cognome},{account.Email},{account.Password},{Tipo}";
                writer.Append(filePath, linea);
            }
            return RedirectToAction("Index", "Account");
        }
    }
}