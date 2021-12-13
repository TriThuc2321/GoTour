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
    class ReviewViewModel : ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;
        public Command NavigationBack { get; }
        public Command NewReviewCommand { get; }

        public ReviewViewModel() { }
        public ReviewViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.currentShell = curentShell;
            ReviewList = DataManager.Ins.ListReview;

            NavigationBack = new Command(() => currentShell.FlyoutIsPresented = !currentShell.FlyoutIsPresented);
            
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
    }
}
