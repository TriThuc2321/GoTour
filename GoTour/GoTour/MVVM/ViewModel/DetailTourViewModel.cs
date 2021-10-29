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

        public DetailTourViewModel() { }
        public DetailTourViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            selectedTour = DataManager.Ins.currentTour;
            OpenDetailTour = new Command(OpenDetailTourHandler);


        }
        public void OpenDetailTourHandler()
        {
            navigation.PushAsync(new DetailTourView2());
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
