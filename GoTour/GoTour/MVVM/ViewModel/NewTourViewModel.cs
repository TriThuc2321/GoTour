using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class NewTourViewModel : ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public Command OpenDetailTour { get; }
        public Command BookTourCommand { get; }
        public Command NavigationBack { get; }
        public Command EditTextCommand { get; }
        public Command AddImageCommand { get; }

        public Stream imgTemp;
        public NewTourViewModel() { }
        public NewTourViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            SelectedTour = DataManager.Ins.currentTour;

            NavigationBack = new Command(() => navigation.PopAsync());
            OpenDetailTour = new Command(OpenDetailTourHandler);
            //EditTextCommand = new Command(updateData);
            AddImageCommand = new Command(addHandleAsync);

            StartDatePicker = DateTime.Now;
            StartTimePicker = new TimeSpan(4, 0, 0);

            TourGuides = new ObservableCollection<User>();
            AllTourGuide = new ObservableCollection<User>();
            foreach (var ite in DataManager.Ins.tourGuides)
            {
                AllTourGuide.Add(ite);
            }
           
            SelectedTourGuide = AllTourGuide[0];
        }
        public ICommand AddTourGuideCommand => new Command<object>(async (obj) =>
        {
            if(SelectedTourGuide!= null)
            {
                TourGuides.Add(SelectedTourGuide);
                AllTourGuide.Remove(SelectedTourGuide);
            }          
            
        });
        public ICommand DeleteTourGuideCommand => new Command<object>(async (obj) =>
        {
            User result = obj as User;
            if (result != null)
            {
                AllTourGuide.Add(result);
                TourGuides.Remove(result);
            }
        });

        private async void addHandleAsync(object obj)
        {
            await CrossMedia.Current.Initialize();
            var imgData = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions());

            if (imgData != null)
            {
                Img = ImageSource.FromStream(imgData.GetStream);
                imgTemp = imgData.GetStream();
            }

        }
        /*private async void updateTour(object obj)
        {
            if (Description == null || Name == null || Price == null || Price == null || Day == null || Night == null || PassengerNumber == null || StartTimePicker == null || StartDatePicker == null ||
                Description == "" || Name == "" || Img == null || TourGuides.Count == 0)
            {
                DependencyService.Get<IToast>().ShortToast("Please fill out tour information.");
                return;
            }
            string[] temp = StartDatePicker.ToString().Split(' ');
            string[] dayTemp = temp[0].Split('/');

            while (true)
            {
                int i = 0;
                DataManager.Ins.currentTour.id = DataManager.Ins.GeneratePlaceId();
                for ( i = 0; i < DataManager.Ins.ListTour.Count; i++)
                {
                    if(DataManager.Ins.currentTour.id == DataManager.Ins.ListTour[i].id)
                    {
                        break;
                    }
                }
                if (i == DataManager.Ins.ListTour.Count) break;
            }                 

            DataManager.Ins.currentTour.description = Description;
            DataManager.Ins.currentTour.name = Name;
            DataManager.Ins.currentTour.basePrice = Price;
            DataManager.Ins.currentTour.startTime = dayTemp[1] + '/' + dayTemp[0] + '/' + dayTemp[2] + " " + StartTimePicker.ToString();
            DataManager.Ins.currentTour.duration = Day + '/' + Night;
            DataManager.Ins.currentTour.passengerNumber = PassengerNumber;
            DataManager.Ins.currentTour.tourGuide = new List<string>();

            for (int i =0; i< TourGuides.Count; i++)
            {
                DataManager.Ins.currentTour.tourGuide.Add(TourGuides[i].email);
            }

            StartDate = dayTemp[1] + '/' + dayTemp[0] + '/' + dayTemp[2];
            ProcessedDuration = Day + " Day " + Night + " Night";
            StartTime = StartTimePicker.ToString();

            DataManager.Ins.currentTour.imgSource = new List<string>();
            string url = await DataManager.Ins.TourServices.saveImage(imgTemp, DataManager.Ins.currentTour.id, 0);
            DataManager.Ins.currentTour.imgSource.Add(url);          

            await DataManager.Ins.TourServices.AddTour(DataManager.Ins.currentTour);
            DependencyService.Get<IToast>().ShortToast("New tour has been inserted");
        }*/
        void updateTour()
        {            
            string[] temp = StartDatePicker.ToString().Split(' ');
            string[] dayTemp = temp[0].Split('/');

            while (true)
            {
                int i = 0;
                DataManager.Ins.currentTour.id = DataManager.Ins.GeneratePlaceId();
                for (i = 0; i < DataManager.Ins.ListTour.Count; i++)
                {
                    if (DataManager.Ins.currentTour.id == DataManager.Ins.ListTour[i].id)
                    {
                        break;
                    }
                }
                if (i == DataManager.Ins.ListTour.Count) break;
            }

            DataManager.Ins.currentTour.description = Description;
            DataManager.Ins.currentTour.name = Name;
            DataManager.Ins.currentTour.basePrice = Price;
            DataManager.Ins.currentTour.startTime = dayTemp[1] + '/' + dayTemp[0] + '/' + dayTemp[2] + " " + StartTimePicker.ToString();
            DataManager.Ins.currentTour.duration = "";
            DataManager.Ins.currentTour.passengerNumber = PassengerNumber;
            DataManager.Ins.currentTour.tourGuide = new List<string>();
            DataManager.Ins.currentImgTourStream = imgTemp;

            for (int i = 0; i < TourGuides.Count; i++)
            {
                DataManager.Ins.currentTour.tourGuide.Add(TourGuides[i].email);
            }

            /*StartDate = dayTemp[1] + '/' + dayTemp[0] + '/' + dayTemp[2];
            ProcessedDuration = Day + " Day " + Night + " Night";
            StartTime = StartTimePicker.ToString();*/


            /*DataManager.Ins.currentTour.imgSource = new List<string>();
            string url = await DataManager.Ins.TourServices.saveImage(imgTemp, DataManager.Ins.currentTour.id, 0);
            DataManager.Ins.currentTour.imgSource.Add(url);

            await DataManager.Ins.TourServices.AddTour(DataManager.Ins.currentTour);
            DependencyService.Get<IToast>().ShortToast("New tour has been inserted");*/
        }
        public void OpenDetailTourHandler()
        {
            if (Description == null || Name == null || Price == null || Price == null || PassengerNumber == null || StartTimePicker == null || StartDatePicker == null ||
                Description == "" || Name == "" || Img == null || TourGuides.Count == 0)
            {
                DependencyService.Get<IToast>().ShortToast("Please fill out tour information.");
                return;
            }
            updateTour();
            navigation.PushAsync(new NewDetailTourView());
        }

        public void OpenBookTourView(object obj)
        {
            navigation.PushAsync(new BookTourView());
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

        private ObservableCollection<User> tourGuides;
        public ObservableCollection<User> TourGuides
        {
            get { return tourGuides; }
            set
            {
                tourGuides = value;
                OnPropertyChanged("TourGuides");
            }
        }
        private ObservableCollection<User> allTourGuide;
        public ObservableCollection<User> AllTourGuide
        {
            get { return allTourGuide; }
            set
            {
                allTourGuide = value;
                OnPropertyChanged("AllTourGuide");
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
        private string descriptionInfo = "True";
        public string DescriptionInfo
        {
            get { return descriptionInfo; }
            set
            {
                descriptionInfo = value;
                OnPropertyChanged("DescriptionInfo");
            }
        }
        private string reviewInfo = "False";
        public string ReviewInfo
        {
            get { return reviewInfo; }
            set
            {
                reviewInfo = value;
                OnPropertyChanged("ReviewInfo");
            }
        }
        /*private void DurationProcess()
        {
            if (DataManager.Ins.currentTour.duration == null) return;
            string[] _ProcessedDuration = DataManager.Ins.currentTour.duration.Split('/');
            string result = _ProcessedDuration[0] + " Day " + _ProcessedDuration[1] + " Night";

            Day = _ProcessedDuration[0];
            Night = _ProcessedDuration[1];

            ProcessedDuration = result;
        }*/
        private MemoryStream GetStreamFromUrl(string url)
        {
            byte[] imageData = null;
            MemoryStream ms;
            ms = null;
            try
            {
                using (var wc = new System.Net.WebClient())
                {
                    imageData = wc.DownloadData(url);
                }
                ms = new MemoryStream(imageData);
            }
            catch (Exception ex)
            {
            }
            return ms;
        }
        private User selectedTourGuide;
        public User SelectedTourGuide
        {
            get { return selectedTourGuide; }
            set
            {
                selectedTourGuide = value;
                OnPropertyChanged("SelectedTourGuide");

            }
        }

        private string sourceIcon;
        public string SourceIcon
        {
            get { return sourceIcon; }
            set
            {
                sourceIcon = value;
                OnPropertyChanged("SourceIcon");

            }
        }
        

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");

            }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");

            }
        }
        private string price;
        public string Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged("Price");

            }
        }
        /*private string day;
        public string Day
        {
            get { return day; }
            set
            {
                day = value;
                OnPropertyChanged("Day");
            }
        }
        private string night;
        public string Night
        {
            get { return night; }
            set
            {
                night = value;
                OnPropertyChanged("Night");

            }
        }*/
        private string startDate;
        public string StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");

            }
        }
        private string startTime;
        public string StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                OnPropertyChanged("StartTime");

            }
        }
        private string passengerNumber;
        public string PassengerNumber
        {
            get { return passengerNumber; }
            set
            {
                passengerNumber = value;
                OnPropertyChanged("PassengerNumber");

            }
        }

        private DateTime startDatePicker;
        public DateTime StartDatePicker
        {
            get { return startDatePicker; }
            set
            {
                startDatePicker = value;
                OnPropertyChanged("StartDatePikcer");
            }
        }
        private TimeSpan startTimePicker;
        public TimeSpan StartTimePicker
        {
            get { return startTimePicker; }
            set
            {
                startTimePicker = value;
                OnPropertyChanged("StartTimePicker");

            }
        }
        private ImageSource img;
        public ImageSource Img
        {
            get { return img; }
            set
            {
                img = value;
                OnPropertyChanged("Img");
            }
        }
    }
}
