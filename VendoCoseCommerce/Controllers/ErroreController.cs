using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VendoCoseCommerce.Controllers
{
    public class ErroreController : Controller
    {
        // GET: Errore
        public ActionResult Index(string messaggio)
        {
            ViewBag.messaggio = messaggio;
            return View();
        }
    }
}