using GoTour.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class NotificationViewModel:ObservableObject
    {
        INavigation navigation;
        public NotificationViewModel() { }
        public NotificationViewModel(INavigation navigation)
        {
            this.navigation = navigation;
        }
    }
}
