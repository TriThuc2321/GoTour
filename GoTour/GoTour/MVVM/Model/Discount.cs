using System;
using System.Collections.Generic;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class Discount
    {
        public string id { get; set; }
        public string percent { get; set; }
        public string isUsed { get; set; }
        public string total { get; set; }
    }
}
