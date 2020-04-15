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
    public class HomeController : Controller
    {
        private IEntityPersistence<Product> productPersist;
        public HomeController(IEntityPersistence<Product> productPersist)
        {
            this.productPersist = productPersist;
        }
        public ActionResult Index()
        {
            IList<Product> productList = productPersist.Get();
            List<ProductViewModel> productListViewModel = productList.Select(
                product => new ProductViewModelFactory(product)
                .SetImageFolder("/Images/")
                .Build()
                ).ToList();
            return View(productListViewModel);
        }

    }
}