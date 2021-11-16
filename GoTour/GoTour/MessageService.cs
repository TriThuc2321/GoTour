using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoTour
{
    public class MessageService : IMessageService
    {
        public async Task ShowAsync(string title, string message)
        {
            await App.Current.MainPage.DisplayAlert(title, message, "Ok");
        }
    }
}
