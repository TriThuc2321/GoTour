using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class FavoriteViewModel : ObservableObject
    {
        INavigation navigation;

        public Command UnlovedCommand { get; }


        public FavoriteViewModel()
        {
        }
        public FavoriteViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            // Tours = DataManager.Ins.ListPlace;

            //UnlovedCommand = new Command(unlovedPlace);

            //Love = "love_red.png";

            init();
        }

        void init()
        {
            FavouriteTour tour = new FavouriteTour()
            {
                name = "Dạo chơi Sóc Trăng",
                imgSource = "https://cdn3.ivivu.com/2019/09/chua-som-rong-soc-trang-ivivu-3.jpg",
                startTime = DateTime.Now.Date,
                days = 3,
                passengerNumber = 20
            };

            Tours = new ObservableCollection<FavouriteTour>();
            Tours.Add(tour);
        }

        private ObservableCollection<FavouriteTour> _tour;
        public ObservableCollection<FavouriteTour> Tours
        {
            get { return _tour; }
            set
            {
                _tour = value;
                OnPropertyChanged("Tours");
            }
        }

        //private string _love;
        //public string Love
        //{
        //    get { return _love; }
        //    set
        //    {
        //        _love = value;
        //        OnPropertyChanged("Love");
        //    }
        //}

        //private void unlovedPlace(object obj)
        //{
        //    _love = "love_white.png";
        //}

    }
}
