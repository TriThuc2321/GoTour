using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
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

        async void registerHandleAsync(object obj)
        {
            if (Account == null || Password == null || ConfirmPassword == null || Name == null|| Account == "" || Password == "" || ConfirmPassword == "" || Name =="")
            {
                DependencyService.Get<IToast>().ShortToast("Please fill out your information");
            }
            else if(!checkEmail(Account))
            {
                DependencyService.Get<IToast>().ShortToast("Email invalid");
            }
            else if (Password.Length < 6)
            {
                DependencyService.Get<IToast>().ShortToast("Password must be more than 6 characters");
            }
            else if (Password != ConfirmPassword)
            {
                DependencyService.Get<IToast>().ShortToast("Confirm password is incorrect");
            }
            else
            {
                await DataManager.Ins.UsersServices.addUser(new User()
                {
                    email = Account,
                    password = DataManager.Ins.UsersServices.Encode(Password),
                    name = Name,
                    contact = "",
                    birthday = "",
                    cmnd = "",
                    profilePic = "defaultUser.png",
                    address = "",
                    score = 0,
                    rank = 3
                });

                DependencyService.Get<IToast>().ShortToast("Register successfully");
            }
            
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
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
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
