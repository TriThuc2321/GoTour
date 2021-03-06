using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoTour.MVVM.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoTour.MVVM.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourGuiderWorkSpaceView : ContentPage
    {
        public TourGuiderWorkSpaceView()
        {
            InitializeComponent();
            //this.BindingContext = new TourGuiderWorkSpaceViewModel(Navigation, Shell.Current);
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext = new TourGuiderWorkSpaceViewModel(Navigation, Shell.Current);
        }
    }
}