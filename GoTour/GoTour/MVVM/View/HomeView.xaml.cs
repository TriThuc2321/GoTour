using GoTour.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoTour.MVVM.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : ContentPage
    {
        public HomeView()
        {
            InitializeComponent();
            //this.BindingContext = new HomeViewModel(Navigation, Shell.Current);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext = new HomeViewModel(Navigation, Shell.Current);
        }
    }
}