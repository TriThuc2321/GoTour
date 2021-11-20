using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class DetailTour2ViewModel : ObservableObject
    {
        INavigation navigation;
        public DetailTour2ViewModel() { }
        public Command NavigationBack { get; }
        public Command ViewTicket { get; }
        public DetailTour2ViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            NavigationBack = new Command(() => navigation.PopAsync());
            ViewTicket = new Command(viewTicket);
           
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
            ListTourGuide = new ObservableCollection<User>();
            for (int i = 0; i < DataManager.Ins.currentTour.tourGuide.Count; i++)
            {
                foreach (var ite in DataManager.Ins.tourGuides)
                {
                    if (DataManager.Ins.currentTour.tourGuide[i] == ite.email)
                    {
                        ListTourGuide.Add(ite);
                        break;
                    }
                }

            }


        }

        void viewTicket(object obj)
        {
            navigation.PushAsync(new BookedTicketDetailView());
        }

        private ObservableCollection<User> listTourGuide;
        public ObservableCollection<User> ListTourGuide
        {
            get { return listTourGuide; }
            set
            {
                listTourGuide = value;
                OnPropertyChanged("listTourGuide");
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
            string result = _ProcessedDuration[0] + " Days "+ _ProcessedDuration[1] + " Nights";
            ProcessedDuration = result;
        }


    }
}
