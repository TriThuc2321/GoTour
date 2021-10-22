using GoTour.Core;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class LoginViewModel: ObservableObject
    {
        INavigation navigation;
        public LoginViewModel() {}

        public Command LoginCommand { get; }
        public Command ForgotPasswordCommand { get; }
        public Command RememberCommand { get; }
        public LoginViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            LoginCommand = new Command(loginHandle);
            ForgotPasswordCommand = new Command(forgotHandle);
        }

        void loginHandle(object obj)
        {
            navigation.PushAsync(new HomeView());
        }
        void forgotHandle(object obj)
        {
            navigation.PushAsync(new HomeView());
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        private bool rememberAccount;
        public bool RememberAccount
        {
            get { return rememberAccount; }
            set
            {
                rememberAccount = value;
                OnPropertyChanged("RememberAccount");
            }
        }

    }
}
