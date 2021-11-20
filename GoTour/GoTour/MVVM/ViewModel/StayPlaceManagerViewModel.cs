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
        public ICommand DeleteCommand => new Command<object>( (obj) =>
        {
            if (selectedStayPlace == null) return;
            List<Tour> listTour = new List<Tour>();
            foreach (Tour ite in DataManager.Ins.ListTour)
                listTour.Add(ite);

            List<Tour> tour_Has_electedStayPlace_List = new List<Tour>();
            tour_Has_electedStayPlace_List = listTour.FindAll(e => e.SPforPList.Exists(p => p.stayPlaceId == selectedStayPlace.id));

            if (tour_Has_electedStayPlace_List.Count == 0)
            {
                DataManager.Ins.StayPlacesServices.DeleteStayPlace(selectedStayPlace);
                ListStayPlace.Remove(SelectedStayPlace);
                DependencyService.Get<IToast>().ShortToast("Delete Successful!");
            }
            else
            {
                string message = "Các tour: ";
                foreach (Tour ite in tour_Has_electedStayPlace_List)
                    message = message + ite.name + ", ";
                DependencyService.Get<IToast>().LongToast(message + " has selected StayPlace, Please delete before doing this task!");
            }

        });
        public ICommand DeleteCommand1 => new Command<object>((obj) =>
        {
            StayPlace selected = (StayPlace)obj;
            if (selected == null) return;
            List<Tour> listTour = new List<Tour>();
            foreach (Tour ite in DataManager.Ins.ListTour)
                listTour.Add(ite);
            List<Tour> tour_Has_electedStayPlace_List = new List<Tour>();
            tour_Has_electedStayPlace_List = listTour.FindAll(e => e.SPforPList.Exists(p => p.stayPlaceId == selected.id));

            if (tour_Has_electedStayPlace_List.Count == 0)
            {
                DataManager.Ins.StayPlacesServices.DeleteStayPlace(selected);
                ListStayPlace.Remove(selected);
                DependencyService.Get<IToast>().ShortToast("Delete Successful!");
                selectedStayPlace = null;
            }
            else
            {
                string message = "Các tour: ";
                foreach (Tour ite in tour_Has_electedStayPlace_List)
                    message = message + ite.name + ", ";
                DependencyService.Get<IToast>().LongToast(message + " has selected StayPlace, Please delete before doing this task!");
                selectedStayPlace = null;
            }

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
