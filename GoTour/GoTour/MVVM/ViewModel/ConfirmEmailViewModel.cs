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
        Shell currentShell;
        public Command ConfirmCommand { get; }
        public ConfirmEmailViewModel() {}
        public ConfirmEmailViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
            ConfirmCommand = new Command(confirmHandle);
           
        }

        async void confirmHandle(object obj)
        {
            if(VerifyCode == DataManager.Ins.VerifyCode)
            {
                await DataManager.Ins.UsersServices.addUser(DataManager.Ins.CurrentUser);
                DependencyService.Get<IToast>().ShortToast("Register successfully");
                await currentShell.GoToAsync($"//{nameof(HomeView)}");
                //navigation.PushAsync(new HomeView());
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
        
    }
}
