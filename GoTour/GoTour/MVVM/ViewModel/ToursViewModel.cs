using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class ToursViewModel : ObservableObject
    {
        INavigation navigation;
        public Command NavigationBack { get; }
        public Command OpenToursFromSelectedPlaces { get; }
        public ToursViewModel() { }
        public ToursViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            Places = new ObservableCollection<Place>();
            OpenToursFromSelectedPlaces = new Command(MutipleSelectedHandler);
            NavigationBack = new Command(() => navigation.PopAsync());
            getPlacesAsync();
        }
        public List<Place> result = new List<Place>();

        public ICommand MultipleSelectedCommand => new Command<IList<object>>((obj) =>
        {

            result = new List<Place>();
            foreach (var item in obj)
            {
                var selectedItem = item as Place;
                result.Add(selectedItem);
            }
            SelecteNumber = result.Count.ToString();

        });



        public void MutipleSelectedHandler()
        {
            DataManager.Ins.currentPlace.Clear();
            foreach ( var ite in result)
            {
                DataManager.Ins.currentPlace.Add(ite.id);
            }
            if (result.Count == 0)
            {
                foreach (var ite in DataManager.Ins.ListPlace)
                {
                    DataManager.Ins.currentPlace.Add(ite.id);
                }
            }
            OpenDetailTourView();
        }
        public void OpenDetailTourView()
        {
            navigation.PushAsync(new ShowTourFromSelectedPlace());
        }

        async Task getPlacesAsync()
        {
            ObservableCollection<Place> temp = DataManager.Ins.ListPlace;

            foreach (Place p in temp)
            {
                Places.Add(p);
            }

        }

      
        private ObservableCollection<Place> _places;

        public ObservableCollection<Place> Places
        {
            get { return _places; }
            set
            {
                _places = value;
                OnPropertyChanged("Places");
            }
        }

        private string selecteNumber = "All";

        public string SelecteNumber
        {
            get { return selecteNumber; }
            set
            {
                if (value == "0")
                    selecteNumber = "All";
                else
                    selecteNumber = value;
                OnPropertyChanged("SelecteNumber");

            }
        }


    }
}
