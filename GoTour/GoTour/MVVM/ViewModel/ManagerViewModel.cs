using GoTour.Database;
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
        public Command StayPlaceCommand { get; }
        public Command TourCommand { get; }
        public Command MenuCommand { get; }
        public Command StaffCommand { get; }
        public Command RevenueCommand { get; }
        public Command DiscountCommand { get; }

        public ManagerViewModel() { }
        public ManagerViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.currentShell = curentShell;

            IsAdmin = false;
            if(DataManager.Ins.CurrentUser.rank == 0)
            {
                IsAdmin = true;
            }
            

            PlaceCommand = new Command(placeHandle);
            StayPlaceCommand = new Command(stayPlaceHandle);
            MenuCommand = new Command(openMenu);
            TourCommand = new Command(tourHandle);
            StaffCommand = new Command(staffHandle);
            RevenueCommand = new Command(revenueHandle);
            DiscountCommand = new Command(discountHandle);
        }

        private void staffHandle(object obj)
        {
            navigation.PushAsync(new StaffManagerView());
           
        }

        private void tourHandle(object obj)
        {
            navigation.PushAsync(new TourManagerView());
        }

        private void stayPlaceHandle(object obj)
        {
            navigation.PushAsync(new StayPlaceManagerView());
        }

        private void discountHandle (object obj)
        {
            navigation.PushAsync(new DiscountManagerView());
        }

        private void revenueHandle(object obj)
        {
            navigation.PushAsync(new RevenueManagerView());
        }


        private void openMenu(object obj)
        {
            currentShell.FlyoutIsPresented = !currentShell.FlyoutIsPresented;
        }

        private void placeHandle(object obj)
        {
            navigation.PushAsync(new PlaceManagerView());
        }
        private bool isAdmin;
        public bool IsAdmin { 
            get
            {
                return isAdmin;
            }
            set
            {
                isAdmin = value;
            }
        }
    }
}
