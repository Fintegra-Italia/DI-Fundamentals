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
    public class AccountController : Controller
    {
        IRepository<Account> accountRepo;
        IRepository<Reservation> reservationRepo;
        IRepository<Manager> managerRepo;
        public AccountController(IRepository<Account> accountRepo, IRepository<Reservation> reservationRepo, IRepository<Manager> managerRepo)
        {
            this.accountRepo = accountRepo ?? throw new ArgumentNullException("Account Repository");
            this.reservationRepo = reservationRepo ?? throw new ArgumentNullException("Reservation Repository");
            this.managerRepo = managerRepo ?? throw new ArgumentNullException("Manager Repository");
        }
        public ActionResult Index()
        {
            if (Session["loggedAdmin"] == null) return RedirectToAction("Index", "Home");
            IList<Account> listaAccount = accountRepo.Get();
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
                IList<Account> listaAccount = accountRepo.Get();
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

                IList<Manager> listaManager = managerRepo.Get();
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
            IList<Reservation> listaPrenotazioni = reservationRepo.Get();
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

            Account account = accountRepo.Get(Id);
            AccountViewModel accountViewModel = new AccountViewModel()
                                                    {
                                                        Id = account.Id,
                                                        Nome = account.Nome,
                                                        Cognome = account.Cognome,
                                                        Email = account.Email,
                                                        TipoAccount = (account.Tipo == Account.tipo.Normal) ? "Normal" : "Premium"
                                                    };
            return View(accountViewModel);
        }
        [HttpPost]
        public ActionResult Edit(int Id, string tipo)
        {
            if (Session["loggedAdmin"] == null) return RedirectToAction("Index", "Home");

            Account daAggiornare = accountRepo.Get(Id);
            daAggiornare.Tipo = (tipo == "Normal") ? Account.tipo.Normal : Account.tipo.Premium;
            accountRepo.Update(daAggiornare);
            return RedirectToAction("Index", "Account");
        }
    }
}