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
    public partial class InProgressView : ContentPage
    {
        public InProgressView()
        {
            InitializeComponent();
            //this.BindingContext = new InProgressViewModel(Navigation, Shell.Current);
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext = new InProgressViewModel(Navigation, Shell.Current);
        }
    }
}