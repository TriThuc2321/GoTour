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
    public partial class SearchResultView : ContentPage
    {
        public SearchResultView()
        {
            InitializeComponent();
            this.BindingContext = new SearchResultViewModel(Navigation);

        }
    }
}