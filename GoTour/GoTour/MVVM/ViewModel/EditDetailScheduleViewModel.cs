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
    class EditDetailScheduleViewModel : ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public Command SaveCommand { get; }
        public EditDetailScheduleViewModel() { }
        public int index;
        bool flag = false;
        public EditDetailScheduleViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            Places = DataManager.Ins.ListPlace;
            StayPlaces = DataManager.Ins.ListStayPlace;
            IsVisible = false;
            SaveCommand = new Command(savehandle);
            index = DataManager.Ins.currentTour.placeDurationList.Count;

            if(DataManager.Ins.currentDuration!= null)
            {
                flag = true;
                Description = DataManager.Ins.currentDuration.description;
                Host = DataManager.Ins.currentDuration.host;

                Day = DataManager.Ins.currentDuration.day;
                Night = DataManager.Ins.currentDuration.night;

                for (int i = 0; i < DataManager.Ins.currentTour.placeDurationList.Count; i++)
                {
                    if (DataManager.Ins.currentDuration == DataManager.Ins.currentTour.placeDurationList[i])
                    {
                        index = i;
                        break;
                    }
                }

                for (int i = 0; i < DataManager.Ins.ListStayPlace.Count; i++)
                {
                    if (DataManager.Ins.ListStayPlace[i].placeId == DataManager.Ins.currentTour.SPforPList[index].stayPlaceId)
                    {
                        StayPlaceSelected = DataManager.Ins.ListStayPlace[i];
                    }
                }
            }
            
        }

        public ICommand NavigationBack => new Command<object>((obj) =>
        {
            navigation.PopAsync();
        });

        private async void savehandle(object obj)
        {
            IsVisible = false;
          
            if( Day == null || Night == null || Description == null || Host == null ||  Description == "")
            {
                DependencyService.Get<IToast>().ShortToast("Please fill out schedule information.");
                return;
            }
            if (flag)
            {
                DataManager.Ins.currentDuration.night = night;
                DataManager.Ins.currentDuration.day = day;
                DataManager.Ins.currentDuration.placeId = Host.id;
                DataManager.Ins.currentDuration.description = description;

                DataManager.Ins.currentTour.SPforPList[index].placeId = Host.id;
                DataManager.Ins.currentTour.SPforPList[index].stayPlaceId = StayPlaceSelected.id;
                DataManager.Ins.currentTour.placeDurationList[index] = DataManager.Ins.currentDuration;

                TourPlace tourPlace = new TourPlace(DataManager.Ins.currentTour.id, DataManager.Ins.currentTour.placeDurationList);

                await DataManager.Ins.TourPlaceServices.UpdateTourPlace(tourPlace);
                await DataManager.Ins.TourServices.UpdateTour(DataManager.Ins.currentTour);                
            }
            else
            {
                DataManager.Ins.currentDuration = new PlaceId_Duration();
                DataManager.Ins.currentDuration.night = night;
                DataManager.Ins.currentDuration.day = day;
                DataManager.Ins.currentDuration.placeId = Host.id;
                DataManager.Ins.currentDuration.description = description;

                if (DataManager.Ins.currentTour.SPforPList == null) DataManager.Ins.currentTour.SPforPList = new ObservableCollection<PlaceId_StayPlace>();
                if (DataManager.Ins.currentTour.placeDurationList == null) DataManager.Ins.currentTour.placeDurationList = new ObservableCollection<PlaceId_Duration>();

                DataManager.Ins.currentTour.SPforPList.Add(new PlaceId_StayPlace(Host.id, StayPlaceSelected.id));
                DataManager.Ins.currentTour.placeDurationList.Add(DataManager.Ins.currentDuration);

                TourPlace tourPlace = new TourPlace(DataManager.Ins.currentTour.id, DataManager.Ins.currentTour.placeDurationList);

                await DataManager.Ins.TourPlaceServices.UpdateTourPlace(tourPlace);
                await DataManager.Ins.TourServices.UpdateTour(DataManager.Ins.currentTour);
            }

            DependencyService.Get<IToast>().ShortToast("New schedule has been updated");


            navigation.RemovePage(navigation.NavigationStack[navigation.NavigationStack.Count - 2]);
            await navigation.PushAsync(new EditDetailTourView());            
            navigation.RemovePage(navigation.NavigationStack[navigation.NavigationStack.Count - 2]);
        }

        private Place host;
        public Place Host
        {
            get { return host; }
            set
            {
                host = value;
                IsVisible = true;
                OnPropertyChanged("Host");
            }
        }
        private StayPlace stayPlaceSelected;
        public StayPlace StayPlaceSelected
        {
            get { return stayPlaceSelected; }
            set
            {
                stayPlaceSelected = value;
                IsVisible = true;
                OnPropertyChanged("StayPlaceSelected");
            }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                IsVisible = true;
                OnPropertyChanged("Description");

            }
        }
        private bool isVisible;
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                OnPropertyChanged("IsVisible");

            }
        }
        private ObservableCollection<Place> places;
        public ObservableCollection<Place> Places
        {
            get { return places; }
            set
            {
                places = value;
                OnPropertyChanged("Places");
            }
        }
        private ObservableCollection<StayPlace> stayPlaces;
        public ObservableCollection<StayPlace> StayPlaces
        {
            get { return stayPlaces; }
            set
            {
                stayPlaces = value;
                OnPropertyChanged("StayPlaces");
            }
        }
        private int day;
        public int Day
        {
            get { return day; }
            set
            {
                day = value;
                IsVisible = true;
                OnPropertyChanged("Day");

            }
        }
        private int night;
        public int Night
        {
            get { return night; }
            set
            {
                night = value;
                IsVisible = true;
                OnPropertyChanged("Night");

            }
        }
    }
}
