﻿using GoTour.MVVM.Model;
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

        public UsersServices UsersServices;
       
        public List<User> usersTemp;
        public bool LoadData = true;

        #region TourView
        public StayPlacesServices StayPlacesServices;
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

        public String currentPlace;
        private DataManager()
        {
            PlacesServices = new PlacesServices();
            UsersServices = new UsersServices();
            ListPlace = new ObservableCollection<Place>();
            ListUser = new ObservableCollection<User>();


            /// Thiên
            ListStayPlace = new ObservableCollection<StayPlace>();
            StayPlacesServices = new StayPlacesServices();
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
        /*private UsersServices usersServices;
        public UsersServices UsersServices
        {
            get
            {
                return usersServices;
            }
            set { usersServices = value; }
        }*/
       
        private ObservableCollection<Place> _places;
        public ObservableCollection<Place> ListPlace
        {
            get { return _places; }
            set
            {
                _places = value;
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
    }


}
