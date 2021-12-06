using GoTour.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class TourPlace : ObservableObject
    {
        public string tourId { get; set; }
        public ObservableCollection<PlaceId_Duration> placeDurationList { get; set; }

        public TourPlace()
        {
        }

        public TourPlace(string tourId, ObservableCollection<PlaceId_Duration> placeDurationList)
        {
            this.tourId = tourId;
            this.placeDurationList = placeDurationList;
        }
    }
}
