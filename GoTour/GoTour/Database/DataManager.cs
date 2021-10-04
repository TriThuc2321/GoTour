using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace GoTour.Database
{
    public class DataManager
    {
        private static DataManager _ins;
        public static DataManager Ins
        {
            get
            {
                if (_ins == null) _ins = new DataManager();
                return _ins;
            }
            set { _ins = value; }
        }
        FirebaseHelper firebaseHelper;
        public PlacesServices PlacesServices { get; set; }
        public UsersServices PsersServices { get; set; }
        private DataManager()
        {
            PlacesServices = new PlacesServices();
            PsersServices = new UsersServices();
            ListPlace = new ObservableCollection<Place>();
            firebaseHelper = new FirebaseHelper();

            getAsync();
        }
        async Task getAsync()
        {
            //await firebaseHelper.AddPlace("3", "VietName", "VN ne", "https://i.pinimg.com/564x/5a/41/04/5a41046452cc2481693ce2df3c93fbc4.jpg");

            List<Place> temp = await firebaseHelper.GetAllPlaces();
            foreach (Place p in temp)
            {
                ListPlace.Add(p);
            }

        }

        private ObservableCollection<Place> _places;
        public ObservableCollection<Place> ListPlace
        {
            get { return _places; }
            set
            {
                _places = value;
            }
        }
    }
}
