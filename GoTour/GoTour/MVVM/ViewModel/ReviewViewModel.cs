using GoTour.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class ReviewViewModel : ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;
        public Command NavigationBack { get; }

        public ReviewViewModel() { }
        public ReviewViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.currentShell = curentShell;

        }
    }
}
