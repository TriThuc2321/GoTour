using GoTour.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class SearchViewModel : ObservableObject
    {
        INavigation navigation;
        public SearchViewModel() { }
        public SearchViewModel(INavigation navigation)
        {
            this.navigation = navigation;
        }
    }
}
