using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using Plugin.Media;
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
        public Command UploadPhoto { get; }
        public Command CancelTicket { get; }
        public Command ViewDetail { get; }
        public BookedTicketDetailViewModel(INavigation navigation)
        {
            this.navigation = navigation;

            this.Ticket = DataManager.Ins.CurrentBookedTicket;
            this.Discount = DataManager.Ins.CurrentDiscount;
            this.Invoice = DataManager.Ins.CurrentInvoice;
            this.Tour = DataManager.Ins.currentTour;

            CancelTicket = new Command(cancelTicket);
            ViewDetail = new Command(viewDetail);

            NavigationBack = new Command(() => navigation.PopAsync());
          //  UploadPhoto = new Command(upload);
            SetInformation();

        }

        void SetInformation()
        {
            DurationProcess();
            PayingVisible = DiscountVisible = true;
            if (!this.Invoice.isPaid)
            {
                PayingVisible = false;
                DisplayUpload = true;
            }
            else
                DisplayUpload = false;
            if (Discount == null)
            {
                DiscountVisible = false;
            }

            if (Tour.isOccured)
                Occured = "Occured";
            else
                Occured = "No occured";

            if (Ticket.isCancel)
                Occured = Occured + " - This ticket was canceled";

            if (Invoice.isPaid)
                Paid = "Yes";
            else
                Paid = "No";

            FormatMoney();
        }

        void viewDetail(object obj)
        {
            navigation.PushAsync(new DetailTourView());
        }
        void cancelTicket(object obj)
        {

        }
        //public void upload(object obj)
        //{
        //    navigation.PushAsync(new MoMoConfirmView());
        //}

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

        private bool displayUpload;
        public bool DisplayUpload
        {
            get { return displayUpload; }
            set
            {
                displayUpload = value;
                OnPropertyChanged("DisplayUpload");
            }
        }


        private void DurationProcess()
        {
            if (DataManager.Ins.currentTour.duration == null) return;
            string[] _ProcessedDuration = DataManager.Ins.currentTour.duration.Split('/');
            string result = _ProcessedDuration[0] + " days - " + _ProcessedDuration[1] + " nights";
            ProcessedDuration = result;
        }


        private string _strBasePrice;
        public string StrBasePrice
        {
            get { return _strBasePrice; }
            set
            {
                _strBasePrice = value;
                OnPropertyChanged("StrBaserPrice");
            }
        }

        private string _strProvisional;
        public string StrProvisional
        {
            get { return _strProvisional; }
            set
            {
                _strProvisional = value;
                OnPropertyChanged("StrProvisional");
            }
        }

        private string _strDiscountMoney;
        public string StrDiscountMoney
        {
            get { return _strDiscountMoney; }
            set
            {
                _strDiscountMoney = value;
                OnPropertyChanged("StrDiscountMoney");
            }
        }

        private string _strTotal;
        public string StrTotal
        {
            get { return _strTotal; }
            set
            {
                _strTotal = value;
                OnPropertyChanged("StrTotal");
            }
        }

        private string _strMoMoVND;
        public string StrMoMoVND
        {
            get { return _strMoMoVND; }
            set
            {
                _strMoMoVND = value;
                OnPropertyChanged("StrMoMoVND");
            }
        }

        void FormatMoney()
        {
            StrTotal = Invoice.total;
            StrBasePrice = Invoice.price;
            if (Invoice.discount != null)
                StrDiscountMoney = Invoice.discountMoney;

           
            var service = DataManager.Ins.InvoicesServices;

            StrTotal = service.FormatMoney(StrTotal);
            StrBasePrice = service.FormatMoney(StrBasePrice);

            if (Invoice.discount != null)
                StrDiscountMoney = service.FormatMoney(StrDiscountMoney);

            if (Invoice.momoVnd != null)
            {
                StrMoMoVND = Invoice.momoVnd;
                StrMoMoVND = service.FormatMoney(StrMoMoVND) + " VND";
            }
           
        }
    }
}
