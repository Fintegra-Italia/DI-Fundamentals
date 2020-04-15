using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VendoCoseCommerce.Models;

namespace VendoCoseCommerce.factories
{
    public class ProductViewModelFactory
    {
        private Product product;
        private string ImageFolder;
        public ProductViewModelFactory(Product product)
        {
            this.product = product ?? throw new ArgumentNullException("product");
        }
        public ProductViewModelFactory SetImageFolder(string imageFolder)
        {
            ImageFolder = imageFolder;
            return this;
        }

        public ProductViewModel Build()
        {
            if (string.IsNullOrEmpty(ImageFolder)) throw new ArgumentNullException("ImageFolder setting by SetImageFolder Method");
            return new ProductViewModel()
            {
                Id = product.Id,
                Nome = product.Nome,
                Descrizione = product.Descrizione,
                Immagine = $@"{ImageFolder}{product.Immagine}",
                Prezzo = product.Prezzo,
                Attivo = product.Attivo
            };
        }
    }
}