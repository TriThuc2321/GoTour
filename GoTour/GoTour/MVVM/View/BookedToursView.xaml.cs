using GoTour.MVVM.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoTour.MVVM.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookedToursView : ContentPage
    {
        public BookedToursView()
        {
            InitializeComponent();
            //this.BindingContext = new BookedToursViewModel(Navigation, Shell.Current);
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext = new BookedToursViewModel(Navigation, Shell.Current);
        }
    }
}