using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace VendoCoseCommerce.Controllers
{
    public class FileController : Controller
    {
        private IImportService importService;
        
        public FileController(IImportService importService)
        {
            this.importService = importService ?? throw new ArgumentNullException("Import Service");
        }

        // GET: File
        public ActionResult Index()
        {
            if (TempData["ViewData"] != null)
            {
                ViewData = (ViewDataDictionary)TempData["ViewData"];
                TempData["ViewData"] = null;

            }
            return View();
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, ContenutoFile contenuto)
        {
            bool error = false;
            if (file == null || file.ContentLength == 0)
            {
                ModelState.AddModelError("file", "il file è vuoto");
                error = true;
                
            }
            if((int)contenuto == -1)
            {
                ModelState.AddModelError("contenuto", "devi scegliere un tipo di contenuto");
                error = true;
            }
            if (error == true)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index", "File");
            }
            try
            {
                importService.Import(file.InputStream, contenuto);
            }catch(Exception exception)
            {
                return RedirectToAction("Index", "Errore", new { messaggio = $"Errore Importazione: {exception.Message}" });
            }
            return View();
        }
    }
}