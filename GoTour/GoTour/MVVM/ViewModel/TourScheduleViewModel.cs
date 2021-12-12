using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class TourScheduleViewModel : ObservableObject
    {
      
        INavigation navigation;
        public TourScheduleViewModel() { }
        public Command NavigationBack { get; }
        public Command ViewTicket { get; }
        public TourScheduleViewModel(INavigation navigation)
        {
            this.navigation = navigation;

            string[] temp = DataManager.Ins.currentTour.startTime.Split(' ');
            string[] TourStartTime = temp[0].Split('/');

            DateTime TourStartTime1 = new DateTime(int.Parse(TourStartTime[2]), int.Parse(TourStartTime[0]), int.Parse(TourStartTime[1]));

            NavigationBack = new Command(() => navigation.PopAsync());
            ViewTicket = new Command(viewTicket);

            timeLine = new List<SupportUI>();
            foreach (Place ite in DataManager.Ins.ListPlace)
            {
                DateTime time;
                foreach (var ite2 in DataManager.Ins.currentTour.placeDurationList)
                {
                    int day = int.Parse(TourStartTime[0]);
                    if (ite2.placeId == ite.id)
                    {
                        ite2.host = ite;
                        for (int i = 0; i < ite2.day; i++)
                        {
                            time = TourStartTime1.AddDays(i);
                            DateTime currrent_time = DateTime.Now.AddDays(0);
                            TimeSpan interval = time.Subtract(currrent_time);
                            double count = interval.Days * 24 + interval.Hours + ((interval.Minutes * 100) / 60) * 0.01;
                            if (count < 0)
                                timeLine.Add(new SupportUI(ite, time, "Black"));
                            else timeLine.Add(new SupportUI(ite, time, "Gray"));
                        }
                        for (int i = 0; i < ite2.night; i++)
                        {

                            time = TourStartTime1.AddDays(i);
                            time = time.AddHours(12);
                            DateTime currrent_time = DateTime.Now.AddDays(0);
                            TimeSpan interval = time.Subtract(currrent_time);
                            double count = interval.Days * 24 + interval.Hours + ((interval.Minutes * 100) / 60) * 0.01;
                            if (count < 0)
                                timeLine.Add(new SupportUI(ite, time, "Black"));
                            else timeLine.Add(new SupportUI(ite, time, "Gray"));
                        }
                        TourStartTime1 = TourStartTime1.AddDays(ite2.day);
                        break;
                    }
                }
            }
            SelectedTour = DataManager.Ins.currentTour;

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
            DurationProcess();
            SortTimeline();
            SetCurrentSchedule();
        }

        private void SetCurrentSchedule()
        {
            currentSchedule = timeLine[0];
            foreach (var ite in timeLine)
            {
                if (ite.color == "White") currentSchedule = ite;
            }

            foreach (var ite2 in selectedTour.SPforPList)
            {
                if (ite2.placeId == currentSchedule.place.id)
                {
                    foreach (var ite3 in DataManager.Ins.ListStayPlace)
                    {
                        if (ite3.id == ite2.stayPlaceId)
                        {
                            selectedStayPlace = ite3;
                        }
                    }
                }
            }
        }

        private string isOccured = "0.5";
        public string IsOccurred
        {
            get { return isOccured; }
            set
            {
                isOccured = value;
                OnPropertyChanged("IsOccurred");
            }
        }


        private SupportUI currentSchedule;
        public SupportUI CurrentSchedule
        {
            get { return currentSchedule; }
            set
            {
                currentSchedule = value;
                OnPropertyChanged("CurrentSchedule");
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

        private List<SupportUI> timeLine;
        public List<SupportUI> TimeLine
        {
            get { return timeLine; }
            set
            {
                timeLine = value;
                OnPropertyChanged("TimeLine");
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

        void viewTicket(object obj)
        {
            navigation.PushAsync(new BookedTicketDetailView());
        }

        private void DurationProcess()
        {
            if (selectedTour.duration == null) return;
            string[] _ProcessedDuration = selectedTour.duration.Split('/');
            string result = _ProcessedDuration[0] + " Days " + _ProcessedDuration[1] + " Nights";
            ProcessedDuration = result;
        }
        private void SortTimeline()
        {
            if (timeLine.Count == 0) return;
            for (int i = 0; i < timeLine.Count - 1; i++)
            {
                for (int j = i; j < timeLine.Count; j++)
                {
                    DateTime time1 = timeLine[i].dateTime;
                    DateTime time2 = timeLine[j].dateTime;

                    TimeSpan interval = time1.Subtract(time2);
                    double count = interval.Days * 24 + interval.Hours + ((interval.Minutes * 100) / 60) * 0.01;
                    if (count < 0) continue;
                    else
                    {
                        SupportUI temp = timeLine[i];
                        timeLine[i] = timeLine[j];
                        timeLine[j] = temp;
                    }

                }
            }
        }

    }
    public class SupportUI : ObservableObject
    {
        public Place place { get; set; }
        public DateTime dateTime { get; set; }
        public string color { get; set; }
        public SupportUI(Place _place, DateTime _datetime, string _color)
        {
            this.place = _place;
            this.color = _color;
            this.dateTime = _datetime;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
