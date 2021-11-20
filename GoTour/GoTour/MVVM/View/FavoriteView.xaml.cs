using GoTour.MVVM.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoTour.MVVM.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoriteView : ContentPage
    {
        public FavoriteView()
        {
            InitializeComponent();
            this.BindingContext = new FavoriteViewModel(Navigation, Shell.Current);
        }
    }
}