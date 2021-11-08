using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class ManagerViewModel
    {
        public INavigation navigation;
        public Shell currentShell;

        public Command PlaceCommand { get; }
        public Command MenuCommand { get; }

        public ManagerViewModel() { }
        public ManagerViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.currentShell = curentShell;

            PlaceCommand = new Command(placeHandle);
            MenuCommand = new Command(openMenu);
        }

        private void openMenu(object obj)
        {
            currentShell.FlyoutIsPresented = !currentShell.FlyoutIsPresented;
        }

        private void placeHandle(object obj)
        {
            navigation.PushAsync(new PlaceManagerView());
        }
    }
}
