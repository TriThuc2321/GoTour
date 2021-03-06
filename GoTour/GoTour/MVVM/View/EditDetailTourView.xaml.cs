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
    public partial class EditDetailTourView : ContentPage
    {
        public EditDetailTourView()
        {
            InitializeComponent();
            //this.BindingContext = new EditDetailTourViewModel(Navigation);
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext = new EditDetailTourViewModel(Navigation);
        }
    }
}