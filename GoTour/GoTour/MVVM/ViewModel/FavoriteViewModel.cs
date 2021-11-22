using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class FavoriteViewModel : ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public Command MenuCommand { get; }

        public FavoriteViewModel() { }
        public FavoriteViewModel(INavigation navigation, Shell current)
        {
            this.navigation = navigation;
            this.currentShell = current;
            MenuCommand = new Command(openMenu);

            Favourites = new ObservableCollection<FavouriteTour>();
            foreach (var favourites in DataManager.Ins.ListFavouriteTours)
            {
                if (favourites.email == DataManager.Ins.CurrentUser.email)
                    Favourites.Add(favourites);
            }

            Refresh = new Command(RefreshFavourite);
        }

        private ObservableCollection<FavouriteTour> _favourites;
        public ObservableCollection<FavouriteTour> Favourites
        {
            get { return _favourites; }
            set
            {
                _favourites = value;
                OnPropertyChanged("Favourites");
            }
        }


        private Tour selectedTour;
        public Tour SelectedTour
        {
            get { return selectedTour; }
            set
            {
                selectedTour = value;
                OnPropertyChanged("SelectedTour");

            }
        }

        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            FavouriteTour result = obj as FavouriteTour;
            if (result != null)
            {
                DataManager.Ins.currentTour = result.tour;
                navigation.PushAsync(new DetailTourView());
                SelectedTour = null;
            }
        });

        public ICommand UnlovedCommand => new Command<object>((obj) =>
        {
            FavouriteTour result = obj as FavouriteTour;

            if (result != null)
            {
                //var decision = page.DisplayAlert("Delete Tour",
                //    "Are you sure to delete this tour?",
                //    "Delete", "Cancel");

                //if (!decision.Result) return;

                int index = Favourites.IndexOf(result);

              //  Favourites[index].love = "love_white.png";

                DataManager.Ins.FavoritesServices.DeleteFavoriteTour(result.id);
                DataManager.Ins.ListFavouriteTours.Remove(result);
                Favourites.Remove(result);
            }
        });

        private void openMenu(object obj)
        {
            currentShell.FlyoutIsPresented = !currentShell.FlyoutIsPresented;
        }
        #region Refresh
        private bool _isRefresh;
        public bool IsRefresh
        {
            get
            {
                return _isRefresh;
            }

            set
            {
                _isRefresh = value;
                OnPropertyChanged("IsRefresh");
            }
        }

        public Command Refresh { get; }
        void RefreshFavourite(object obj)
        {
            IsRefresh = true;
            Favourites.Clear();
            Favourites = new ObservableCollection<FavouriteTour>();
            foreach (var favourites in DataManager.Ins.ListFavouriteTours)
            {
                if (favourites.email == DataManager.Ins.CurrentUser.email)
                    Favourites.Add(favourites);
            }
            IsRefresh = false;
        }
        #endregion
    }
}
