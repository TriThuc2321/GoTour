using System;
using System.Collections.Generic;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class TourPlace
    {
        public string tourId;
        public List<PlaceId_Duration> placeDurationList;

        public TourPlace()
        {
        }

        public TourPlace(string tourId, List<PlaceId_Duration> placeDurationList)
        {
            this.tourId = tourId;
            this.placeDurationList = placeDurationList;
        }
    }
}
