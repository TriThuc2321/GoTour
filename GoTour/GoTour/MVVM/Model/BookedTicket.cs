using System;
using System.Collections.Generic;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class BookedTicket
    { 
        string id { get; set; }
        string tourId { get; set; }
        string name { get; set; }
        string birthday { get; set; }
        string contact { get; set; }
        string email { get; set; }
        string cmnd { get; set; }
        string address { get; set; }
        Discount discount { get; set; }
        string price { get; set; }
        

    }
}
