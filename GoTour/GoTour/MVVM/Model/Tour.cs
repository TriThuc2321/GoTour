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
        public Tour() { }

        public Tour(string id, string name, List<string> imgSource, string startTime, string duration, List<PlaceId_StayPlace> SPforPList ,List<string> tourGuide, string passengerNumber, string description, bool isOccured, string basePrice, string remaining)
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
            this.placeDurationList = new List<PlaceId_Duration>();
            this.basePrice = basePrice;
            this.SPforPList = SPforPList;
            this.remaining = remaining;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
