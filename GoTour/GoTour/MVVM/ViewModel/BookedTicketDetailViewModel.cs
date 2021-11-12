using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class BookedTicketDetailViewModel: ObservableObject
    {
        INavigation navigation;
        public Command NavigationBack { get; }
        public BookedTicketDetailViewModel() {}

        public BookedTicketDetailViewModel(INavigation navigation)
        {
            this.navigation = navigation;

            this.Ticket = DataManager.Ins.CurrentBookedTicket;
            this.Discount = DataManager.Ins.CurrentDiscount;
            this.Invoice = DataManager.Ins.CurrentInvoice;
            this.Tour = DataManager.Ins.currentTour;

            NavigationBack = new Command(() => navigation.PopAsync());
            SetInformation();

        }

        void SetInformation()
        {
            DurationProcess();
            PayingVisible = DiscountVisible = true;
            if (!this.Invoice.isPaid)
            {
                PayingVisible = false;
            }

            if (Discount == null)
            {
                DiscountVisible = false;
            }

            if (Tour.isOccured)
                Occured = "Occured";
            else
                Occured = "No occured";

            if (Invoice.isPaid)
                Paid = "Yes";
            else
                Paid = "No";
        }

        private BookedTicket _ticket;
        public BookedTicket Ticket
        {
            get { return _ticket; }
            set
            {
               _ticket  = value;
                OnPropertyChanged("Ticket");

            }
        }

        private Discount _discount;
        public Discount Discount
        {
            get { return _discount; }
            set
            {
                _discount = value;
                OnPropertyChanged("Discount");

            }
        }

        private Invoice _invoice;
        public Invoice Invoice
        {
            get { return _invoice; }
            set
            {
                _invoice = value;
                OnPropertyChanged("Invoice");

            }
        }

        private Tour _tour;
        public Tour Tour
        {
            get { return _tour; }
            set
            {
                _tour = value;
                OnPropertyChanged("Tour");

            }
        }

        private string processedDuration;
        public string ProcessedDuration
        {
            get { return processedDuration; }
            set
            {
                processedDuration = value;
                OnPropertyChanged("ProcessedDuration");
            }
        }


        private bool payingVisible;
        public bool PayingVisible
        {
            get { return payingVisible; }
            set
            {
                payingVisible = value;
                OnPropertyChanged("PayingVisible");
            }
        }

        private bool discountVisible;
        public bool DiscountVisible
        {
            get { return discountVisible; }
            set
            {
                discountVisible = value;
                OnPropertyChanged("DiscountVisible");
            }
        }

        private string occured;
        public string Occured
        {
            get { return occured; }
            set
            {
                occured = value;
                OnPropertyChanged("Occured");
            }
        }

        private string paid;
        public string Paid
        {
            get { return paid; }
            set
            {
                paid = value;
                OnPropertyChanged("Paid");
            }
        }
        private void DurationProcess()
        {
            if (DataManager.Ins.currentTour.duration == null) return;
            string[] _ProcessedDuration = DataManager.Ins.currentTour.duration.Split('/');
            string result = _ProcessedDuration[0] + " days - " + _ProcessedDuration[1] + " nights";
            ProcessedDuration = result;
        }
    }
}
