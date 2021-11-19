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
    public partial class RevenueManagerView : ContentPage
    {
        public RevenueManagerView()
        {
            InitializeComponent();
            this.BindingContext = new RevenueManagerViewModel(Navigation, Shell.Current);
        }
    }
}