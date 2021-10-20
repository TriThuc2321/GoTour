using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class DetailTourViewModel
    {
        INavigation navigation;
        public DetailTourViewModel() { }
        public DetailTourViewModel(INavigation navigation)
        {
            this.navigation = navigation;
        }
    }
}
