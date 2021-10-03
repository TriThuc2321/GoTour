using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class ToursViewModel: ObservableObject
    {
        INavigation navigation;
        FirebaseHelper firebaseHelper;
        public ToursViewModel() { }
        public ToursViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            firebaseHelper = new FirebaseHelper();
            Places = new ObservableCollection<Place>();
            Places2 = new ObservableCollection<Place>();
            getPlacesAsync();
        }

        async Task getPlacesAsync()
        {
            //await firebaseHelper.AddPlace("3", "VietName", "VN ne", "https://i.pinimg.com/564x/5a/41/04/5a41046452cc2481693ce2df3c93fbc4.jpg");

            List<Place> temp = await firebaseHelper.GetAllPlaces();

            foreach (Place p in temp)
            {
                Places.Add(p);
            }
            foreach (Place p in temp)
            {
                Places2.Add(p);
            }
            int a = 5;
        }

        private ObservableCollection<Place> _places2;
        public ObservableCollection<Place> Places2
        {
            get { return _places2; }
            set
            {
                _places2 = value;
                OnPropertyChanged("Places2");
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
    }

   
}
