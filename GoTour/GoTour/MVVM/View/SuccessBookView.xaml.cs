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
    public partial class SuccessBookView : ContentPage
    {
        public SuccessBookView()
        {
            InitializeComponent();
            this.BindingContext = new SuccessBookViewModel(Navigation, Shell.Current);
        }
    }
}