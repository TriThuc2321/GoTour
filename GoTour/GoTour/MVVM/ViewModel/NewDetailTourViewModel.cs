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
    class NewDetailTourViewModel : ObservableObject
    {
        INavigation navigation;
        public NewDetailTourViewModel() { }
        public Command NavigationBack { get; }

        public NewDetailTourViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            NavigationBack = new Command(() => navigation.PopAsync());

            if (DataManager.Ins.currentTour.placeDurationList == null) DataManager.Ins.currentTour.placeDurationList = new List<PlaceId_Duration>();

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
            PlaceDurationsList = new ObservableCollection<PlaceId_Duration>();
            foreach (var item in SelectedTour.placeDurationList)
            {
                PlaceDurationsList.Add(item);
            }

            // TourPlaces = temp.FindAll(e => DataManager.Ins.currentTour.placeDurationList.Exists(p => p.placeId == e.id));
            DurationProcess();
        }

        public ICommand DeleteCommand => new Command<object>(async (obj) =>
        {
            var duration = obj as PlaceId_Duration;
            SelectedTour.placeDurationList.Remove(duration);
            PlaceDurationsList.Remove(duration);

            TourPlace tourPlace = new TourPlace(DataManager.Ins.currentTour.id, SelectedTour.placeDurationList);
            await DataManager.Ins.TourPlaceServices.UpdateTourPlace(tourPlace);

            DependencyService.Get<IToast>().ShortToast("Schedule has been deleted");
        });

        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            PlaceId_Duration result = obj as PlaceId_Duration;

            if (result != null)
            {
                DataManager.Ins.currentDuration = result;
                navigation.PushAsync(new NewScheduleTourView());
                SelectedDuration = null;
            }
        });

        public ICommand NewScheduleCommand => new Command<object>((obj) =>
        {
            DataManager.Ins.currentDuration = null;
            navigation.PushAsync(new NewScheduleTourView());
        });
        public ICommand SaveCommand => new Command<object>(async (obj) =>
        {
            if(DataManager.Ins.currentTour.placeDurationList == null || DataManager.Ins.currentTour.placeDurationList.Count == 0)
            {
                DependencyService.Get<IToast>().ShortToast("The tour schedule is not available");
                return;
            }
            TourPlace tourPlace = new TourPlace(DataManager.Ins.currentTour.id, DataManager.Ins.currentTour.placeDurationList);

            DataManager.Ins.currentTour.imgSource = new List<string>();
            string url = await DataManager.Ins.TourServices.saveImage(DataManager.Ins.currentImgTourStream, DataManager.Ins.currentTour.id, 0);
            DataManager.Ins.currentTour.imgSource.Add(url);

            await DataManager.Ins.TourServices.AddTour(DataManager.Ins.currentTour);
            await DataManager.Ins.TourPlaceServices.AddTourPlace(tourPlace);

            DependencyService.Get<IToast>().ShortToast("New tour has been inserted");
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
            if (DataManager.Ins.currentTour.duration == null || DataManager.Ins.currentTour.duration == "") return;
            string[] _ProcessedDuration = DataManager.Ins.currentTour.duration.Split('/');
            string result = _ProcessedDuration[0] + " Day " + _ProcessedDuration[1] + " Night";
            ProcessedDuration = result;
        }
    }
}
