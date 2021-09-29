using GoTour.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class MyTourViewModel: ObservableObject
    {
        INavigation navigation;
        public MyTourViewModel() { }
        public MyTourViewModel(INavigation navigation)
        {
            this.navigation = navigation;
        }
    }
}
