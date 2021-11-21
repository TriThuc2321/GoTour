using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class BookedToursViewModel
    {
        INavigation navigation;

        public BookedToursViewModel()
        {

        }

        public BookedToursViewModel(INavigation navigation)
        {
            this.navigation = navigation;

            BookedTicketsList = new ObservableCollection<BookedTicket>();

            foreach (var tk in DataManager.Ins.ListBookedTickets)
            {
                if (tk.email == DataManager.Ins.CurrentUser.email)
                    BookedTicketsList.Add(tk);
            }

            SortingTicket();

        }

        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            BookedTicket result = obj as BookedTicket;
            if (result != null)
            {
                if (SelectedTicket.invoice.isPaid)

                DataManager.Ins.CurrentBookedTicket = result;
                DataManager.Ins.CurrentInvoice = result.invoice;
                if (result.invoice.discount != null)
                    DataManager.Ins.CurrentDiscount = result.invoice.discount;
                DataManager.Ins.currentTour = result.tour;

                
                navigation.PushAsync(new BookedTicketDetailView());

                SelectedTicket = null;
            }
        });

        private BookedTicket selectedTicket;
        public BookedTicket SelectedTicket
        {
            get { return selectedTicket; }
            set
            {
                selectedTicket = value;
                OnPropertyChanged("SelectedTicket");

            }
        }

        private string payment;
        public string Payment
        {
            get { return payment; }
            set
            {
                payment = value;
                OnPropertyChanged("Payment");

            }
        }

        private ObservableCollection<BookedTicket> _bookedTicketsList;
        public ObservableCollection<BookedTicket> BookedTicketsList
        {
            get { return _bookedTicketsList; }
            set
            {
                _bookedTicketsList = value;
                OnPropertyChanged("BookedTicketsList");
            }
        }

        void SortingTicket()
        {
            // Xep giam dan
            for (int i = 0; i < BookedTicketsList.Count; i++)
            {
                for (int j = i + 1; j < BookedTicketsList.Count - 1; j++)
                {
                    //string[] datetimeI = BookedTicketsList[i].bookTime.Split(' ');
                    //string[] datetimeJ = BookedTicketsList[j].bookTime.Split(' ');

                    //datetimeI
                    string datetimeI = BookedTicketsList[i].bookTime;
                    string datetimeJ = BookedTicketsList[j].bookTime;

                    if (DateTime.Parse(datetimeI) < DateTime.Parse(datetimeJ))
                    {
                        BookedTicket tmp = new BookedTicket();
                        tmp = BookedTicketsList[i];
                        BookedTicketsList[i] = BookedTicketsList[j];
                        BookedTicketsList[j] = tmp;
                    }
                }
            }
        }


    }


}
