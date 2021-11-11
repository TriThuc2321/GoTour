using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;

namespace GoTour.MVVM.ViewModel
{
    class HomeViewModel: ObservableObject
    {       
        INavigation navigation;
        Shell currentShell;

        public Command MenuCommand { get; }
        public Command NotificaitonCommand { get; }
        public Command ToursCommand { get; }
        public Command FavoriteCommand { get; }
        public Command MyTourCommand { get; }

        public Command SearchButtonHandleCommand { get; }

        public HomeViewModel() { }
        public HomeViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            MenuCommand = new Command(openMenu);
            NotificaitonCommand = new Command(openNotifi);
            ToursCommand = new Command(openTours);
            FavoriteCommand = new Command(openFavorite);
            MyTourCommand = new Command(openMyTour);
            SearchButtonHandleCommand = new Command(searchButtonHandle);

            StayPlaces = DataManager.Ins.ListStayPlace;
            int a = 5;
           /* Places = DataManager.Ins.ListPlace;*/

            ProfilePic = DataManager.Ins.CurrentUser.profilePic;
           /* Tours = DataManager.Ins.ListTour;*/
            Places = DataManager.Ins.ListPlace;

        }

        private void searchButtonHandle()
        {
            if(string.IsNullOrWhiteSpace(PlaceToSearch))
            {
                DependencyService.Get<IToast>().ShortToast("Where do you want to find ?");
                return;
            }
            else
            {
                DataManager.Ins.SearchServices.RefreshDataSearch();
                DataManager.Ins.SearchServices.PlaceToSearch = PlaceToSearch;
                DataManager.Ins.SearchServices.GetResult();
                if (DataManager.Ins.SearchServices.ResultList.Count > 0)
                {
                    navigation.PushAsync(new SearchResultView());
                    //DependencyService.Get<IToast>().ShortToast(DataManager.Ins.SearchServices.ResultList.Count.ToString());
                }
                else
                {
                    DependencyService.Get<IToast>().ShortToast(" Sorry! At present, there is no tour that comes to the place you want.");
                    return;
                }
            }
        }

        #region open view
        private void openMenu(object obj)
        {
            currentShell.FlyoutIsPresented = !currentShell.FlyoutIsPresented;
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
        private ObservableCollection<StayPlace> _stayPlaces;
        public ObservableCollection<StayPlace> StayPlaces
        {
            get { return _stayPlaces; }
            set
            {
                _stayPlaces = value;
                OnPropertyChanged("StayPlaces");
            }
        }

        private ObservableCollection<Tour> _tours;
        public ObservableCollection<Tour> Tours
        {
            get { return _tours; }
            set
            {
                _tours = value;
                OnPropertyChanged("Tours");
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
        private string placeToSearch;
        public string PlaceToSearch
        {
            get { return placeToSearch; }
            set
            {
                placeToSearch = value;
                OnPropertyChanged("PlaceToSearch");
            }
        }
        
    }
}
