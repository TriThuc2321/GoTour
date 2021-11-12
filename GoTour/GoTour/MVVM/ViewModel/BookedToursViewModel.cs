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
    }


}
