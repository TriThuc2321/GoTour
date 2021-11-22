using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class ConfirmInvoiceViewModel: ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public Command ConfirmCommand { get; }
        public Command NavigationBack { get; }
        public ConfirmInvoiceViewModel() { }
        public ConfirmInvoiceViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
            NavigationBack = new Command(() => currentShell.FlyoutIsPresented = !currentShell.FlyoutIsPresented);
            InitList();
            

        }
        private void InitList()
        {
            ListTour = new List<Tour>();
            ListInvoice = new List<Invoice>();
            ListBookedSticket = new ObservableCollection<BookedTicket>();
            foreach( BookedTicket ite in DataManager.Ins.ListBookedTickets)
            {
                if (ite.invoice.IsPaid == false) ListBookedSticket.Add(ite);
            }
        }
        public ICommand AcceptCommand => new Command<object>((obj) =>
        {
            BookedTicket selected = obj as BookedTicket;
            if (selected != null)
            {
                selected.invoice.IsPaid = true;
                DataManager.Ins.InvoicesServices.UpdateInvoice(selected.invoice);
                ListBookedSticket.Remove(selected);
            }

        });
        public ICommand DeclineCommand => new Command<object>((obj) =>
        {
            BookedTicket selected = obj as BookedTicket;
            if (selected != null)
            {
                string message = "Dear " + selected.name + ",\n\n" +
                "Thank you for using our service, you have just booked a ticket for the tour: '" + selected.tour.name + "', there was an error during the transaction processing, we have canceled your ticket, sorry for the inconvenience." +
                "\n\n---------------------------------------\nFor more information about our tour or refund policy, please contact:\n 0383303061 - Nguyen Khanh Linh\n or 0834344655 - Pham Vo Di Thien";
                string title = "Notification About " + selected.tour.id.ToUpper() + " Booked Sticket";
                DataManager.Ins.NotiServices.SendNoti(DataManager.Ins.GeneratePlaceId(), "System", selected.email, 1, message, title, selected.tour.id.ToString());
                ListBookedSticket.Remove(selected);
                DataManager.Ins.InvoicesServices.DeleteInvoice(selected.invoice.id);
                DataManager.Ins.BookedTicketsServices.DeleteBookedTicket(selected.id);

            }
        });
        public ICommand SelectedCommand => new Command<object>(async (obj) =>
        {
            BookedTicket selected = obj as BookedTicket;
            if (selected != null)
            {              
                DataManager.Ins.CurrentBookedTicket = selected;            
                navigation.PushAsync(new DetailInvoiceView());
                SelectedBookedSticket = null;
            }
        });

        private BookedTicket selectedBookedSticket;
        public BookedTicket SelectedBookedSticket
        {
            get { return selectedBookedSticket; }
            set
            {
                selectedBookedSticket = value;
                OnPropertyChanged("SelectedBookedSticket");
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
