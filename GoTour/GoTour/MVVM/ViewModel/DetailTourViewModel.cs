using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class DetailTourViewModel : ObservableObject
    {
        INavigation navigation;
        public Command OpenDetailTour { get; }
        public Command OpenDetailTour1 { get; }
        public Command NavigationBack { get; }

        public DetailTourViewModel() { }
        public DetailTourViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            selectedTour = DataManager.Ins.currentTour;
            NavigationBack = new Command(() => navigation.PopAsync());
            OpenDetailTour = new Command(OpenDetailTourHandler);
            OpenDetailTour1 = new Command(OpenDetailTourHandler1);


        }
        public void OpenDetailTourHandler()
        {
            navigation.PushAsync(new DetailTourView2());
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
    }
}
