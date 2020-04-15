using DataLayer;
using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendoCoseCommerce.factories;
using VendoCoseCommerce.Models;

namespace VendoCoseCommerce.Controllers
{
    public class HomeController : Controller
    {
        IProductRepository productRepo;
        public HomeController(IProductRepository productRepo)
        {
            this.productRepo = productRepo ?? throw new ArgumentNullException("Product Repository");
        }
        public ActionResult Index()
        {
            //var reader = new ProductReader();
            //var productFilePath = Server.MapPath(@"/App_Data/Prodotti.txt");
            //List<Product> productList = reader.Read(productFilePath);
            IList<Product> productList = productRepo.Get();
            List<ProductViewModel> productListViewModel = productList.Select(
                product => new ProductViewModelFactory(product)
                .SetImageFolder("/Images/")
                .Build()
                ).ToList();
            return View(productListViewModel);
        }

    }
}