using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class ReviewViewModel : ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;
        public Command OpenAddReview { get; }
        public Command NavigationBack { get; }
        public Command NewReviewCommand { get; }

        public ReviewViewModel() { }
        public ReviewViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.currentShell = curentShell;
            ReviewList = DataManager.Ins.ListReview;

            int s = 0;
            foreach(var r in DataManager.Ins.ListReview)
            {
                s += r.starNumber;
            }
            StarAverage = (double)s / DataManager.Ins.ListReview.Count;
            StarAverage = Math.Round(StarAverage, 1);

            OpenAddReview = new Command(openAddReview);
            NavigationBack = new Command(() => currentShell.FlyoutIsPresented = !currentShell.FlyoutIsPresented);
        }

        void openAddReview()
        {
            navigation.PushAsync(new SendReviewView());
        }
        private ObservableCollection<Review> reviewList;
        public ObservableCollection<Review> ReviewList
        {
            get { return reviewList; }
            set
            {
                reviewList = value;
                OnPropertyChanged("ReviewList");
            }
        }
        private double starAverage;
        public double StarAverage
        {
            get { return starAverage; }
            set
            {
                starAverage = value;
                OnPropertyChanged("StarAverage");
            }
        }
    }
}
