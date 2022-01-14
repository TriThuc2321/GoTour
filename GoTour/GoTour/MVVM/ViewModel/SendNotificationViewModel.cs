using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

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


        //ALERT YES NO AND SEND
        async void OnAlertYesNoClicked()
        {
            bool answer = await App.Current.MainPage.DisplayAlert("Confirm", "Once you sent, you can not take back !", "Yes", "No");
            if (answer)
            {
                if (IsCheckToAll)
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
                            foreach (User i in Recievers)
                            {
                                DataManager.Ins.NotiServices.SendNoti(DataManager.Ins.GeneratePlaceId(), DataManager.Ins.CurrentUser.email, i.email, 2, Message, Title, TourIdText);
                            }
                            //Title = "";
                            //TourIdText = "";
                            //StartTimetext = "";
                            //IsCheckToAll = true;
                            //Message = "";
                            //Recievers.Clear();
                            DependencyService.Get<IToast>().ShortToast("Sended successfully !");
                            navigation.PopAsync();
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
                            foreach (User i in Recievers)
                            {
                                DataManager.Ins.NotiServices.SendNoti(DataManager.Ins.GeneratePlaceId(), DataManager.Ins.CurrentUser.email, i.email, 2, Message, Title, TourIdText);
                            }
                            //Title = "";
                            //TourIdText = "";
                            //StartTimetext = "";
                            //IsCheckToAll = true;
                            //Message = "";
                            //Recievers.Clear();
                            DependencyService.Get<IToast>().ShortToast("Sended successfully !");
                            navigation.PopAsync();
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }

        //SEND COMMAND
        public ICommand SendCommand => new Command(() => SendNotification());
        public void SendNotification()
        {
            OnAlertYesNoClicked();
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

        //TO RADIO BUTTON ALL CHECKED
        private bool isCheckToAll { get; set; }
        public bool IsCheckToAll
        {
            get { return isCheckToAll; }
            set
            {
                if (isCheckToAll != value)
                {
                    isCheckToAll = value;
                    if (isCheckToAll == true)
                    {
                        IsVisibleRevieverPicker = false;
                        Recievers.Clear();
                        foreach (string i in ListEmailPicker)
                        {
                            foreach (User x in DataManager.Ins.ListUser)
                            {
                                if (x.email == i)
                                {
                                    Recievers.Add(x);
                                }
                            }
                        }
                    }
                    else
                    {
                        Recievers.Clear();
                        IsVisibleRevieverPicker = true;
                    }

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

        //PICKER TOUR LIST THAT YOUR OWN TO BE GUIDER
        public List<string> NameTourList { get; set; }
        public List<string> GetListNameTour()
        {

            List<string> ListTourYouWorkOn = new List<string>();

            if (DataManager.Ins.CurrentUser.rank == 1)
            {
                foreach (Tour i in DataManager.Ins.ListTour)
                {
                    ListTourYouWorkOn.Add(i.name);
                }
            }
            else
            {
                List<Tour> temp1 = new List<Tour>(); //List Tour

                foreach (Tour ite in DataManager.Ins.ListTour)
                {
                    temp1.Add(ite);
                }

                List<Tour> temp = new List<Tour>();
                List<Tour> result = new List<Tour>();


                //Lay email guider
                string yourEmail = DataManager.Ins.CurrentUser.email;


                //Loc email
                //temp = temp1.FindAll(e => e.tourGuide.Exists(p => p == yourEmail));
                foreach (var e in temp1)
                {
                    foreach (var p in e.tourGuide)
                    {
                        if (p == yourEmail)
                        {
                            temp.Add(e);
                            break;
                        }
                    }
                }

                foreach (var plc in temp)
                    if (!result.Contains(plc))
                        result.Add(plc);

                foreach (Tour ite3 in result)
                {
                    ListTourYouWorkOn.Add(ite3.name);
                }
            }
            return ListTourYouWorkOn;
        }

        //SELECTED TOUR NAME
        private string selectedTourName { get; set; }
        public string SelectedTourName
        {
            get { return selectedTourName; }
            set
            {
                if (selectedTourName != value)
                {
                    selectedTourName = value;

                    foreach (Tour i in DataManager.Ins.ListTour)
                    {
                        if (i.name.Equals(selectedTourName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            TourIdText = i.id;
                            getListEmailPicker(TourIdText);
                            IsCheckToAll = false;
                            StartTimetext = "Start: " + i.startTime;
                            break;
                        }

                    }
                    DependencyService.Get<IToast>().ShortToast("About Tour:  " + selectedTourName);

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
        //PICKER EMAIL LIST
        private ObservableCollection<string> listEmailPicker { get; set; }
        public ObservableCollection<string> ListEmailPicker
        {
            get { return listEmailPicker; }
            set
            {
                listEmailPicker = value;
                OnPropertyChanged("ListEmailPicker");
            }
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
                    foreach (User i in DataManager.Ins.ListUser)
                    {
                        if (i.email == selectedEmail)
                        {
                            if (Recievers == null || Recievers.Count == 0)
                            {
                                Recievers.Add(i);
                            }
                            else
                            {
                                if (!Recievers.Contains(i))
                                {
                                    Recievers.Add(i);
                                }
                                else
                                {
                                    DependencyService.Get<IToast>().ShortToast("This one has been added");

                                }
                            }
                            //ListEmailPicker.Remove(i.email);
                        }
                    }
                }
                OnPropertyChanged("SelectedTourName");
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

            ListEmailPicker = new ObservableCollection<string>();
            NameTourList = GetListNameTour();
            Recievers = new ObservableCollection<User>();
            IsCheckToAll = false;
        }

        void getListEmailPicker(string tourid)
        {
            ListEmailPicker = new ObservableCollection<string>();
            string temp = "";
            foreach (BookedTicket i in DataManager.Ins.ListBookedTickets)
            {
                if (i.tour != null && i.tour.id.Equals(tourid))
                {
                    temp = i.email;
                    if (!ListEmailPicker.Contains(temp))
                    {
                        ListEmailPicker.Add(temp);
                    }
                }
            }

            OnPropertyChanged("ListEmailPicker");
        }

    }
}