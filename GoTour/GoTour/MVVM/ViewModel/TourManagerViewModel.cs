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
    class TourManagerViewModel: ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public TourManagerViewModel() { }
        public TourManagerViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            ListTourManager = new ObservableCollection<Tour>();

            for (int i =0; i< DataManager.Ins.ListTour.Count; i++)
            {
                if (DataManager.Ins.ListTour[i].remaining == DataManager.Ins.ListTour[i].passengerNumber) ListTourManager.Add(DataManager.Ins.ListTour[i]);
            }

            NavigationBack = new Command(() => navigation.PopAsync());        
        }
        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            Tour result = obj as Tour;
            if (result != null)
            {
                DataManager.Ins.currentTour = result;
                navigation.PushAsync(new EditTourView());
                SelectedTour = null;
            }
        });
        public ICommand DeleteCommand => new Command<object>(async (obj) =>
        {
            Tour result = obj as Tour;
            if (result != null)
            {
                ListTourManager.Remove(result);
                DataManager.Ins.ListTour.Remove(result);
                await DataManager.Ins.TourServices.DeletePlace(result);
                await DataManager.Ins.TourPlaceServices.DeleteTourPlace(result.id);
            }
        });
        public ICommand NewTourCommand => new Command<object>((obj) =>
        {
            navigation.PushAsync(new NewTourView());
        });
        public Command NavigationBack { get; }

        private ObservableCollection<Tour> listTourManager;
        public ObservableCollection<Tour> ListTourManager
        {
            get { return listTourManager; }
            set
            {
                listTourManager = value;
                OnPropertyChanged("ListTourManager");
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

        
    }
}
