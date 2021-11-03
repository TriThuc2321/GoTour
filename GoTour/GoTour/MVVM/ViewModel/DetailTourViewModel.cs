using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class DetailTourViewModel : ObservableObject
    {
        INavigation navigation;
        public Command OpenDetailTour { get; }
        public Command BookTourCommand { get; }
        public Command OpenDetailTour1 { get; }
        public Command NavigationBack { get; }

        public DetailTourViewModel() { }
        public DetailTourViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            selectedTour = DataManager.Ins.currentTour;

            BookTourCommand = new Command(OpenBookTourView);
            FavouriteTour temp = CheckLoved();
            NavigationBack = new Command(() => navigation.PopAsync());
            OpenDetailTour = new Command(OpenDetailTourHandler);
            OpenDetailTour1 = new Command(OpenDetailTourHandler1);


        }
        public void OpenDetailTourHandler()
        {
            navigation.PushAsync(new DetailTourView2());
        }

        public void OpenBookTourView(object obj)
        {
            navigation.PushAsync(new BookTourView());
        }

        public void OpenDetailTourHandler1()
        {
            navigation.PushAsync(new TourScheduleView());
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


        #region FavouriteTour
        private string loveBtn;
        public string LoveBtn
        {
            get 
            {
                //if (CheckLoved() != null) 
                //    return "heartIcon.png";
                return loveBtn ;
            }
            set
            {
                loveBtn = value;
                OnPropertyChanged("LoveBtn");
            }
        }

        //Check the tour exist in FavouriteList
        FavouriteTour CheckLoved()
        {
            foreach (var favourite in DataManager.Ins.ListFavouriteTours)
            {
                if (favourite.tour.id == DataManager.Ins.currentTour.id)
                {
                    LoveBtn = "love_red.png";
                    return favourite;
                }
            }
            // LoveBtn = "love_white.png";
            LoveBtn = "heartIcon.png";
            return null;
        }

        public ICommand LoveCommand => new Command<object>((obj) =>
        {
            FavouriteTour check = CheckLoved();
            //Love
            if (check == null)
            {
                FavouriteTour favourite = new FavouriteTour
                {
                    tour = DataManager.Ins.currentTour,
                    email = DataManager.Ins.CurrentUser.email,
                    id = (new Random().Next(999999)).ToString()
                };


                DataManager.Ins.FavoritesServices.AddFavouriteTour(favourite);
                DataManager.Ins.ListFavouriteTours.Add(favourite);

                LoveBtn = "love_red.png";
            }
            else
            {
                //Unloved
                DataManager.Ins.FavoritesServices.DeleteFavoriteTour(check.id);
                DataManager.Ins.ListFavouriteTours.Remove(check);

                //LoveBtn = "love_white.png";
                LoveBtn = "heartIcon.png";

            }


        });

        #endregion
    }

}
