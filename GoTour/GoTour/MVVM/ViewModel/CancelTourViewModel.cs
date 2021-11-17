using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class CancelTourViewModel: ObservableObject
    {
        INavigation navigation;
        public CancelTourViewModel() { }
        public Command NavigationBack { get; }
        public Command CancelTicket { get; }

        public CancelTourViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            CancelTicket = new Command(cancelTicket);
            NavigationBack = new Command(() => navigation.PopAsync());

            SetInformation();

        }

        async void cancelTicket(object obj)
        {
            if (IsCheckRegulation)
            {
                DataManager.Ins.CurrentBookedTicket.isCancel = true;
                await DataManager.Ins.BookedTicketsServices.UpdateBookedTicket(DataManager.Ins.CurrentBookedTicket);

                

                await navigation.PopAsync();
            }    
        }

        void SetInformation()
        {
            SelectedTicket = DataManager.Ins.CurrentBookedTicket;
            IsCheckRegulation = false;
            Regulation = "\tIn case of service cancellation from customers: For instance, you can not use your service/tour, you must notify the Company in writing or by email (Do not handle with the case of contact transfer/cancellation by phone). At the same time, please bring the tour/service registration record & payment receipt to GoTour office for cancelling/transfering tour procedures. In case of service / tour cancellation: You must bear the cost of canceling the tour / service in accordance with GoTour's regulations and the entire budget for online payment work. \nRight after payment or before 10 days: cancel 30% of tour price. \nCancellation 5 days before the start date: cancel 50% of the tour price. \nCancellation 3 days before the start date: cancel 70% of the tour fee \nCancel 1 day before the start of the day: cancel 100% of the tour fee case of late arrival feature is canceled 5 days before the start date.";
        }

        private string _regulation;
        public string Regulation
        {
            get { return _regulation; }
            set
            {
                _regulation = value;
                OnPropertyChanged("Regulation");
            }
        }

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

        private BookedTicket selectedTicket;
        public BookedTicket SelectedTicket
        {
            get { return selectedTicket; }
            set
            {
                selectedTicket = value;
                OnPropertyChanged("SelectedTicket");
            }
        }
    }
}
