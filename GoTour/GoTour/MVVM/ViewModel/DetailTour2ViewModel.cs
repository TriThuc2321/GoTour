using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class DetailTour2ViewModel : ObservableObject
    {
        INavigation navigation;
        public DetailTour2ViewModel() { }
        public DetailTour2ViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            List<Place> temp = new List<Place>();
            foreach (Place ite in DataManager.Ins.ListPlace)
            {
                temp.Add(ite);
            }
            TourPlaces = temp.FindAll(e => DataManager.Ins.currentTour.placeDurationList.Exists(p => p.placeId == e.id));
            int c = 6;


        }

        private List<Place> tourPlace;
        public List<Place> TourPlaces
        {
            get { return tourPlace; }
            set
            {
                tourPlace = value;
                OnPropertyChanged("TourPlaces");
            }
        }
    }
}
