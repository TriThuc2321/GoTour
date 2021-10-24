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
    class FavoriteViewModel: ObservableObject
    {
        INavigation navigation;

        public Command UnlovedCommand { get; }

        
        public FavoriteViewModel() {
        }
        public FavoriteViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            Places = DataManager.Ins.ListPlace;

            UnlovedCommand = new Command(unlovedPlace);

            Love = "love_red.png";
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

        private string _love;
        public string Love
        {
            get { return _love; }
            set
            {
                _love = value;
                OnPropertyChanged("Love");
            }
        }

        private void unlovedPlace(object obj)
        {
            _love = "love_white.png";
        }

    }
}
