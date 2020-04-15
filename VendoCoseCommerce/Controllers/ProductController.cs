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
    public class ProductController : Controller
    {
        IProductRepository productRepo;
        public ProductController(IProductRepository productRepo)
        {
            this.productRepo = productRepo ?? throw new ArgumentNullException("Product Repository");
        }
        public ActionResult Index()
        {
            if (Session["loggedAdmin"] == null) return RedirectToAction("Index", "Home");
            //var reader = new ProductReader();
            //IList<Product> productList = reader.Read(Server.MapPath(@"/App_Data/Prodotti.txt"));
            IList<Product> productList = productRepo.Get();
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
            //var reader = new ProductReader();
            //List<Product> productList = reader.Read(Server.MapPath(@"/App_Data/Prodotti.txt"));
            Product product = productRepo.Get(Id);
            //ProductViewModel productVieModel = productList.Where(e => e.Id == Id).Select(product => new ProductViewModel()
            //                                                                                {
            //                                                                                    Id = product.Id,
            //                                                                                    Nome = product.Nome,
            //                                                                                    Descrizione = product.Descrizione,
            //                                                                                    Immagine = @"/Images/" + product.Immagine,
            //                                                                                    Prezzo = product.Prezzo,
            //                                                                                    Attivo = product.Attivo
            //                                                                                }).FirstOrDefault();
            ProductViewModel productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                Nome = product.Nome,
                Descrizione = product.Descrizione,
                Immagine = @"/Images/" + product.Immagine,
                Prezzo = product.Prezzo,
                Attivo = product.Attivo
            };
            return View(productViewModel);
        }
        public ActionResult Edit(int Id)
        {
            if (Session["loggedAdmin"] == null) return RedirectToAction("Index", "Home");
            //var reader = new ProductReader();
            //List<Product> productList = reader.Read(Server.MapPath(@"/App_Data/Prodotti.txt"));
            Product product = productRepo.Get(Id);
            //ProductViewModel productViewModel = productList.Where(e=>e.Id==Id).Select(product => new ProductViewModel()
            //                                    {
            //                                        Id = product.Id,
            //                                        Nome = product.Nome,
            //                                        Descrizione = product.Descrizione,
            //                                        Immagine = @"/Images/" + product.Immagine,
            //                                        Prezzo = product.Prezzo,
            //                                        Attivo = product.Attivo
            //                                    }).Single();
            ProductViewModel productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                Nome = product.Nome,
                Descrizione = product.Descrizione,
                Immagine = @"/Images/" + product.Immagine,
                Prezzo = product.Prezzo,
                Attivo = product.Attivo
            };
            return View(productViewModel);
        }

        [HttpPost]
        public ActionResult Edit(ProductViewModel productViewModel)
        {
            if (Session["loggedAdmin"] == null) return RedirectToAction("Index", "Home");
            //var reader = new ProductReader();
            //var productFilePath = Server.MapPath(@"/App_Data/Prodotti.txt");
            //List<Product> productList = reader.Read(productFilePath);
            //ProductViewModel productViewModelOld = productList.Where(e => e.Id == productViewModel.Id).Select(product => new ProductViewModel()
            //{
            //    Id = product.Id,
            //    Nome = product.Nome,
            //    Descrizione = product.Descrizione,
            //    Immagine = @"/Images/" + product.Immagine,
            //    Prezzo = product.Prezzo,
            //    Attivo = product.Attivo
            //}).FirstOrDefault();
            //productViewModel.Immagine = productViewModelOld.Immagine;

            Product daInserire = new Product()
            {
                Id = productViewModel.Id,
                Nome = productViewModel.Nome,
                Descrizione = productViewModel.Descrizione,
                Immagine = productViewModel.Immagine.Replace("/Images/", ""),
                Attivo = productViewModel.Attivo,
                Prezzo = productViewModel.Prezzo
            };
            productRepo.Update(daInserire);
            //productList[productList.FindIndex(e => e.Id == productViewModelOld.Id)] = daInserire;

            //var productWriter = new ProductWriter();
            //productWriter.Reset(productFilePath);
            //foreach (var prod in productList)
            //{
            //    string linea = $"{prod.Id}|{prod.Nome}|{prod.Descrizione}|{prod.Immagine}|{prod.Prezzo}|{prod.Attivo}|";
            //    productWriter.Append(productFilePath, linea);

            //}
            return RedirectToAction("Index", "Product");
        }
    }
}