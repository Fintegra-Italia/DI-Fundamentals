using DataLayer;
using DataLayer.Interfaces;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendoCoseCommerce.factories;
using VendoCoseCommerce.Models;

namespace VendoCoseCommerce.Controllers
{
    public class ProductController : Controller
    {
        private IEntityPersistence<Product> productPersist;
        public ProductController(IEntityPersistence<Product> productPersist)
        {
            this.productPersist = productPersist;
        }
        public ActionResult Index()
        {
            if (Session["loggedAdmin"] == null) return RedirectToAction("Index", "Home");
            IList<Product> productList = productPersist.Get();
            IList<ProductViewModel> productListViewModel = productList.Select(
                product => new ProductViewModelFactory(product)
                .SetImageFolder("/Images/")
                .Build())
                .ToList();
            return View(productListViewModel);
        }
        public ActionResult Details(int Id)
        {
            Product product = productPersist.Get(Id);
            ProductViewModel productViewModel = new ProductViewModelFactory(product).SetImageFolder("/Images/").Build();
            return View(productViewModel);
        }
        public ActionResult Edit(int Id)
        {
            if (Session["loggedAdmin"] == null) return RedirectToAction("Index", "Home");
            Product product = productPersist.Get(Id);
            ProductViewModel productViewModel = new ProductViewModelFactory(product).SetImageFolder("/Images/").Build();
            return View(productViewModel);
        }

        [HttpPost]
        public ActionResult Edit(ProductViewModel productViewModel)
        {
            if (Session["loggedAdmin"] == null) return RedirectToAction("Index", "Home");

            Product daInserire = new Product()
            {
                Id = productViewModel.Id,
                Nome = productViewModel.Nome,
                Descrizione = productViewModel.Descrizione,
                Immagine = productViewModel.Immagine.Replace("/Images/", ""),
                Attivo = productViewModel.Attivo,
                Prezzo = productViewModel.Prezzo
            };
            productPersist.Update(daInserire);
            return RedirectToAction("Index", "Product");
        }
    }
}