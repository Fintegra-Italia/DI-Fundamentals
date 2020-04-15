using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VendoCoseCommerce.Controllers
{
    public class AdminAreaController : Controller
    {
        // GET: AdminArea
        public ActionResult Index()
        {
            if (Session["loggedAdmin"] == null) return RedirectToAction("Index", "Home");
            return View();
        }
    }
}