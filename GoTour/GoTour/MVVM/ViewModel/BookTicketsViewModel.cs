using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class BookTicketsViewModel
    {
        INavigation navigation;

        public BookTicketsViewModel(INavigation navigation)
        {
            this.navigation = navigation;
        }
    }


}
