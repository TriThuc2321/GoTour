using System;
using System.Collections.Generic;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class StayPlace
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> imgSource { get; set; }
        public string address { get; set; }
        public string description { get; set; }
        public string placeId { get; set; }
    }
}
