using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class ResetPasswordViewModel: ObservableObject
    {
        INavigation navigation;
        public ResetPasswordViewModel() {}

        public Command EyeCommand { get; }
        public Command EyeConfirmCommand { get; }
        public Command ConfirmCommand { get; }

        public ResetPasswordViewModel(INavigation navigation)
        {
            this.navigation = navigation;

            EyeSource = "eyeOffIcon.png";
            IsPassword = true;
            EyeCommand = new Command(eyeHandle);

            EyeSourceConfirm = "eyeOffIcon.png";
            IsPasswordConfirm = true;
            EyeConfirmCommand = new Command(eyeConfirmHandle);

            ConfirmCommand = new Command(confirmHandle);
        }
        async void confirmHandle(object obj)
        {
            if(VerifyCode==null || Password == null || ConfirmPassword == null || VerifyCode == "" || Password == "" || ConfirmPassword == "")
            {
                DependencyService.Get<IToast>().ShortToast("Please fill out your information");
            }
            else if (Password.Length < 6)
            {
                DependencyService.Get<IToast>().ShortToast("Password must be more than 6 characters");
            }
            else if (Password != ConfirmPassword)
            {
                DependencyService.Get<IToast>().ShortToast("Confirm password is incorrect");
            }
            else if (VerifyCode == DataManager.Ins.VerifyCode)
            {
                DataManager.Ins.CurrentUser.password = DataManager.Ins.UsersServices.Encode(Password);
                await DataManager.Ins.UsersServices.UpdateUser(DataManager.Ins.CurrentUser);
                DependencyService.Get<IToast>().ShortToast("Reset password successfully");
                navigation.PushAsync(new HomeView());
            }
            else
            {
                DependencyService.Get<IToast>().ShortToast("Verify code is incorrect");
            }
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
        private string verifyCode;
        public string VerifyCode
        {
            get { return verifyCode; }
            set
            {
                verifyCode = value;
                OnPropertyChanged("VerifyCode");
            }
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
    }
}
