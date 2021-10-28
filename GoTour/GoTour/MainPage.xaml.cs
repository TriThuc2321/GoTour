using GoTour.MVVM.View;
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
            Routing.RegisterRoute("HomeView", typeof(HomeView));
            Routing.RegisterRoute("MenuView", typeof(MenuView));
        }
    }
}
