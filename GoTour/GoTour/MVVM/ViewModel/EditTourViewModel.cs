using GoTour.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class EditTourViewModel: ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public EditTourViewModel() { }
        public EditTourViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
        }
    }
}
