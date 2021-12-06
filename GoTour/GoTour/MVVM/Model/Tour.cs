using GoTour.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class Tour : ObservableObject
    {
        private string _id;
        public string id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("id");
            }
        }

        private string _name;
        public string name 
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }

        private ObservableCollection<string> _imgSource;
        public ObservableCollection<string> imgSource
        {
            get { return _imgSource; }
            set
            {
                _imgSource = value;
                OnPropertyChanged("imgSource");
            }
        }

        private string _startTime;
        public string startTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                OnPropertyChanged("startTime");
            }
        }

        private string _duration;
        public string duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                OnPropertyChanged("duration");
            }
        }

        private ObservableCollection<string> _tourGuide;
        public ObservableCollection<string> tourGuide
        {
            get { return _tourGuide; }
            set
            {
                _tourGuide = value;
                OnPropertyChanged("tourGuide");
            }
        }

        private string _passengerNumber;
        public string passengerNumber
        {
            get { return _passengerNumber; }
            set
            {
                _passengerNumber = value;
                OnPropertyChanged("passengerNumber");
            }
        }

        private string _description;
        public string description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("description");
            }
        }

        private bool _isOccured;
        public bool isOccured
        {
            get { return _isOccured; }
            set
            {
                _isOccured = value;
                OnPropertyChanged("isOccured");
            }
        }

        private string _basePrice;
        public string basePrice
        {
            get { return _basePrice; }
            set
            {
                _basePrice = value;
                OnPropertyChanged("basePrice");
            }
        }

        private string _remaining;
        public string remaining
        {
            get { return _remaining; }
            set
            {
                _remaining = value;
                OnPropertyChanged("remaining");
            }
        }

        private ObservableCollection<PlaceId_StayPlace> _SPforPList;
        public ObservableCollection<PlaceId_StayPlace> SPforPList
        {
            get { return _SPforPList; }
            set
            {
                _SPforPList = value;
                OnPropertyChanged("SPforPList");
            }
        }

        private ObservableCollection<PlaceId_Duration> _placeDurationList;
        public ObservableCollection<PlaceId_Duration> placeDurationList
        {
            get { return _placeDurationList; }
            set
            {
                _placeDurationList = value;
                OnPropertyChanged("placeDurationList");
            }
        }
        private ObservableCollection<Review> _reviewList;
        public ObservableCollection<Review> reviewList
        {
            get { return _reviewList; }
            set
            {
                _reviewList = value;
                OnPropertyChanged("reviewList");
            }
        }
        private string _startNumber;
        public string starNumber
        {
            get { return _startNumber; }
            set
            {
                _startNumber = value;
                OnPropertyChanged("startNumber");
            }
        }
        public Tour() { }

        public Tour(string id, string name, ObservableCollection<string> imgSource, string startTime, string duration, ObservableCollection<string> tourGuide, string passengerNumber, string description, bool isOccured, string basePrice, string remaining, ObservableCollection<PlaceId_StayPlace> sPforPList, ObservableCollection<PlaceId_Duration> placeDurationList, ObservableCollection<Review> reviewList, string starNumber)
        {
            this.id = id;
            this.name = name;
            this.imgSource = imgSource;
            this.startTime = startTime;
            this.duration = duration;
            this.tourGuide = tourGuide;
            this.passengerNumber = passengerNumber;
            this.description = description;
            this.isOccured = isOccured;
            this.basePrice = basePrice;
            this.remaining = remaining;
            SPforPList = sPforPList;
            this.placeDurationList = placeDurationList;
            this.reviewList = reviewList;
            this.starNumber = starNumber;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        
        

    }
}
