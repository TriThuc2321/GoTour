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
    public class StayPlaceManagerViewModel : ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public StayPlaceManagerViewModel() { }
        public StayPlaceManagerViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            ListStayPlace = DataManager.Ins.ListStayPlace;
        }

        private StayPlace selectedStayPlace;
        public StayPlace SelectedStayPlace
        {
            get { return selectedStayPlace; }
            set
            {
                selectedStayPlace = value;
                OnPropertyChanged("SelectedStayPlace");

            }
        }
        private ObservableCollection<StayPlace> listStayPlace;
        public ObservableCollection<StayPlace> ListStayPlace
        {
            get { return listStayPlace; }
            set
            {
                listStayPlace = value;
                OnPropertyChanged("ListStayPlace");
            }
        }
    }
}
