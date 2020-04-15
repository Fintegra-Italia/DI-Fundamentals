using DataLayer;
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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var reader = new ProductReader();
            var productFilePath = Server.MapPath(@"/App_Data/Prodotti.txt");
            List<Product> productList = reader.Read(productFilePath);
            List<ProductViewModel> productListViewModel = productList.Select(
                product => new ProductViewModelFactory(product)
                .SetImageFolder("/Images/")
                .Build()
                ).ToList();
            return View(productListViewModel);
        }

    }
}