using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class Tour : INotifyPropertyChanged
    {

        public string id { get; set; }
        public string name { get; set; }
        public List<string> imgSource { get; set; }
        public string startTime { get; set; }
        public string duration { get; set; }
        public List<string> tourGuide { get; set; }
        public string passengerNumber { get; set; }
        public string description { get; set; }
        public bool isOccured { get; set; }
        public string basePrice { get; set; }
        public string remaining { get; set; }
        public List<PlaceId_StayPlace> SPforPList { get; set; }
        public List<PlaceId_Duration> placeDurationList { get; set; }
        public List<Review> reviewList { get; set; }
        public string starNumber { get; set; }
        public Tour() { }

        public Tour(string id, string name, List<string> imgSource, string startTime, string duration, List<string> tourGuide, string passengerNumber, string description, bool isOccured, string basePrice, string remaining, List<PlaceId_StayPlace> sPforPList, List<PlaceId_Duration> placeDurationList, List<Review> reviewList, string starNumber)
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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
