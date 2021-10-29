using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class InProgressViewModel
    {
        INavigation navigation;
        public InProgressViewModel()
        {

        }

        public InProgressViewModel(INavigation navigation)
        {
            this.navigation = navigation;
        }
    }
}
