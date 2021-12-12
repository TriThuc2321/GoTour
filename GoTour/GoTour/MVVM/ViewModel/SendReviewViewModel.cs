using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class SendReviewViewModel: ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public Command Send { get; }

        public Command NavigationBack { get; }
        public SendReviewViewModel()
        {

        }

        public SendReviewViewModel(INavigation navigation, Shell shell)
        {
            this.navigation = navigation;
            this.currentShell = shell;

            NavigationBack = new Command(() => navigation.PopAsync());
            Send = new Command(send);

            SetInformation();
        }

        public async void send(object obj)
        {
            if (Message == "")
            {
                NoticeVisible = true;
                Notice = "Please enter your message!";
                return;
            }

            int star = 0;
            if (Choose1) star = 1;
            if (Choose2) star = 2;
            if (Choose3) star = 3;
            if (Choose4) star = 4;
            if (Choose5) star = 5;

            Review review = new Review(
                DataManager.Ins.CurrentUser.email,
                Message,
               DateTime.Now,
                star
                );

           await DataManager.Ins.ReviewService.Add(review);
        }

        void SetInformation()
        {
            Choose5 = true;

            Notice = "";
            NoticeVisible = false;

            Message = "";

        }

        private bool choose1;
        public bool Choose1
        {
            get { return choose1; }
            set
            {
                choose1 = value;
                OnPropertyChanged("Choose1");
            }
        }

        private bool choose2;
        public bool Choose2
        {
            get { return choose2; }
            set
            {
                choose2 = value;
                OnPropertyChanged("Choose2");
            }
        }

        private bool choose3;
        public bool Choose3
        {
            get { return choose3; }
            set
            {
                choose3 = value;
                OnPropertyChanged("Choose3");
            }
        }

        private bool choose4;
        public bool Choose4
        {
            get { return choose4; }
            set
            {
                choose4 = value;
                OnPropertyChanged("Choose4");
            }
        }

        private bool choose5;
        public bool Choose5
        {
            get { return choose5; }
            set
            {
                choose5 = value;
                OnPropertyChanged("Choose5");
            }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }

        private string notice;
        public string Notice
        {
            get { return notice; }
            set
            {
                notice = value;
                OnPropertyChanged("Notice");
            }
        }

        private bool noticeVisble;
        public bool NoticeVisible
        {
            get { return noticeVisble; }
            set
            {
                noticeVisble = value;
                OnPropertyChanged("NoticeVisible");
            }
        }
    }
}
