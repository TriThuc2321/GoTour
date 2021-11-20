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
    public class RevenueManagerViewModel :ObservableObject
    {
        INavigation navigation;
        Shell currentShell;

        private readonly IMessageService _messageService;
        public RevenueManagerViewModel() { }
        public RevenueManagerViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
            InitList();



       

        }
        private void InitList()
        {
            ListTour = new List<Tour>();
            ListInvoice = new List<Invoice>();
            ListBookedSticket = new ObservableCollection<BookedTicket>();
           
            foreach (BookedTicket ite in DataManager.Ins.ListBookedTickets)
            {
                if (ite.invoice.IsPaid == true) ListBookedSticket.Add(ite);
            }
            int a = 6;

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
