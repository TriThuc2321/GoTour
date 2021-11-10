using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class BookedTicketDetailViewModel
    {
        INavigation navigation;
        public BookedTicket ticket { get; }
        public Command NavigationBack { get; }
        public BookedTicketDetailViewModel()
        {

        }

        public BookedTicketDetailViewModel(INavigation navigation, BookedTicket ticket)
        {
            this.navigation = navigation;
            this.ticket = ticket;


            NavigationBack = new Command(() => navigation.PopAsync());

        }
    }
}
