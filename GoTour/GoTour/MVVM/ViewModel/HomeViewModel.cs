using GoTour.Core;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class HomeViewModel: ObservableObject
    {
        INavigation navigation;

        public Command MenuCommand { get; }
        public Command NotificaitonCommand { get; }
        public Command ToursCommand { get; }
        public Command FavoriteCommand { get; }
        public Command MyTourCommand { get; }

        public HomeViewModel() { }
        public HomeViewModel(INavigation navigation)
        {
            this.navigation = navigation;

            MenuCommand = new Command(openMenu);
            NotificaitonCommand = new Command(openNotifi);
            ToursCommand = new Command(openTours);
            FavoriteCommand = new Command(openFavorite);
            MyTourCommand = new Command(openMyTour);

            Places = new ObservableCollection<Place>();

            addData();
        }
        #region open view
        private void openMenu(object obj)
        {
            navigation.PushAsync(new MenuView());
        }
        private void openNotifi(object obj)
        {
            navigation.PushAsync(new NotificationView());
        }
        private void openTours(object obj)
        {
            navigation.PushAsync(new ToursView());
        }
        private void openFavorite(object obj)
        {
            navigation.PushAsync(new FavoriteView());
        }
        private void openMyTour(object obj)
        {
            navigation.PushAsync(new MyTourView());
        }
        #endregion
        void addData()
        {
            Places.Add(new Place
            {
                id = "0",
                country = "VietName",
                title = "VN ne",
                imgSourse = "https://i.pinimg.com/564x/5a/41/04/5a41046452cc2481693ce2df3c93fbc4.jpg"

            });
            Places.Add(new Place
            {
                id = "1",
                country = "Finland",
                title = "Glass Igloo",
                imgSourse = "https://i.pinimg.com/564x/fb/e7/be/fbe7be68ba8a8bed4fa3d132b9fe9752.jpg"

            });
            Places.Add(new Place
            {
                id = "0",
                country = "Italian",
                title = "Tress House",
                imgSourse = "https://i.pinimg.com/236x/01/e4/9c/01e49c7e77ea91983f041b4b4de61eb5.jpg"

            });


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
