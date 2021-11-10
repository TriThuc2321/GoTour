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

        public ICommand NewStayPlaceCommand => new Command<object>((obj) =>
        {
            navigation.PushAsync(new NewStayPlaceView());

        });
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
