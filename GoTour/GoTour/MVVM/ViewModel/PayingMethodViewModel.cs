using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class PayingMethodViewModel: ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
       
        public Command NavigationBack { get; }
        public Command Confirm{ get; }
        public PayingMethodViewModel() { }
        public PayingMethodViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
           
            SetInformation();
            checkDateRegulation();
            SelectedTour = DataManager.Ins.currentTour;
            NavigationBack = new Command(() => navigation.PopAsync());
            Confirm = new Command(confirmPress);
        }

        async void confirmPress(object obj)
        {
            if (!IsCheckRegulation)
            {
                DependencyService.Get<IToast>().ShortToast("Please check regulation box");
            }

            if (!MoMo && !Cash)
            {
                DependencyService.Get<IToast>().ShortToast("Please choose a paying method");
            }

            if (await checkDiscount() == false) 
            { 
                return;
            }
            if (await checkRemaining() == false) 
            { 
                return; 
            }

            if (IsCheckRegulation && MoMo)
            {
                ConfirmEnable = false;
                DataManager.Ins.CurrentBookedTicket.invoice.method = "MoMo";
                navigation.PushAsync(new MoMoConfirmView());
            }
            else if (IsCheckRegulation && Cash)
            {
                ConfirmEnable = false;
                DataManager.Ins.CurrentInvoice.method = "Cash";
                DataManager.Ins.CurrentInvoice.isPaid = false;
                DataManager.Ins.CurrentInvoice.payingTime = DateTime.Now.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));

                DataManager.Ins.CurrentBookedTicket.bookTime = DateTime.Now.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));

                await DataManager.Ins.InvoicesServices.AddInvoice(DataManager.Ins.CurrentInvoice);
                await DataManager.Ins.BookedTicketsServices.AddBookedTicket(DataManager.Ins.CurrentBookedTicket);

                if (DataManager.Ins.CurrentDiscount != null)
                {
                    int isUsed = int.Parse(DataManager.Ins.CurrentDiscount.isUsed);
                    isUsed++;
                    DataManager.Ins.CurrentDiscount.isUsed = isUsed.ToString();

                    await DataManager.Ins.DiscountsServices.UpdateDiscount(DataManager.Ins.CurrentDiscount);

                }

                if (DataManager.Ins.currentTour != null)
                {
                    int remaining = int.Parse(DataManager.Ins.currentTour.remaining);
                    remaining = remaining - int.Parse(DataManager.Ins.CurrentInvoice.amount);
                    DataManager.Ins.currentTour.remaining = remaining.ToString();

                    await DataManager.Ins.TourServices.UpdateTour(DataManager.Ins.currentTour);
                }


                DependencyService.Get<IToast>().ShortToast("Booked this tour successfully!");
                updateManager();
               // await currentShell.GoToAsync($"//{nameof(HomeView)}");
                 await navigation.PushAsync(new SuccessBookView());



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

        #region TotalPrice 
        private string _total;
        public string Total
        {
            get { return _total; }
            set
            {
                _total = value;
                OnPropertyChanged("Total");
            }
        }
        #endregion

        private bool confirmEnable;
        public bool ConfirmEnable
        {
            get { return confirmEnable; }
            set
            {
                confirmEnable = value;
                OnPropertyChanged("ConfirmEnable");
            }
        }
        async Task<bool> checkRemaining()
        {
            Tour temp = await DataManager.Ins.TourServices.FindTourById(DataManager.Ins.currentTour.id);
            if (int.Parse(DataManager.Ins.CurrentInvoice.amount) <= int.Parse(temp.remaining))
                return true;
            else
                DependencyService.Get<IToast>().ShortToast("This tour has no turn!");

            return false;
        }

        async Task<bool> checkDiscount()
        {
            if (DataManager.Ins.CurrentDiscount == null) return true;
            if (DataManager.Ins.CurrentDiscount != null)
            {
                Discount temp = await DataManager.Ins.DiscountsServices.FindDiscountById(DataManager.Ins.CurrentDiscount.id);
                if (int.Parse(temp.isUsed) < int.Parse(temp.total))
                    return true;
                else
                    DependencyService.Get<IToast>().ShortToast("Your discount has no turn!");
            };

            return false;
        }
      
        
        void updateManager()
        {
            DataManager.Ins.CurrentBookedTicket.tour = DataManager.Ins.currentTour;
            DataManager.Ins.CurrentBookedTicket.invoice = DataManager.Ins.CurrentInvoice;

            if (DataManager.Ins.CurrentDiscount != null)
                DataManager.Ins.CurrentBookedTicket.invoice.discount = DataManager.Ins.CurrentDiscount;


            DataManager.Ins.ListBookedTickets.Add(DataManager.Ins.CurrentBookedTicket);
            DataManager.Ins.ListInvoice.Add(DataManager.Ins.CurrentInvoice);

            DataManager.Ins.CurrentBookedTicket.invoice = DataManager.Ins.CurrentInvoice;

            if (DataManager.Ins.CurrentDiscount != null)
            {
                DataManager.Ins.CurrentBookedTicket.invoice.discount = DataManager.Ins.CurrentDiscount;

                for (int i = 0; i < DataManager.Ins.ListDiscount.Count - 1; i++)
                {
                    if (DataManager.Ins.ListDiscount[i].id == DataManager.Ins.CurrentDiscount.id)
                    {
                        DataManager.Ins.ListDiscount[i] = DataManager.Ins.CurrentDiscount;
                        break;
                    }
                }

               
            }
        }

        void checkDateRegulation()
        {
            LaterNoticeVisible = false;
            PermitCheckCash = true;
            LaterNotice = "";
            if (DataManager.Ins.BookedTicketsServices.countBookTourRegulation(DataManager.Ins.currentTour) < 2)
            {
                PermitCheckCash = false;
                LaterNotice = "Since you book the tour 2 days before it starts, you are required to pay for it by Momo/Banking ";
                MoMo = true;
                LaterNoticeVisible = true;
            }    

        }

        private string _laterNotice;
        public string LaterNotice
        {
            get { return _laterNotice; }
            set
            {
                _laterNotice = value;
                OnPropertyChanged("LaterNotice");
            }
        }

        private bool _laterNoticeVisible;
        public bool LaterNoticeVisible
        {
            get { return _laterNoticeVisible; }
            set
            {
                _laterNoticeVisible = value;
                OnPropertyChanged("LaterNoticeVisible");
            }
        }

        private bool _permitCheckCash;
        public bool PermitCheckCash
        {
            get { return _permitCheckCash; }
            set
            {
                _permitCheckCash = value;
                OnPropertyChanged("PermitCheckCash");
            }
        }

          void SetInformation()
        {
            confirmEnable = true;
            Total = DataManager.Ins.CurrentInvoice.total;

            var service = DataManager.Ins.InvoicesServices;
            Total = service.FormatMoney(Total);

            var rules = DataManager.Ins.Rule;

            Regulation = "Right after payment or before 10 days: cancel " + rules.deduct[0] + "% of tour price.";

            for (int i= 1; i< rules.deduct.Count-1; i++)
            {
                Regulation += "\nCancellation 5 days before the start date: cancel " + rules.deduct[i] + "% of tour price.";
            }

            Regulation += "\nCancel 1 day before the start of the day: cancel " + rules.deduct[rules.deduct.Count - 1] + "% of the tour fee case of late arrival feature is canceled 5 days before the start date. ";

        }  

    }
}
