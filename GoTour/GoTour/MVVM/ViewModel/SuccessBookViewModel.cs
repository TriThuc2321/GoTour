using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class SuccessBookViewModel: ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public Command GoToHome { get; }
        public SuccessBookViewModel() { }
        public SuccessBookViewModel(INavigation navigation, Shell current)
        {
            this.navigation = navigation;
            this.currentShell = current;

            this.Ticket = DataManager.Ins.CurrentBookedTicket;
            if (DataManager.Ins.CurrentDiscount != null)
                this.Discount = DataManager.Ins.CurrentDiscount;
            this.Invoice = DataManager.Ins.CurrentInvoice;
            this.Tour = DataManager.Ins.currentTour;

            GoToHome = new Command(goHome);

            //  UploadPhoto = new Command(upload);
            SetInformation();
        }

        void goHome(object obj)
        {
            currentShell.GoToAsync($"//{nameof(HomeView)}");
        }

        void SetInformation()
        {
            DurationProcess();



            if (!this.Invoice.isPaid)
            {
                // PayingVisible = false;
                DisplayUpload = true;
            }
            else
                DisplayUpload = false;

            checkTourStatus(Tour);

            if (Ticket != null && Ticket.isCancel)
                Occured = Occured + " - This ticket was canceled";

            if (Invoice.isPaid)
                Paid = "Yes";
            else
                Paid = "No";

            if (Invoice.method == "MoMo")
                payingPhotoVisible = true;
            else payingPhotoVisible = false;

            if (Discount != null)
            {
                if (this.Discount.id != null)
                    DiscountVisible = true;
            }
            else
                DiscountVisible = false;

            FormatMoney();

        }

        void viewDetail(object obj)
        {

            navigation.PushAsync(new DetailTourView());
        }
        void cancelTicket(object obj)
        {
            navigation.PushAsync(new CancelTourView());
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
                _ticket = value;
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


        private bool payingPhotoVisible;
        public bool PayingPhotoVisible
        {
            get { return payingPhotoVisible; }
            set
            {
                payingPhotoVisible = value;
                OnPropertyChanged("PayingPhotoVisible");
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

        private bool cancelVisible;
        public bool CancelVisible
        {
            get { return cancelVisible; }
            set
            {
                cancelVisible = value;
                OnPropertyChanged("CancelVisible");
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

        public void checkTourStatus(Tour tour)
        {
            CancelVisible = true;

            string[] tourStartTime = tour.startTime.Split('/');

            string[] splitYear = tourStartTime[2].Split(' ');
            DateTime timeStart = new DateTime(
                int.Parse(splitYear[0]),
                int.Parse(tourStartTime[1]),
                int.Parse(tourStartTime[0])
                );

            string[] duration = tour.duration.Split('/');


            DateTime currentTime = DateTime.Now.AddDays(0);
            TimeSpan interval = timeStart.Subtract(currentTime);

            string maxDuration = int.Parse(duration[0]) > int.Parse(duration[1]) ? duration[0] : duration[1];

            maxDuration = (int.Parse(maxDuration) * 24 * 60 * 60).ToString();

            // Thoi gian bat dau tour den current time
            double count = interval.Seconds;
            if (count > 0)
            {
                Occured = "Not occured";
                return;
            }
            if (count <= 0 && Math.Abs(count) <= int.Parse(maxDuration))
            {
                Occured = "Occuring";
                CancelVisible = false;
                return;
            }

            Occured = "Occured";
            CancelVisible = false;
        }
    }
}
