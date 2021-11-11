using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class StayPlace : INotifyPropertyChanged
    {
        public StayPlace(string id, string name, List<string> imgSource, string address, string description, string placeId)
        {
            this.id = id;
            this.name = name;
            this.imgSource = imgSource;
            this.address = address;
            this.description = description;
            this.placeId = placeId;
        }
        public StayPlace()
        {
            
        }

        public string id { get; set; }
        public string name { get; set; }
        public List<string> imgSource { get; set; }
        public string address { get; set; }
        public string description { get; set; }
        public string placeId { get; set; }

      

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
