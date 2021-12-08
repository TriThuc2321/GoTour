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
