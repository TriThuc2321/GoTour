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
    class EditDetailScheduleViewModel : ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public Command SaveCommand { get; }
        public EditDetailScheduleViewModel() { }
        public int index;
        public EditDetailScheduleViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            Description = DataManager.Ins.currentDuration.description;
            Host = DataManager.Ins.currentDuration.host;
            Places = DataManager.Ins.ListPlace;
            Day = DataManager.Ins.currentDuration.day;
            Night = DataManager.Ins.currentDuration.night;

            StayPlaces = DataManager.Ins.ListStayPlace;

            IsVisible = false;
            SaveCommand = new Command(savehandle);

            for(int i =0; i < DataManager.Ins.currentTour.placeDurationList.Count; i++)
            {
                if (DataManager.Ins.currentDuration == DataManager.Ins.currentTour.placeDurationList[i])
                {
                    index = i;
                    break;
                }
            }

            for(int i =0; i< DataManager.Ins.ListStayPlace.Count; i++)
            {
                if(DataManager.Ins.ListStayPlace[i].placeId == DataManager.Ins.currentTour.SPforPList[index].stayPlaceId)
                {
                    StayPlaceSelected = DataManager.Ins.ListStayPlace[i];
                }
            }
        }

        private async void savehandle(object obj)
        {
            IsVisible = false;

            DataManager.Ins.currentDuration.night = night;
            DataManager.Ins.currentDuration.day = day;
            DataManager.Ins.currentDuration.placeId = Host.id;
            DataManager.Ins.currentDuration.description = description;

            DataManager.Ins.currentTour.SPforPList[index].placeId = Host.id;
            DataManager.Ins.currentTour.SPforPList[index].stayPlaceId = StayPlaceSelected.id;

            DataManager.Ins.currentDuration.host = null;

            DataManager.Ins.currentTour.placeDurationList[index] = DataManager.Ins.currentDuration;


            await DataManager.Ins.TourServices.UpdateTour(DataManager.Ins.currentTour);


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
