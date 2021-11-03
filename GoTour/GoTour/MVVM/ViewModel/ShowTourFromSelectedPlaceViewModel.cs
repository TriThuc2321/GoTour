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
    class ShowTourFromSelectedPlaceViewModel : ObservableObject
    {
        INavigation navigation;
        public Command NavigationBack { get; }

        private ObservableCollection<Tour> listTourFromSelectedPlace;
        public ObservableCollection<Tour> ListTourFromSelectedPlace
        {
            get { return listTourFromSelectedPlace; }
            set
            {
                listTourFromSelectedPlace = value;
                OnPropertyChanged("ListTourFromSelectedPlace");
            }
        }
        public ShowTourFromSelectedPlaceViewModel() { }

        private Tour selectedTour;
        public Tour SelectedTour
        {
            get { return selectedTour; }
            set
            {
                selectedTour = value;
                OnPropertyChanged("SelectedTour");

            }
        }

        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            Tour result = obj as Tour;
        if (result != null)
        {
            DataManager.Ins.currentTour = result;
                navigation.PushAsync(new DetailTourView());
                SelectedTour = null;
            }
        });

        public ShowTourFromSelectedPlaceViewModel(INavigation navigation)
        {
            ListTourFromSelectedPlace = new ObservableCollection<Tour>();
            NavigationBack = new Command(() => navigation.PopAsync());
            List<Place> temp1 = new List<Place>();
            List<Tour> temp2 = new List<Tour>();
            foreach (Place ite in DataManager.Ins.ListPlace)
            {
                temp1.Add(ite);
            }
            foreach (Tour ite in DataManager.Ins.ListTour)
            {
                temp2.Add(ite);
            }

            List<Tour> temp = new List<Tour>();
            this.navigation = navigation;
            foreach (var ite in DataManager.Ins.currentPlace)
            {
                temp = temp2.FindAll(e => e.placeDurationList.Exists(p => p.placeId == ite));
            }
         
            foreach (Tour ite3 in temp)
            {
                ListTourFromSelectedPlace.Add(ite3);
            }
        }
    }
}
