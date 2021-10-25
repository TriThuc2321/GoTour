using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class ConfirmEmailViewModel: ObservableObject
    {
        INavigation navigation;
        public Command ConfirmCommand { get; }
        public Command EyeCommand { get; }
        public Command EyeConfirmCommand { get; }
        public ConfirmEmailViewModel() {}
        public ConfirmEmailViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            ConfirmCommand = new Command(confirmHandle);

            EyeSource = "eyeOffIcon.png";
            IsPassword = true;
            EyeCommand = new Command(eyeHandle);

            EyeSourceConfirm = "eyeOffIcon.png";
            IsPasswordConfirm = true;
            EyeConfirmCommand = new Command(eyeConfirmHandle);
        }

        async void confirmHandle(object obj)
        {
            if(VerifyCode == DataManager.Ins.VerifyCode && DataManager.Ins.IsRegister)
            {
                await DataManager.Ins.UsersServices.addUser(DataManager.Ins.CurrentUser);
                DependencyService.Get<IToast>().ShortToast("Register successfully");
                navigation.PushAsync(new HomeView());
            }
            else if (VerifyCode == DataManager.Ins.VerifyCode && !DataManager.Ins.IsRegister)
            {
                await DataManager.Ins.UsersServices.addUser(DataManager.Ins.CurrentUser);
                DependencyService.Get<IToast>().ShortToast("Reset password successfully");
                navigation.PushAsync(new LoginView());
            }
            else
            {
                DependencyService.Get<IToast>().ShortToast("Verify code is incorrect");
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
    }
}
