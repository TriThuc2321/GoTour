using GoTour.Core;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GoTour.Database
{
    public class DataManager: ObservableObject
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

        
       
        public List<User> usersTemp;
        public bool LoadData = true;

        #region TourView
        
        async Task getAllStayPlaceList()
        {
            //await firebaseHelper.AddPlace("3", "VietName", "VN ne", "https://i.pinimg.com/564x/5a/41/04/5a41046452cc2481693ce2df3c93fbc4.jpg");

            List<StayPlace> temp = await StayPlacesServices.GetAllStayPlaces();
            foreach (StayPlace p in temp)
            {
                ListStayPlace.Add(p);
            }

        }
        private ObservableCollection<StayPlace> _stayPlaces;
        public ObservableCollection<StayPlace> ListStayPlace
        {
            get { return _stayPlaces; }
            set
            {
                _stayPlaces = value;
            }
        }
        #endregion

        public List<string> currentPlace = new List<string>();
        public Tour currentTour = new Tour();
        private DataManager()
        {
            PlacesServices = new PlacesServices();
            UsersServices = new UsersServices();
            ListPlace = new ObservableCollection<Place>();
            ListUser = new ObservableCollection<User>();
            ListTour = new ObservableCollection<Tour>();


            /// Thiên
            ListStayPlace = new ObservableCollection<StayPlace>();
            StayPlacesServices = new StayPlacesServices();
            TourPlaceServices = new TourPlaceServices();
            TourServices = new ToursServices();
            getAllStayPlaceList();



            CurrentUser = new User();
            getAllList();
        }
        async Task getAllList()
        {
            //await firebaseHelper.AddPlace("3", "VietName", "VN ne", "https://i.pinimg.com/564x/5a/41/04/5a41046452cc2481693ce2df3c93fbc4.jpg");

            usersTemp = await UsersServices.GetAllUsers();
            foreach (User u in usersTemp)
            {
                ListUser.Add(u);
            }

            List<Place> temp = await PlacesServices.GetAllPlaces();
            foreach (Place p in temp)
            {
                ListPlace.Add(p);
            }

            List<Tour> tourList = await TourServices.GetAllTours();
            List<TourPlace> tourPlaceList = await TourPlaceServices.GetAllTourPlaces();
            foreach (Tour ite in tourList)
            {
                ListTour.Add(ite);
                TourPlace temp2 = tourPlaceList.Find(e => (e.tourId == ite.id));
                ite.placeDurationList = temp2.placeDurationList;
            }
            int v = 5;
            
        }
        private ToursServices tourServices;
        public ToursServices TourServices
        {
            get
            {
                return tourServices;
            }
            set { tourServices = value; }
        }


        private StayPlacesServices stayPlacesServices;
        public StayPlacesServices StayPlacesServices
        {
            get
            {
                return stayPlacesServices;
            }
            set { stayPlacesServices = value; }
        }

        private PlacesServices placesServices;
        public PlacesServices PlacesServices
        {
            get
            {
                return placesServices;
            }
            set { placesServices = value; }
        }

        private TourPlaceServices tourPlaceServices;
        public TourPlaceServices TourPlaceServices
        {
            get
            {
                return tourPlaceServices;
            }
            set { tourPlaceServices = value; }
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

        private ObservableCollection<Tour> _tours;
        public ObservableCollection<Tour> ListTour
        {
            get { return _tours; }
            set
            {
                _tours = value;
            }
        }

        private ObservableCollection<User> _users;
        public ObservableCollection<User> ListUser
        {
            get { return _users; }
            set
            {
                _users = value;
            }
        }

        private User currentUser;
        public User CurrentUser
        {
            get { return currentUser; }
            set
            {
                currentUser = value;
                ProfilePic = value.profilePic;
            }
        }
        private string verifyCode;
        public string VerifyCode
        {
            get { return verifyCode; }
            set
            {
                verifyCode = value;
            }
        }
        public bool isRegister;
        public bool IsRegister
        {
            get { return isRegister; }
            set
            {
                isRegister = value;
            }
        }

        private string profilePic;
        public string ProfilePic
        {
            get { return profilePic; }
            set
            {
                profilePic = value;
                OnPropertyChanged("ProfilePic");
            }
        }

        public UsersServices usersServices;
        public UsersServices UsersServices
        {
            get { return usersServices; }
            set
            {
                usersServices = value;
                OnPropertyChanged("UsersServices");
            }
        }

    }


}
