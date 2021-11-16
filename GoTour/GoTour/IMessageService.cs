using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoTour
{
    interface IMessageService
    {
        Task ShowAsync(string title, string message);
    }
}
