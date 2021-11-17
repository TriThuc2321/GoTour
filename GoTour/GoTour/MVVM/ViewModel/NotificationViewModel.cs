using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class NotificationViewModel : ObservableObject
    {
        INavigation navigation;
        public NotificationViewModel() { }
        public NotificationViewModel(INavigation navigation)
        {
            this.navigation = navigation;
        }
    }
}
