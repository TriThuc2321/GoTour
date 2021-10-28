using System;
using System.Collections.Generic;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class Price
    {
        public string id;
        public string name;
        public int price;
        public string hotleId;

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
