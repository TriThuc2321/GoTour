using GoTour.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class ToursViewModel: ObservableObject
    {
        INavigation navigation;
        public ToursViewModel() { }
        public ToursViewModel(INavigation navigation)
        {
            this.navigation = navigation;
        }
    }
}
