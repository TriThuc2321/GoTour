using GoTour.Core;
using System;
using System.Collections.Generic;
using System.Text;
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
