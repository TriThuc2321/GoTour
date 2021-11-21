using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class DetailNotificationViewModel: ObservableObject
    {
        INavigation navigation;
        public DetailNotificationViewModel() { }
        public Command NavigationBack { get; }
        public DetailNotificationViewModel(INavigation navigation)
        {           
            this.navigation = navigation;
            NavigationBack = new Command(() => navigation.PopAsync());

            SelectedNoti = DataManager.Ins.CurrentNoti;

            foreach (Notification ite in DataManager.Ins.NotiList)
                {

            }


        }

        private Notification selectedNoti;
        public Notification SelectedNoti
        {
            get { return selectedNoti; }
            set
            {
                selectedNoti = value;
                OnPropertyChanged("SelectedNoti");
            }
        }
    }
}
