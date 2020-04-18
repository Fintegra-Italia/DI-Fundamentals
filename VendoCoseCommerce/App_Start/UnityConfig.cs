using DataLayer;
using DataLayer.Import;
using DataLayer.Interfaces;
using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
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
            string productFile = @"App_Data/Prodotti.json";
            var FileMapping = new Dictionary<string, string>()
            {
                { "ProductFilePath",  "App_Data/Prodotti.json" },
                { "ReservationFilePath", "App_Data/Prenotazioni.json"},
                { "ReservationConfirmedFilePath", "App_Data/Confermate.json" },
                { "AccountFilePath", "App_Data/Utenti.json" },
                { "ManagerFilePath", "App_Data/Gestori.json" }
            };
            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<IProductRowParser, ProductRowParser>(new InjectionConstructor(new object[] { '|' }));
            container.RegisterType<IReservationRowParser, ReservationRowParser>(new InjectionConstructor(new object[] { '|' }));
            container.RegisterType<IReservationConfirmedRowParser, ReservationConfirmedRowParser>(new InjectionConstructor(new object[] { '|' }));
            container.RegisterType<IAccountRowParser, AccountRowParser>(new InjectionConstructor(new object[] { ',' }));
            container.RegisterType<IManagerRowParser, ManagerRowParser>(new InjectionConstructor(new object[] { ',' }));

            container.RegisterType<IFileReaderByStream, StreamToStringList>();
            container.RegisterType<IRepository<Product>, ProductRepository>(new InjectionConstructor(new object[] { FileMapping["ProductFilePath"] }));
            container.RegisterType<IRepository<Reservation>, ReservationRepository>(new InjectionConstructor(new object[] { FileMapping["ReservationFilePath"] }));
            container.RegisterType<IRepository<ReservationConfirmed>, ReservationConfirmedRepository>(new InjectionConstructor(new object[] { FileMapping["ReservationConfirmedFilePath"] }));
            container.RegisterType<IRepository<Account>, AccountRepository>(new InjectionConstructor(new object[] { FileMapping["AccountFilePath"] }));
            container.RegisterType<IRepository<Manager>, ManagerRepository>(new InjectionConstructor(new object[] { FileMapping["ManagerFilePath"] }));

            container.RegisterType<IImportService, ImportService>(  new InjectionProperty("ProductFilePath", FileMapping["ProductFilePath"]),
                                                                    new InjectionProperty("ReservationFilePath", FileMapping["ReservationFilePath"]),
                                                                    new InjectionProperty("ReservationConfirmedFilePath", FileMapping["ReservationConfirmedFilePath"]),
                                                                    new InjectionProperty("AccountFilePath", FileMapping["AccountFilePath"]),
                                                                    new InjectionProperty("ManagerFilePath", FileMapping["ManagerFilePath"])
                                                                    );
        }
    }
}
