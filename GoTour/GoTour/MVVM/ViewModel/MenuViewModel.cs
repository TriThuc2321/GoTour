using GoTour.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class MenuViewModel : ObservableObject
    {
        INavigation navigation;
        public MenuViewModel()
        {

        }
        public MenuViewModel(INavigation navigation)
        {
            this.navigation = navigation;
        }
    }
}
