using System;
using System.Collections.Generic;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class FavouriteTour
    {
        public string id { get; set; }
        public string name { get; set; }
        public string imgSource { get; set; }

        public DateTime startTime { get; set; }
        public int days { get; set; }

        public string tourId { get; set; }
        public string userId { get; set; }


    }
}
