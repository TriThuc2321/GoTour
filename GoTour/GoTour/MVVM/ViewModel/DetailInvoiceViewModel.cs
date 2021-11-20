using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class DetailInvoiceViewModel : ObservableObject
    {

        INavigation navigation;
        public Command NavigationBack { get; }
        public DetailInvoiceViewModel(){}

        public DetailInvoiceViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            NavigationBack = new Command(() => navigation.PopAsync());
            SelectedBookedSticket = DataManager.Ins.CurrentBookedTicket;

     
        }


        private BookedTicket selectedBookedSticket;
        public BookedTicket SelectedBookedSticket
        {
            get { return selectedBookedSticket; }
            set
            {
                selectedBookedSticket = value;
                OnPropertyChanged("SelectedBookedSticket");
            }
        }


    }
}
