using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class PayingMethodViewModel: ObservableObject
    {
        INavigation navigation;
        public Command NavigationBack { get; }
        public Command Confirm{ get; }
        public PayingMethodViewModel() { }
        public PayingMethodViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            SelectedTour = DataManager.Ins.currentTour;
            NavigationBack = new Command(() => navigation.PopAsync());
            Confirm = new Command(confirmPress);
        }

        void confirmPress(object obj)
        {
            if (!IsCheckRegulation)
                DependencyService.Get<IToast>().ShortToast("Please check regulation box");

            if (!MoMo && !Cash)
                DependencyService.Get<IToast>().ShortToast("Please choose a paying method");

            if (IsCheckRegulation && MoMo)
                navigation.PushAsync(new MoMoConfirmView());
            else if (IsCheckRegulation && Cash)
            {
                DependencyService.Get<IToast>().ShortToast("Booked this tour successfully!");
                navigation.PushAsync(new BookedTicketDetailView());
            }

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

        #region method
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
        #endregion

        #region regulation
        private bool _isCheckRegulation;
        public bool IsCheckRegulation
        {
            get { return _isCheckRegulation; }
            set
            {
                _isCheckRegulation = value;
                OnPropertyChanged("IsCheckRegulation");
            }
        }
        #endregion

        #region MoMo & Cash
        private bool _momo;
        public bool MoMo
        {
            get { return _momo; }
            set
            {
                _momo = value;
                OnPropertyChanged("MoMo");
            }
        }

        private bool _cash;
        public bool Cash
        {
            get { return _cash; }
            set
            {
                _cash = value;
                OnPropertyChanged("Cash");
            }
        }
        #endregion
    }
}
