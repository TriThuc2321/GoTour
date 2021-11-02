using System;
using System.Collections.Generic;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class Class1
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> imgSource { get; set; }
        public string startTime { get; set; }
        public string duration { get; set; }
        public List<string> tourGuide { get; set; }
        public string passengerNumber { get; set; }
        public string description { get; set; }
        public Boolean isOccured { get; set; }
        public Class1() { }

        public Class1(string id, string name, List<string> imgSource, string startTime, string duration, List<string> tourGuide, string passengerNumber, string description, bool isOccured)
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
        }
    }
}
