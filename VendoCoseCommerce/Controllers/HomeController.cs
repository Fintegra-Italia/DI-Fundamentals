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
        IProductReader productReader;
        string filepath = @"/App_Data/Prodotti.txt";
        public HomeController(IProductReader productReader)
        {

            this.productReader = productReader ?? throw new ArgumentException("product reader");
        }
        public ActionResult Index()
        {
            var productFilePath = Server.MapPath(filepath);
            List<Product> productList = productReader.Read(productFilePath);

            List<ProductViewModel> productListViewModel = productList.Select(
                product => new ProductViewModelFactory(product)
                .SetImageFolder("/Images/")
                .Build()
                ).ToList();

            return View(productListViewModel);
        }

    }
}