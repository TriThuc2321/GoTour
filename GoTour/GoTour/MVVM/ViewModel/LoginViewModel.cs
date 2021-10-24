using GoTour.Core;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class LoginViewModel: ObservableObject
    {
        INavigation navigation;
        public LoginViewModel() {}

        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        public Command ForgotPasswordCommand { get; }
        public Command RememberCommand { get; }
        public Command EyeCommand { get; }
        public LoginViewModel(INavigation navigation)
        {
            this.navigation = navigation;

            LoginCommand = new Command(loginHandleAsync);
            RegisterCommand = new Command(registerHandle);
            ForgotPasswordCommand = new Command(forgotHandle);

            EyeSource = "eyeOffIcon.png";
            IsPassword = true;
            EyeCommand = new Command(eyeHandle);
        }
        void eyeHandle(object obj)
        {
            IsPassword = !IsPassword;
            if (!IsPassword)
            {
                EyeSource = "eyeIcon.png";
            }
            else
            {
                EyeSource = "eyeOffIcon.png";
            }
        }
        void registerHandle(object obj)
        {
            navigation.PushAsync(new RegisterView());
        }
        async void loginHandleAsync(object obj)
        {
            //navigation.PushAsync(new HomeView());

            DependencyService.Get<IToast>().ShortToast("Lorem ipsum dolor sit amet");
            //await SendEmail("tuhc", "text", "trithuc23232@gmail.com");

        }
        void forgotHandle(object obj)
        {
            navigation.PushAsync(new HomeView());
        }
        public async Task SendEmail(string subject, string body, string recipient)
        {
            try
            {

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("flanerapplication@gmail.com");
                mail.To.Add(recipient);
                mail.Subject = subject;
                mail.Body = body;

                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("flanerapplication@gmail.com", "flanerflaner");

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
               
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
