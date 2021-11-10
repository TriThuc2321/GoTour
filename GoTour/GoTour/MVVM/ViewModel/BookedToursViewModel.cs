using System;
using System.Collections.Generic;
using System.Text;
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
        }
    }


}
