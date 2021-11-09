using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class SearchResultViewModel : ObservableObject
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

        public SearchResultViewModel() { }

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
        public SearchResultViewModel(INavigation navigation)
        {
            ListTourFromSelectedPlace = new ObservableCollection<Tour>();
            NavigationBack = new Command(() => navigation.PopAsync());
            this.navigation = navigation;
            foreach (Tour ite in DataManager.Ins.SearchServices.ResultList)
            {
                ListTourFromSelectedPlace.Add(ite);
            }
        }
    }
}
