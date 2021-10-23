using GoTour.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class RegisterViewModel: ObservableObject
    {
        INavigation navigation;
        public Command EyeCommand { get; }
        public Command EyeConfirmCommand { get; }
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        public RegisterViewModel() {}
        public RegisterViewModel(INavigation navigation)
        {
            this.navigation = navigation;

            EyeSource = "eyeOffIcon.png";
            IsPassword = true;
            EyeCommand = new Command(eyeHandle);

            EyeSourceConfirm = "eyeOffIcon.png";
            IsPasswordConfirm = true;
            EyeConfirmCommand = new Command(eyeConfirmHandle);

            LoginCommand = new Command(loginHandleAsync);
            RegisterCommand = new Command(registerHandleAsync);
        }

        void registerHandleAsync(object obj)
        {
            
        }

        async void loginHandleAsync(object obj)
        {
            await navigation.PopAsync();
        }

        void eyeHandle(object obj)
        {
            IsPassword = !IsPassword;
            EyeSource = !IsPassword ? "eyeIcon.png" : "eyeOffIcon.png";
        }
        void eyeConfirmHandle(object obj)
        {
            IsPasswordConfirm = !IsPasswordConfirm;
            EyeSourceConfirm = !IsPasswordConfirm ? "eyeIcon.png" : "eyeOffIcon.png";
        }

        bool checkEmail(string inputEmail)
        {
            if (inputEmail == null) return false;
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            return re.IsMatch(inputEmail);
        }
        private string eyeSource;
        public string EyeSource
        {
            get { return eyeSource; }
            set
            {
                eyeSource = value;
                OnPropertyChanged("EyeSource");
            }
        }
        private bool isPassword;
        public bool IsPassword
        {
            get { return isPassword; }
            set
            {
                isPassword = value;
                OnPropertyChanged("IsPassword");
            }
        }
        private string eyeSourceConfirm;
        public string EyeSourceConfirm
        {
            get { return eyeSourceConfirm; }
            set
            {
                eyeSourceConfirm = value;
                OnPropertyChanged("EyeSourceConfirm");
            }
        }
        private bool isPasswordConfirm;
        public bool IsPasswordConfirm
        {
            get { return isPasswordConfirm; }
            set
            {
                isPasswordConfirm = value;
                OnPropertyChanged("IsPasswordConfirm");
            }
        }
        private string account;
        public string Account
        {
            get { return account; }
            set
            {
                account = value;
                OnPropertyChanged("Account");
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
        private string confirmPassword;
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                confirmPassword = value;
                OnPropertyChanged("ConfirmPassword");
            }
        }
    }
}
