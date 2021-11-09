using GoTour.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class Place : ObservableObject
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> imgSource { get; set; }
        public string description { get; set; }

        public Place() { }
        public Place(string id, string name, List<string> imgSource, string description)
        {
            this.id = id;
            this.name = name;
            this.imgSource = imgSource;
            this.description = description;
        }
    }
}
