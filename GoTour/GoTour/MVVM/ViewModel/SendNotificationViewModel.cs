using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class SendNotificationViewModel : ObservableObject
    {
        INavigation navigation;

        //DELETE RECIVER ITEM
        public ICommand XButtonCommand => new Command<object>((obj) =>
        {
            User selected = obj as User;
            if (selected != null)
            {
                Recievers.Remove(selected);
            }
        });

        //DELETE FORM FILLED
        public ICommand DeleteCommand => new Command(() =>
        {
            Title = "";
            TourIdText = "";
            StartTimetext = "";
            IsCheckToAll = true;
            Message = "";
            Recievers.Clear();
        });

        //BACK PRESS
        public ICommand BackPress => new Command(() => navigation.PopAsync());

        //SEND COMMAND
        public ICommand SendCommand => new Command(() => SendNotification());
        public void SendNotification()
        {
            int type = 0;
            if(IsCheckToAll)
            {
                if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(TourIdText) || string.IsNullOrWhiteSpace(Message))
                {
                    DependencyService.Get<IToast>().ShortToast("Please fill out your notification needs");
                    return;
                }
                else
                {
                    if(SystemSelected == true)
                    {
                        type = 1;
                        foreach(User i in DataManager.Ins.ListUser)
                        {           
                           DataManager.Ins.NotiServices.SendNoti(DataManager.Ins.GeneratePlaceId(), DataManager.Ins.CurrentUser.email, i.email, type, Message, Title, TourIdText);
                        }
                        Title = "";
                        TourIdText = "";
                        StartTimetext = "";
                        IsCheckToAll = true;
                        Message = "";
                        Recievers.Clear();
                        DependencyService.Get<IToast>().ShortToast("Sended successfully !");
                    }
                    else
                    {
                        type = 2;
                        foreach(User i in DataManager.Ins.ListUser)
                        {
                            DataManager.Ins.NotiServices.SendNoti(DataManager.Ins.GeneratePlaceId(), DataManager.Ins.CurrentUser.email, i.email, type, Message, Title, TourIdText);
                        }
                        Title = "";
                        TourIdText = "";
                        StartTimetext = "";
                        IsCheckToAll = true;
                        Message = "";
                        Recievers.Clear();
                        DependencyService.Get<IToast>().ShortToast("Sended successfully !");
                    }
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(TourIdText) || string.IsNullOrWhiteSpace(Message))
                {
                    DependencyService.Get<IToast>().ShortToast("Please fill out your notification needs");
                    return;
                }
                else
                {
                    if (Recievers.Count < 1)
                    {
                        DependencyService.Get<IToast>().ShortToast("Who will recieve this notification ?");
                        return;
                    }
                    else
                    {
                        if (SystemSelected == true)
                        {
                            type = 1;
                            foreach (User i in Recievers)
                            {
                                DataManager.Ins.NotiServices.SendNoti(DataManager.Ins.GeneratePlaceId(), DataManager.Ins.CurrentUser.email, i.email, type, Message, Title, TourIdText);
                            }
                            Title = "";
                            TourIdText = "";
                            StartTimetext = "";
                            IsCheckToAll = true;
                            Message = "";
                            Recievers.Clear();
                            DependencyService.Get<IToast>().ShortToast("Sended successfully !");
                        }
                        else
                        {
                            type = 2;
                            foreach (User i in Recievers)
                            {
                                DataManager.Ins.NotiServices.SendNoti(DataManager.Ins.GeneratePlaceId(), DataManager.Ins.CurrentUser.email, i.email, type, Message, Title, TourIdText);
                            }
                            Title = "";
                            TourIdText = "";
                            StartTimetext = "";
                            IsCheckToAll = true;
                            Message = "";
                            Recievers.Clear();
                            DependencyService.Get<IToast>().ShortToast("Sended successfully !");
                        }
                    }
                }                 
            }
        }

        //LIST REVIEVER
        private ObservableCollection<User> _recievers;
        public ObservableCollection<User> Recievers
        {
            get { return _recievers; }
            set
            {
                _recievers = value;
                OnPropertyChanged("Recievers");
            }
        }

        //TITLE
        private string title { get; set; }
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        //SYSTEM RADIO BUTTON CHECKED
        private bool systemSelected { get; set; }
        public bool SystemSelected
        {
            get { return systemSelected; }
            set
            {
                systemSelected = value;
                OnPropertyChanged("SystemSelected");
            }
        }

        //TO RADIO BUTTON ALL CHECKED
        private bool isCheckToAll { get; set; }
        public bool IsCheckToAll
        {
            get { return isCheckToAll; }
            set
            {
                if(isCheckToAll != value)
                {
                    isCheckToAll = value;
                    if (isCheckToAll == true)
                    {
                        IsVisibleRevieverPicker = false;
                        Recievers.Clear();
                    }
                    else
                        IsVisibleRevieverPicker = true;
                }               
                OnPropertyChanged("IsCheckToAll");
            }
        }

        //VISIBLE RECIEVER PICKER
        private bool isVisibleRevieverPicker { get; set; }
        public bool IsVisibleRevieverPicker
        {
            get { return isVisibleRevieverPicker; }
            set
            { 
                isVisibleRevieverPicker = value;
                OnPropertyChanged("IsVisibleRevieverPicker");
            }
        }

        //PICKER TOUR LIST
        public List<string> NameTourList { get; set; }
        public List<string> GetListNameTour()
        {
            var temp = new List<string>();
            foreach(Tour i in DataManager.Ins.ListTour)
            {
                temp.Add(i.name);
            }
            return temp;
        }

        //SELECTED TOUR NAME
        private string selectedTourName { get; set; }
        public string SelectedTourName
        {
            get { return selectedTourName; }
            set
            {
                if(selectedTourName != value)
                {
                    selectedTourName = value;
                    DependencyService.Get<IToast>().ShortToast("About Tour:  " + selectedTourName);
                    foreach(Tour i in DataManager.Ins.ListTour)
                    {
                        if(i.name.Equals(selectedTourName, StringComparison.CurrentCultureIgnoreCase))
                            TourIdText = i.id;
                            StartTimetext = "Start: " + i.startTime;
                    }
                }
                OnPropertyChanged("SelectedTourName");
            }
        }

        //PICKER EMAIL LIST
        public List<string> ListEmailPicker { get; set; }
        public List<string> GetListEmail()
        {
            var temp = new List<string>();
            foreach (User i in DataManager.Ins.ListUser)
            {
                temp.Add(i.email);
            }
            return temp;
        }

        //SELECTED RMAIL
        private string selectedEmail { get; set; }
        public string SelectedEmail
        {
            get { return selectedEmail; }
            set
            {
                if (selectedEmail != value)
                {
                    selectedEmail = value;
                    DependencyService.Get<IToast>().ShortToast("Added " + selectedEmail);
                    foreach(User i in DataManager.Ins.ListUser)
                    {
                        if (i.email == selectedEmail)
                        {
                            Recievers.Add(i);
                        }       
                    }
                }
                OnPropertyChanged("SelectedTourName");
            }
        }

        //LABEL TOURID
        private string tourIdText { get; set; }
        public string TourIdText
        {
            get { return tourIdText; }
            set
            {
                tourIdText = value;
                OnPropertyChanged("TourIdText");
            }
        }
        
        //LABEL STARTING TIME
        private string startTimetext { get; set; }
        public string StartTimetext
        {
            get { return startTimetext; }
            set
            {
                startTimetext = value;
                OnPropertyChanged("StartTimetext");
            }
        }

        //MESSAGE
        private string message { get; set; }
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }

        //CONSTRUCTOR
        public SendNotificationViewModel() { }
        public SendNotificationViewModel(INavigation navigation)
        {
            this.navigation = navigation;

            //Recievers = DataManager.Ins.ListUser;
            NameTourList = GetListNameTour();
            ListEmailPicker = GetListEmail();
            Recievers = new ObservableCollection<User>();
            SystemSelected = true;
            IsCheckToAll = true;
        
        }


        
    }
}
