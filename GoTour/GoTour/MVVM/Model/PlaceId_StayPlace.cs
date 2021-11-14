using GoTour.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class PlaceId_StayPlace : ObservableObject
    {
        public string placeId { get; set; }
        public string stayPlaceId { get; set; }
        public PlaceId_StayPlace() { }
        public PlaceId_StayPlace(string placeId, string stayPlaceId)
        {
            this.placeId = placeId;
            this.stayPlaceId = stayPlaceId;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
