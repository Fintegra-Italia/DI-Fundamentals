using DataLayer;
using DataLayer.Interfaces;
using DomainModel;
using DomainModel.Interfaces;
using System;

using Unity;
using Unity.Injection;
using Unity.Resolution;

namespace VendoCoseCommerce
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        public static void RegisterTypes(IUnityContainer container)
        {
            string productFile = @"App_Data/Prodotti.txt";
            container.RegisterType<IProductWriter, ProductWriter>(new InjectionConstructor(new object[] { productFile, new ProductSerializer('|'), new ProductRowParser('|') }));
            container.RegisterType<IProductReader, ProductReader>(new InjectionConstructor(new object[] { new ProductRowParser('|'), new FlatFileReader(productFile) }));
            container.RegisterType<IProductRepository, ProductRepository>();
        }
    }
}
