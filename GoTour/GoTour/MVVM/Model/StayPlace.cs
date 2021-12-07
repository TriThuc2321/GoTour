using GoTour.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class StayPlace : ObservableObject
    {
        
        public StayPlace()
        {
            
        }

        public StayPlace(string id, string name, ObservableCollection<string> imgSource, string address, string description, bool isEnable, string placeId)
        {
            this.id = id;
            this.name = name;
            this.imgSource = imgSource;
            this.address = address;
            this.description = description;
            this.isEnable = isEnable;
            this.placeId = placeId;
        }

        private string _id;
        public string id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("id");
            }
        }

        private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }

        private ObservableCollection<string> _imgSource;
        public ObservableCollection<string> imgSource
        {
            get { return _imgSource; }
            set
            {
                _imgSource = value;
                OnPropertyChanged("imgSource");
            }
        }

        private string _address;
        public string address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged("address");
            }
        }

        private string _description;
        public string description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("description");
            }
        }
        private bool _isEnable;
        public bool isEnable
        {
            get { return _isEnable; }
            set
            {
                _isEnable = value;
                OnPropertyChanged("isEnable");
            }
        }
        private string _placeid;
        public string placeId
        {
            get { return _placeid; }
            set
            {
                _placeid = value;
                OnPropertyChanged("placeid");
            }
        }


    }
}
