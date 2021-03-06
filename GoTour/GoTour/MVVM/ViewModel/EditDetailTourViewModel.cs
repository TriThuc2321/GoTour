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
    class EditDetailTourViewModel: ObservableObject
    {
        INavigation navigation;
        public EditDetailTourViewModel() { }
        public Command NavigationBack { get; }

        public EditDetailTourViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            NavigationBack = new Command(() => navigation.PopAsync());
            IsOldTour = true;

            if (!DataManager.Ins.currentTour.isOccured || DataManager.Ins.currentTour.remaining != DataManager.Ins.currentTour.passengerNumber)
            {
                IsOldTour = false;
            }

            if (DataManager.Ins.currentTour.placeDurationList == null) DataManager.Ins.currentTour.placeDurationList = new ObservableCollection<PlaceId_Duration>();

            foreach (Place ite in DataManager.Ins.ListPlace)
            {
                foreach (var ite2 in DataManager.Ins.currentTour.placeDurationList)
                {
                    if (ite2.placeId == ite.id)
                    {
                        ite2.host = ite;
                        break;
                    }
                }
            }
            SelectedTour = DataManager.Ins.currentTour;
            PlaceDurationsList = SelectedTour.placeDurationList;
                     
            // TourPlaces = temp.FindAll(e => DataManager.Ins.currentTour.placeDurationList.Exists(p => p.placeId == e.id));
            DurationProcess();
        }

        public ICommand DeleteCommand => new Command<object>(async (obj) =>
        {
            if (DataManager.Ins.currentTour.isOccured || DataManager.Ins.currentTour.remaining != DataManager.Ins.currentTour.passengerNumber)
            {
                DependencyService.Get<IToast>().LongToast("Tour is occured or tour has been booked");
                return;
            }
            var duration = obj as PlaceId_Duration;            
            SelectedTour.placeDurationList.Remove(duration);
            PlaceDurationsList.Remove(duration);
            DataManager.Ins.currentTour.placeDurationList.Remove(duration);


            int day = 0;
            int night = 0;
            for (int i = 0; i < SelectedTour.placeDurationList.Count; i++)
            {
                day += SelectedTour.placeDurationList[i].day;
                night += SelectedTour.placeDurationList[i].night;
            }
            DataManager.Ins.currentTour.duration = day + "/" + night;
            DurationProcess();

            TourPlace tourPlace = new TourPlace(DataManager.Ins.currentTour.id, SelectedTour.placeDurationList);
            await DataManager.Ins.TourPlaceServices.UpdateTourPlace(tourPlace);
            await DataManager.Ins.TourServices.UpdateTour(SelectedTour);

            DependencyService.Get<IToast>().ShortToast("Schedule has been deleted");
        });

        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            if (!DataManager.Ins.currentTour.isOccured || DataManager.Ins.currentTour.remaining != DataManager.Ins.currentTour.passengerNumber)
            {
                DependencyService.Get<IToast>().LongToast("Tour is occured or tour has been booked");
                return;
            }

            PlaceId_Duration result = obj as PlaceId_Duration;
            
            if (result != null)
            {
                DataManager.Ins.currentDuration = result;
                navigation.PushAsync(new EditDetailScheduleView());
                SelectedDuration = null;
            }

        });

        public ICommand SaveCommand => new Command<object>(async (obj) =>
        {
            await DataManager.Ins.TourServices.AddTour(DataManager.Ins.currentTour);
        });

        public ICommand NewScheduleCommand => new Command<object>((obj) =>
        {
            DataManager.Ins.currentDuration = null;
            navigation.PushAsync(new EditDetailScheduleView());
        });

        private PlaceId_Duration selectedDuration;
        public PlaceId_Duration SelectedDuration
        {
            get { return selectedDuration; }
            set
            {
                selectedDuration = value;
                OnPropertyChanged("SelectedDuration");
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

        private bool isOldTour;
        public bool IsOldTour
        {
            get { return isOldTour; }
            set
            {
                isOldTour = value;
                OnPropertyChanged("IsOldTour");
            }
        }

        private ObservableCollection<PlaceId_Duration> placeDurationsList;
        public ObservableCollection<PlaceId_Duration> PlaceDurationsList
        {
            get { return placeDurationsList; }
            set
            {
                placeDurationsList = value;
                OnPropertyChanged("PlaceDurationsList");
            }
        }

        private List<Place> tourPlace;
        public List<Place> TourPlaces
        {
            get { return tourPlace; }
            set
            {
                tourPlace = value;
                OnPropertyChanged("TourPlaces");
            }
        }
        private string processedDuration;
        public string ProcessedDuration
        {
            get { return processedDuration; }
            set
            {
                processedDuration = value;
                OnPropertyChanged("ProcessedDuration");
            }
        }

        private void DurationProcess()
        {
            if (DataManager.Ins.currentTour.duration == null) return;
            string[] _ProcessedDuration = DataManager.Ins.currentTour.duration.Split('/');
            string result = _ProcessedDuration[0] + " Day(s) " + _ProcessedDuration[1] + " Night(s)";
            ProcessedDuration = result;
        }
    }
}
