using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendoCoseCommerce.Models
{
    public class ReservationResumeViewModel
    {
        public ReservationResumeViewModel()
        {
            listaPrenotazioniAttive = new List<ReservationViewModel>();
            listaPrenotazioniConfermate = new List<ReservationConfirmedViewModel>();
            listaPrenotazioniEvase = new List<ReservationConfirmedViewModel>();
        }
        public IList<ReservationViewModel> listaPrenotazioniAttive { get; set; }
        public IList<ReservationConfirmedViewModel> listaPrenotazioniConfermate { get; set; }
        public IList<ReservationConfirmedViewModel> listaPrenotazioniEvase { get; set; }
    }
}