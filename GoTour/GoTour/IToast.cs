using System;
using System.Collections.Generic;
using System.Text;

namespace GoTour
{
    public interface IToast
    {
        void ShortToast(string message);
        void LongToast(string message);
    }
}
