using GoTour.Database;
using GoTour.MVVM.View;
using GoTour.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GoTour
{
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RegisterView), typeof(RegisterView));
            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(ResetPassword), typeof(ResetPassword));
            Routing.RegisterRoute(nameof(ConfirmEmailView), typeof(ConfirmEmailView));
            Routing.RegisterRoute(nameof(ManagerView), typeof(ManagerView));
            this.BindingContext = DataManager.Ins;
        }
    }
}
