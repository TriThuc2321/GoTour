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
    class BookedToursViewModel: ObservableObject
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

        }

        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            BookedTicket result = obj as BookedTicket;
            if (result != null)
            {
                DataManager.Ins.CurrentBookedTicket = result;
                DataManager.Ins.CurrentInvoice = result.invoice;
                if (result.invoice.discount != null)
                    DataManager.Ins.CurrentInvoice.discount = result.invoice.discount;
                DataManager.Ins.currentTour = result.tour;
                navigation.PushAsync(new BookedTicketDetailView());
                SelectedTicket = null;
            }
        });

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

        private Tour selectedTicket;
        public Tour SelectedTicket
        {
            get { return selectedTicket; }
            set
            {
                selectedTicket = value;
                OnPropertyChanged("SelectedTicket");

            }
        }

       
    }


}
