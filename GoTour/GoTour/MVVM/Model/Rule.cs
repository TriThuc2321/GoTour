using System;
using System.Collections.Generic;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class Rule
    {
       public List<string> deduct { get; set; }

        public Rule(List<string> deduct)
        {
            this.deduct = deduct;
        }
        public Rule()
        {
        }
    }
}
