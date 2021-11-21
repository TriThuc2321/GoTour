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
    class EditTourViewModel: ObservableObject
    {
        INavigation navigation;
        Shell currentShell;

        public Command OpenDetailTour { get; }
        public Command BookTourCommand { get; }
        public Command OpenDetailTour1 { get; }
        public Command NavigationBack { get; }
        public Command EditTextCommand { get; }
        public Command AddImageCommand { get; }

        public Stream imgTemp;

        public EditTourViewModel() { }
        public EditTourViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            SelectedTour = DataManager.Ins.currentTour;

            NavigationBack = new Command(() => navigation.PopAsync());          
            OpenDetailTour = new Command(OpenDetailTourHandler);
            OpenDetailTour1 = new Command(OpenDetailTourHandler1);
            EditTextCommand = new Command(editTextHandle);
            AddImageCommand = new Command(addHandleAsync);


            Name = DataManager.Ins.currentTour.name;
            Description = DataManager.Ins.currentTour.description;
            Price = DataManager.Ins.currentTour.basePrice;
            DurationProcess();
            StartDate = DataManager.Ins.currentTour.startTime;
            Img = ImageSource.FromUri(new Uri(SelectedTour.imgSource[0]));
            PassengerNumber = DataManager.Ins.currentTour.passengerNumber;

            string[] temp = DataManager.Ins.currentTour.startTime.Split(' ');
            string[] dayTemp = temp[0].Split('/');

            StartTime = temp[1];

            StartDatePicker = new DateTime(int.Parse(dayTemp[2]), int.Parse(dayTemp[0]), int.Parse(dayTemp[1]));
            StartTimePicker = new TimeSpan(4, 0, 0);

            IsEdit = false;
            SourceIcon = "editIcon.png";
            IsText = true;

            TourGuides = new ObservableCollection<User>();
             for (int i = 0; i < DataManager.Ins.currentTour.tourGuide.Count; i++)
             {
                foreach (var ite in DataManager.Ins.tourGuides)
                {
                    if (DataManager.Ins.currentTour.tourGuide[i] == ite.email)
                    {
                        TourGuides.Add(ite);
                        break;
                    }
                }              
             }

            AllTourGuide = new ObservableCollection<User>();
            foreach (var ite in DataManager.Ins.tourGuides)
            {
                AllTourGuide.Add(ite);
            }

            SelectedTourGuide = AllTourGuide[0];

        }

        private async void editTextHandle(object obj)
        {
            IsEdit = !IsEdit;
            if (!IsEdit)
            {
                SourceIcon = "editIcon.png";
                updateData();
            }
            else
            {
                SourceIcon = "tickIcon.png";
            }
            IsText = !IsText;
        }
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
        private async void updateData()
        {
            string[] temp = StartDatePicker.ToString().Split(' ');
            string[] dayTemp = temp[0].Split('/');

            DataManager.Ins.currentTour.description = Description;
            DataManager.Ins.currentTour.name = Name;
            DataManager.Ins.currentTour.basePrice = Price;
            DataManager.Ins.currentTour.startTime = dayTemp[0] + '/' + dayTemp[1] + '/' + dayTemp[2] + " " + StartTimePicker.ToString(); 
            DataManager.Ins.currentTour.duration = Day + '/' + Night;
            DataManager.Ins.currentTour.passengerNumber = PassengerNumber;

            StartDate = dayTemp[0] + '/' + dayTemp[1] + '/' + dayTemp[2];
            ProcessedDuration = Day + " Day " + Night + " Night";
            StartTime = StartTimePicker.ToString();

            if (imgTemp != null)
            {
                DataManager.Ins.currentTour.imgSource = new List<string>();
                await DataManager.Ins.TourServices.DeleteFile(DataManager.Ins.currentTour.id, 0);
                string url = await DataManager.Ins.TourServices.saveImage(imgTemp, DataManager.Ins.currentTour.id, 0);
                DataManager.Ins.currentTour.imgSource.Add(url);
            }         
            
            await DataManager.Ins.TourServices.UpdateTour(DataManager.Ins.currentTour);

        }
        public void OpenDetailTourHandler()
        {
            navigation.PushAsync(new EditDetailTourView());
        }

        public void OpenBookTourView(object obj)
        {
            navigation.PushAsync(new BookTourView());
        }

        public void OpenDetailTourHandler1()
        {
            navigation.PushAsync(new TourScheduleView());
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
        private void DurationProcess()
        {
            if (DataManager.Ins.currentTour.duration == null) return;
            string[] _ProcessedDuration = DataManager.Ins.currentTour.duration.Split('/');
            string result = _ProcessedDuration[0] + " Day " + _ProcessedDuration[1] + " Night";

            Day = _ProcessedDuration[0];
            Night = _ProcessedDuration[1];

            ProcessedDuration = result;
        }
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
        private bool isEdit;
        public bool IsEdit
        {
            get { return isEdit; }
            set
            {
                isEdit = value;
                OnPropertyChanged("IsEdit");

            }
        }
        private bool isText;
        public bool IsText
        {
            get { return isText; }
            set
            {
                isText = value;
                OnPropertyChanged("IsText");

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
        private string day;
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
        }
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
