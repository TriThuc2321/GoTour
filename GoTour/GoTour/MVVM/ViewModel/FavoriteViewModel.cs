using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class FavoriteViewModel
    {
        INavigation navigation;
        public FavoriteViewModel() { }
        public FavoriteViewModel(INavigation navigation)
        {
            this.navigation = navigation;
        }
    }
}
