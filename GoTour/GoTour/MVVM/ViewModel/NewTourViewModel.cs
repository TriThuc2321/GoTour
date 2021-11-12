using GoTour.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class NewTourViewModel : ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public NewTourViewModel() { }
        public NewTourViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
        }
    }
}
