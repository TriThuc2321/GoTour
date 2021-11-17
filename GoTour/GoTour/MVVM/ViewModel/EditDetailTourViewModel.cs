using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
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
            List<PlaceId_Duration> temp = selectedTour.placeDurationList;
            // TourPlaces = temp.FindAll(e => DataManager.Ins.currentTour.placeDurationList.Exists(p => p.placeId == e.id));
            DurationProcess();
            int c = 6;


        }
        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            PlaceId_Duration result = obj as PlaceId_Duration;
            
            if (result != null)
            {
                DataManager.Ins.currentDuration = result;
                navigation.PushAsync(new EditDetailScheduleView());
                SelectedDuration = null;
            }
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
            string result = _ProcessedDuration[0] + " Day " + _ProcessedDuration[1] + " Night";
            ProcessedDuration = result;
        }
    }
}
