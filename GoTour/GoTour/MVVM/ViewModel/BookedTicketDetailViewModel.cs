using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class BookedTicketDetailViewModel
    {
        INavigation navigation;

        public BookedTicketDetailViewModel()
        {

        }

        public BookedTicketDetailViewModel(INavigation navigation)
        {
            this.navigation = navigation;
        }
    }
}
