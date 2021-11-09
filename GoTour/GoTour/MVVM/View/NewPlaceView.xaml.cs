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
    public partial class NewPlaceView : ContentPage
    {
        public NewPlaceView()
        {
            InitializeComponent();
            this.BindingContext = new NewPlaceViewModel(Navigation, Shell.Current);
        }
    }
}