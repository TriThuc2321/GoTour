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
    class NotificationViewModel:ObservableObject
    {
        INavigation navigation;
        public Command NavigationBack { get; }
        public NotificationViewModel() { }
        public NotificationViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            ListNotification = new ObservableCollection<Notification>();
            ListNotification2 = new ObservableCollection<Notification>();
            NavigationBack = new Command(() => navigation.PopAsync());

            foreach (Notification ite in DataManager.Ins.NotiServices.GetMySystemNoti(DataManager.Ins.CurrentUser.email))
               ListNotification.Add(ite);

            foreach (Notification ite in DataManager.Ins.NotiServices.GetMyGuiderNoti(DataManager.Ins.CurrentUser.email))         
               ListNotification2.Add(ite);
        }
        public ICommand DeleteCommand => new Command<object>((obj) =>
        {
            Notification selected = (Notification)obj;
            if (selected == null) return;
            foreach (Notification ite in ListNotification)
            {
                if (ite.id == selected.id)
                {
                    ite.IsVisible = "False";
                    DataManager.Ins.NotiServices.UpdateNoti(selected);
                    break;
                }
            }
        });
        public ICommand DeleteCommand1 => new Command<object>((obj) =>
        {
            Notification selected = (Notification)obj;
            if (selected == null) return;
            foreach (Notification ite in ListNotification2)
            {
                if (ite.id == selected.id)
                {
                    ite.IsVisible = "False";
                    DataManager.Ins.NotiServices.UpdateNoti(selected);
                    break;
                }
            }
        });
        public ICommand SelectedCommand => new Command<object>(async (obj) =>
        {
            Notification selected = obj as Notification;
            if (selected != null )
            {
                if (selected.IsChecked == "False")
                {
                    selected.IsChecked = "True";
                    await DataManager.Ins.NotiServices.UpdateNoti(selected);
                }
               
                DataManager.Ins.CurrentNoti = selected;

                /*OnPropertyChanged("ListNotification");*/
                await navigation.PushAsync(new DetailNotification());
                SelectedNoti = null;
            }
        });
        public ICommand SelectedCommand_2 => new Command<object>(async (obj) =>
        {
            Notification selected = obj as Notification;
            if (selected != null)
            {
                if (selected.IsChecked == "False")
                {
                    selected.IsChecked = "True";
                    await DataManager.Ins.NotiServices.UpdateNoti(selected);
                }
                DataManager.Ins.CurrentNoti = selected;
              
                navigation.PushAsync(new DetailNotification());
                SelectedNoti_2 = null;
            }
        });

        public ICommand AddMember => new Command<object>((obj) =>
        {
           
            DataManager.Ins.NotiServices.SendNoti(DataManager.Ins.GeneratePlaceId(), "System", "19522267@gm.uit.edu.vn", 2, "Test Notification","saaaaa","11");
          
        });

        public ICommand SystemNoti => new Command<object>((obj) =>
        {
            SystemNotiUI = "True";
            GuiderNotiUI = "False";

        });

        public ICommand GuiderNoti => new Command<object>((obj) =>
        {
            SystemNotiUI = "False";
            GuiderNotiUI = "True";

        });

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
        private Notification selectedNoti_2;
        public Notification SelectedNoti_2
        {
            get { return selectedNoti_2; }
            set
            {
                selectedNoti_2 = value;
                OnPropertyChanged("SelectedNoti_2");
            }
        }


        private ObservableCollection<Notification> listNotification;
        public ObservableCollection<Notification> ListNotification
        {
            get { return listNotification; }
            set
            {
                listNotification = value;
                OnPropertyChanged("ListNotification");
            }
        }
        private ObservableCollection<Notification> listNotification2;
        public ObservableCollection<Notification> ListNotification2
        {
            get { return listNotification2; }
            set
            {
                listNotification2 = value;
                OnPropertyChanged("ListNotification2");
            }
        }

        private string systemNotiUI = "True";
        public string SystemNotiUI
        {
            get { return systemNotiUI; }
            set
            {
                systemNotiUI = value;
                OnPropertyChanged("SystemNotiUI");

            }
        }
        private string guiderNotiUI = "False";
        public string GuiderNotiUI
        {
            get { return guiderNotiUI; }
            set
            {
                guiderNotiUI = value;
                OnPropertyChanged("GuiderNotiUI");

            }
        }
    }
}
