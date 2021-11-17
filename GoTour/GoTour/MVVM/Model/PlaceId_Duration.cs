using GoTour.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class PlaceId_Duration : ObservableObject
    {
        public int day { get; set; }
        public int night { get; set; }
        public string placeId { get; set; }
        public string description { get; set; }
        public Place host { get; set; }
        public PlaceId_Duration() { }
        public PlaceId_Duration(int day, int night, string placeId, string description)
        {
            this.day = day;
            this.night = night;
            this.placeId = placeId;
            this.description = description;
            this.host = null;
        }     

    }
}
