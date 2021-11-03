using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class ManagerViewModel
    {
        INavigation navigation;
        Shell curentShell;

        public Command PlaceCommand { get; }

        public ManagerViewModel() { }
        public ManagerViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.curentShell = curentShell;

            PlaceCommand = new Command(placeHandle);
        }

        private void placeHandle(object obj)
        {
            navigation.PushAsync(new PlaceManagerView());
        }
    }
}
