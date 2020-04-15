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
    public class ProductController : Controller
    {

        public ActionResult Index()
        {
            if (Session["loggedAdmin"] == null) return RedirectToAction("Index", "Home");
            var reader = new ProductReader();
            IList<Product> productList = reader.Read(Server.MapPath(@"/App_Data/Prodotti.txt"));

            IList<ProductViewModel> productListViewModel = productList.Select(product => new ProductViewModel() {
                                                                                                Id = product.Id,
                                                                                                Nome = product.Nome,
                                                                                                Descrizione = product.Descrizione,
                                                                                                Immagine = @"/Images/" + product.Immagine,
                                                                                                Prezzo = product.Prezzo,
                                                                                                Attivo = product.Attivo
                                                                                        }).ToList();
            return View(productListViewModel);
        }
        public ActionResult Details(int Id)
        {
            var reader = new ProductReader();
            List<Product> productList = reader.Read(Server.MapPath(@"/App_Data/Prodotti.txt"));
            ProductViewModel productVieModel = productList.Where(e => e.Id == Id).Select(product => new ProductViewModel()
                                                                                            {
                                                                                                Id = product.Id,
                                                                                                Nome = product.Nome,
                                                                                                Descrizione = product.Descrizione,
                                                                                                Immagine = @"/Images/" + product.Immagine,
                                                                                                Prezzo = product.Prezzo,
                                                                                                Attivo = product.Attivo
                                                                                            }).FirstOrDefault(); 
            return View(productVieModel);
        }
        public ActionResult Edit(int Id)
        {
            if (Session["loggedAdmin"] == null) return RedirectToAction("Index", "Home");
            var reader = new ProductReader();
            List<Product> productList = reader.Read(Server.MapPath(@"/App_Data/Prodotti.txt"));
            ProductViewModel productViewModel = productList.Where(e=>e.Id==Id).Select(product => new ProductViewModel()
                                                {
                                                    Id = product.Id,
                                                    Nome = product.Nome,
                                                    Descrizione = product.Descrizione,
                                                    Immagine = @"/Images/" + product.Immagine,
                                                    Prezzo = product.Prezzo,
                                                    Attivo = product.Attivo
                                                }).Single();
            return View(productViewModel);
        }

        [HttpPost]
        public ActionResult Edit(ProductViewModel productViewModel)
        {
            if (Session["loggedAdmin"] == null) return RedirectToAction("Index", "Home");
            var reader = new ProductReader();
            var productFilePath = Server.MapPath(@"/App_Data/Prodotti.txt");
            List<Product> productList = reader.Read(productFilePath);
            ProductViewModel productViewModelOld = productList.Where(e => e.Id == productViewModel.Id).Select(product => new ProductViewModel()
            {
                Id = product.Id,
                Nome = product.Nome,
                Descrizione = product.Descrizione,
                Immagine = @"/Images/" + product.Immagine,
                Prezzo = product.Prezzo,
                Attivo = product.Attivo
            }).FirstOrDefault();
            productViewModel.Immagine = productViewModelOld.Immagine;

            Product daInserire = new Product()
            {
                Id = productViewModel.Id,
                Nome = productViewModel.Nome,
                Descrizione = productViewModel.Descrizione,
                Immagine = productViewModel.Immagine.Replace("/Images/", ""),
                Attivo = productViewModel.Attivo,
                Prezzo = productViewModel.Prezzo
            };

            productList[productList.FindIndex(e => e.Id == productViewModelOld.Id)] = daInserire;

            var productWriter = new ProductWriter();
            productWriter.Reset(productFilePath);
            foreach (var prod in productList)
            {
                string linea = $"{prod.Id}|{prod.Nome}|{prod.Descrizione}|{prod.Immagine}|{prod.Prezzo}|{prod.Attivo}|";
                productWriter.Append(productFilePath, linea);

            }
            return RedirectToAction("Index", "Product");
        }
    }
}