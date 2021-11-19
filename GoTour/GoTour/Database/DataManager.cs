﻿using GoTour.Core;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

        
       
        public List<User> users;
        public List<User> admins;
        public List<User> managements;
        public List<User> tourGuides;
        public List<User> customers;
        public List<ImagePlaceStream> imagePlaceStreams;
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
        public PlaceId_Duration currentDuration = new PlaceId_Duration();
        public int idTourDuration;
        private DataManager()
        {
            PlacesServices = new PlacesServices();
            UsersServices = new UsersServices();
            ReviewService = new ReviewService();
            ListPlace = new ObservableCollection<Place>();
            ListUser = new ObservableCollection<User>();
            ListTour = new ObservableCollection<Tour>();
            ListReview = new ObservableCollection<Review>();


            /// Thiên
            ListStayPlace = new ObservableCollection<StayPlace>();
            StayPlacesServices = new StayPlacesServices();
            TourPlaceServices = new TourPlaceServices();
            TourServices = new ToursServices();
            
            getAllStayPlaceList();


            /// Linh
            ListFavouriteTours = new ObservableCollection<FavouriteTour>();
            FavoritesServices = new FavoriteToursServices();

            ListBookedTickets = new ObservableCollection<BookedTicket>();
            BookedTicketsServices = new BookedTicketServices();

            ListDiscount = new ObservableCollection<Discount>();
            DiscountsServices = new DiscountsServices();

            ListInvoice = new ObservableCollection<Invoice>();
            InvoicesServices = new InvoicesServices();

            CurrentDiscount = new Discount();
            currentInvoice = new Invoice();

            CurrentUser = new User();
            
            getAllList();
        }
        async Task getAllList()
        {
            //await firebaseHelper.AddPlace("3", "VietName", "VN ne", "https://i.pinimg.com/564x/5a/41/04/5a41046452cc2481693ce2df3c93fbc4.jpg");

            users = await UsersServices.GetAllUsers();

            admins = new List<User>();
            managements = new List<User>();
            tourGuides = new List<User>();
            customers = new List<User>();
            foreach (User u in users)
            {
                ListUser.Add(u);
                if (u.rank == 3) customers.Add(u);
                else if (u.rank == 2) tourGuides.Add(u);
                else if (u.rank == 1) managements.Add(u);
                else admins.Add(u);
            }

            List<Place> temp = await PlacesServices.GetAllPlaces();
            foreach (Place p in temp)
            {
                ListPlace.Add(p);
            }

            

            List<Tour> tourList = await TourServices.GetAllTours();
            List<TourPlace> tourPlaceList = await TourPlaceServices.GetAllTourPlaces();
            List<Review> reviews = await ReviewService.GetAllReviews();
            foreach (Tour ite in tourList)
            {
                ListTour.Add(ite);
                TourPlace temp2 = tourPlaceList.Find(e => (e.tourId == ite.id));
                ite.placeDurationList = temp2.placeDurationList;
                ite.reviewList = reviews.FindAll(e => e.tourId == ite.id);
            }

            /*imagePlaceStreams = GetImageStreamPlaces();
            foreach (var ite in imagePlaceStreams)
            {
                ImgPlaceStreams.Add(ite);
            }*/

            //Linh
            List<FavouriteTour> favouriteToursList = await FavoritesServices.GetAllFavourite();
            foreach (FavouriteTour favourite in favouriteToursList)
            {
                favourite.tour = tourList.Find(e => (e.id == favourite.tour.id));
                ListFavouriteTours.Add(favourite);
            }

            List<Discount> discountsList = await DiscountsServices.GetAllDiscounts();
            foreach (Discount discount in discountsList)
            {
                ListDiscount.Add(discount);
            }

            List<Invoice> invoicesList = await InvoicesServices.GetAllInvoice();
            foreach (Invoice invoice in invoicesList)
            {
                if (invoice.discount != null)
                    invoice.discount = discountsList.Find(e => (e.id == invoice.discount.id));
                ListInvoice.Add(invoice);
            }

            List<BookedTicket> bookedTicketsList = await BookedTicketsServices.GetAllBookedTicket();
            foreach(BookedTicket booked in bookedTicketsList)
            {
                booked.tour = tourList.Find(e => (e.id == booked.tour.id));
                booked.invoice = invoicesList.Find(e => (e.id == booked.invoice.id));
                ListBookedTickets.Add(booked);
             
            }
        }
        public string GeneratePlaceId(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());

            int i =0;
            while (i< ListPlace.Count())
            {
                if(ListPlace[i].id == randomString)
                {
                    i = -1;
                    randomString = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
                }
                i++;
            }
            return randomString;
        }
        public string GenerateStayPlaceId(int length = 3)
        {
            const string chars = "0123456789";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());

            int i = 0;
            while (i < ListPlace.Count())
            {
                if (ListPlace[i].id == randomString)
                {
                    i = -1;
                    randomString = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
                }
                i++;
            }
            return "SP" + randomString;
        }
        public List<ImagePlaceStream> GetImageStreamPlaces()
        {
            List<ImagePlaceStream> temp = new List<ImagePlaceStream>();
            for (int i =0; i< ListPlace.Count(); i++)
            {
                List<Stream> streams = new List<Stream>();
                foreach (var p in ListPlace[i].imgSource)
                {                   
                    streams.Add(GetStreamFromUrl(p));
                }
                temp.Add(new ImagePlaceStream(ListPlace[i].id, streams));
            }

            return temp;
        }
        private MemoryStream GetStreamFromUrl(string url)
        {
            byte[] imageData = null;
            MemoryStream ms;

            ms = null;

            try
            {
                using (var wc = new System.Net.WebClient())
                {
                    imageData = wc.DownloadData(url);
                }
                ms = new MemoryStream(imageData);
            }
            catch (Exception ex)
            {

            }

            return ms;
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

        private FavoriteToursServices favoritesServices;
        public FavoriteToursServices FavoritesServices
        {
            get
            {
                return favoritesServices;
            }
            set { favoritesServices = value; }
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
        private ObservableCollection<Review> listReview;
        public ObservableCollection<Review> ListReview
        {
            get { return listReview; }
            set
            {
                listReview = value;
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
                CurrentName = value.name;
                if(value.rank == 0)
                {
                    IsManager = true;
                }
                else
                {
                    IsManager = false;
                }
            }
        }
        private string currentName;
        public string CurrentName
        {
            get { return currentName; }
            set
            {
                currentName = value;
                OnPropertyChanged("CurrentName");
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
        private string USDcurrency;
        public string USDCurrency
        {
            get { return USDcurrency; }
            set
            {
                USDcurrency = value;
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

        private ObservableCollection<FavouriteTour> favouriteTours;
        public ObservableCollection<FavouriteTour> ListFavouriteTours
        {
            get { return favouriteTours; }
            set
            {
                favouriteTours = value;
                OnPropertyChanged("ListFavouriteTours");
            }
        }

        private ObservableCollection<BookedTicket> bookedTicketList;
        public ObservableCollection<BookedTicket> ListBookedTickets
        {
            get { return bookedTicketList; }
            set
            {
                bookedTicketList = value;
                OnPropertyChanged("ListBookedTickets");
            }
        }


        private BookedTicketServices bookedTicketServices;
        public BookedTicketServices BookedTicketsServices
        {
            get
            {
                return bookedTicketServices;
            }
            set { bookedTicketServices = value; }
        }


        private ObservableCollection<Discount> discountList;
        public ObservableCollection<Discount> ListDiscount
        {
            get { return discountList; }
            set
            {
                discountList = value;
                OnPropertyChanged("ListDiscount");
            }
        }


        private DiscountsServices discountServices;
        public DiscountsServices DiscountsServices
        {
            get
            {
                return discountServices;
            }
            set { discountServices = value; }
        }

        private ObservableCollection<Invoice> invoiceList;
        public ObservableCollection<Invoice> ListInvoice
        {
            get { return invoiceList; }
            set
            {
                invoiceList = value;
                OnPropertyChanged("ListInvoice");
            }
        }


        private InvoicesServices invoiceServices;
        public InvoicesServices InvoicesServices
        {
            get
            {
                return invoiceServices;
            }
            set { invoiceServices = value; }
        }

        private BookedTicket currentBookedTicket;
        public BookedTicket CurrentBookedTicket
        {
            get { return currentBookedTicket; }
            set
            {
                currentBookedTicket = value;
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

        public ReviewService reviewService;
        public ReviewService ReviewService
        {
            get { return reviewService; }
            set
            {
                reviewService = value;
                OnPropertyChanged("ReviewService");
            }
        }

        private Place currentPlaceManager;
        public Place CurrentPlaceManager
        {
            get { return currentPlaceManager; }
            set
            {
                currentPlaceManager = value;
                OnPropertyChanged("CurrentPlaceManager");
            }
        }

        private StayPlace currentStayPlaceManager;
        public StayPlace CurrentStayPlaceManager
        {
            get { return currentStayPlaceManager; }
            set
            {
                currentStayPlaceManager = value;
                OnPropertyChanged("CurrentStayPlaceManager");
            }
        }
        private Invoice currentInvoice;
        public Invoice CurrentInvoice
        {
            get { return currentInvoice; }
            set
            {
                currentInvoice = value;
                OnPropertyChanged("CurrentInvoice");
            }
        }

        private bool isManager;
        public bool IsManager
        {
            get { return isManager; }
            set
            {
                isManager = value;
                OnPropertyChanged("IsManager");
            }
        }
        private ObservableCollection<ImagePlaceStream> imgPlaceStreams;
        public ObservableCollection<ImagePlaceStream> ImgPlaceStreams
        {
            get { return imgPlaceStreams; }
            set
            {
                imgPlaceStreams = value;
                OnPropertyChanged("ImgPlaceStreams");
            }
        }

        private Discount currentDiscount;
        public Discount CurrentDiscount
        {
            get { return currentDiscount; }
            set
            {
                currentDiscount = value;
             
            }
        }
    }


}
