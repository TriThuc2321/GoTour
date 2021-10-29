using GoTour.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class Price : ObservableObject
    {
        public string id {get;set ;}
        public string name { get; set; }
        public int price { get; set; }
        public string hotleId { get; set; }
        public Price()
        {
        }

        public Price(string id, string name, int price, string hotleId)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.hotleId = hotleId;
        }
    }
}
