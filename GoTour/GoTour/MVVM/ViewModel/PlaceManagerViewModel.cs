using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class PlaceManagerViewModel: ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public PlaceManagerViewModel() { }
        public PlaceManagerViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            ListPlace = DataManager.Ins.ListPlace;
        }

        public ICommand DeleteCommand => new Command<object>((obj) =>
        {
            var place = obj as Place;
            
            ListPlace.Remove(place);
        });
        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            Place result = obj as Place;
            if(result != null)
            {
                DataManager.Ins.CurrentPlaceManager = result;
                navigation.PushAsync(new EditPlaceView());
            }      
            SelectedPlace = null;
        });

        public ICommand NewPlaceCommand => new Command<object>((obj) =>
        {
            navigation.PushAsync(new NewPlaceView());

        });
        private Place selectedPlace;
        public Place SelectedPlace
        {
            get { return selectedPlace; }
            set
            {
                selectedPlace = value;
                OnPropertyChanged("SelectedPlace");

            }
        }
        private ObservableCollection<Place> listPlace;
        public ObservableCollection<Place> ListPlace
        {
            get { return listPlace; }
            set
            {
                listPlace = value;
                OnPropertyChanged("ListPlace");
            }
        }
    }
}
