using System;
using System.Collections.Generic;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class Review
    {
        public string tourId { get; set; }
        public string email { get; set; }
        public string message { get; set; }
        public DateTime time { get; set; }
        public int starNumber { get; set; }

        public Review()
        {
        }

        public Review(string tourId, string email, string message, DateTime time, int star)
        {
            this.tourId = tourId;
            this.email = email;
            this.message = message;
            this.time = time;
            this.starNumber = star;
        }
    }
}
