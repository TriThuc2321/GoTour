using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class PayingMethodViewModel: ObservableObject
    {
        INavigation navigation;
        public PayingMethodViewModel() { }
        public PayingMethodViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            SelectedTour = DataManager.Ins.currentTour;
        }

        private Tour selectedTour;
        public Tour SelectedTour
        {
            get { return selectedTour; }
            set
            {
                selectedTour = value;
                OnPropertyChanged("SelectedTour");
            }
        }

        private string _method;
        public string Method
        {
            get { return _method; }
            set
            {
                _method = value;
                OnPropertyChanged("Method");
            }
        }
    }
}
