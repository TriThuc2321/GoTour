using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class ConfirmInvoiceViewModel: ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public Command ConfirmCommand { get; }
        public ConfirmInvoiceViewModel() { }
        public ConfirmInvoiceViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
            InitList();
            

        }
        private void InitList()
        {
            ListTour = new List<Tour>();
            ListInvoice = new List<Invoice>();
            ListBookedSticket = DataManager.Ins.ListBookedTickets;
            foreach (Tour ite in DataManager.Ins.ListTour)
            {
                listTour.Add(ite);
            }
            foreach (Invoice ite1 in DataManager.Ins.ListInvoice)
            {
                listInvoice.Add(ite1);
            }
            foreach (BookedTicket ite in ListBookedSticket)
            {
                ite.tour = listTour.Find(e => e.id == ite.tour.id);
                ite.invoice = listInvoice.Find(e => e.id == ite.invoice.id);
                if (ite.invoice.isPaid == true) ListBookedSticket.Remove(ite);
            }
           
        }

        private ObservableCollection<BookedTicket> listBookedSticket;
        public ObservableCollection<BookedTicket> ListBookedSticket
        {
            get { return listBookedSticket; }
            set
            {
                listBookedSticket = value;
                OnPropertyChanged("ListBookedSticket");
            }
        }
        private List<Tour> listTour;
        public List<Tour> ListTour
        {
            get { return listTour; }
            set
            {
                listTour = value;
                OnPropertyChanged("ListTour");
            }
        }
        private List<Invoice> listInvoice;
        public List<Invoice> ListInvoice
        {
            get { return listInvoice; }
            set
            {
                listInvoice = value;
                OnPropertyChanged("ListInvoice");
            }
        }

    }

   
}
