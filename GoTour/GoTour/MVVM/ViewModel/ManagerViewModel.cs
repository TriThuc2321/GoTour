﻿using GoTour.MVVM.View;
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
        public Command RevenueCommand { get; }

        public ManagerViewModel() { }
        public ManagerViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.currentShell = curentShell;

            PlaceCommand = new Command(placeHandle);
            StayPlaceCommand = new Command(stayPlaceHandle);
            MenuCommand = new Command(openMenu);
            TourCommand = new Command(tourHandle);
            RevenueCommand = new Command(revenueHandle);
        }

        private void tourHandle(object obj)
        {
            navigation.PushAsync(new TourManagerView());
        }

        private void stayPlaceHandle(object obj)
        {
            navigation.PushAsync(new StayPlaceManagerView());
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
    }
}
