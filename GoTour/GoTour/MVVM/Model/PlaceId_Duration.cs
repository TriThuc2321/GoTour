using System;
using System.Collections.Generic;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class PlaceId_Duration
    {
        public int day;
        public int night;
        public string placeId;
        public PlaceId_Duration() { }
        public PlaceId_Duration(int day, int night, string placeId)
        {
            this.day = day;
            this.night = night;
            this.placeId = placeId;
        }
    }
}
