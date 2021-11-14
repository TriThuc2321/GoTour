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
    public partial class NewTourView : ContentPage
    {
        public NewTourView()
        {
            InitializeComponent();
            this.BindingContext = new NewTourViewModel(Navigation, Shell.Current);
        }
    }
}