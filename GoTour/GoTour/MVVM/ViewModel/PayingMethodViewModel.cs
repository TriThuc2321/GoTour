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
       
        public Command NavigationBack { get; }
        public Command Confirm{ get; }
        public PayingMethodViewModel() { }
        public PayingMethodViewModel(INavigation navigation)
        {
            this.navigation = navigation;
           
            SetInformation();

            SelectedTour = DataManager.Ins.currentTour;
            NavigationBack = new Command(() => navigation.PopAsync());
            Confirm = new Command(confirmPress);
        }

        async void confirmPress(object obj)
        {
            if (!IsCheckRegulation)
                DependencyService.Get<IToast>().ShortToast("Please check regulation box");

            if (!MoMo && !Cash)
                DependencyService.Get<IToast>().ShortToast("Please choose a paying method");

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
                DataManager.Ins.CurrentBookedTicket.invoice.method = "MoMo";
                navigation.PushAsync(new MoMoConfirmView());
            }
            else if (IsCheckRegulation && Cash)
            {
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
                await navigation.PushAsync(new BookedTicketDetailView());

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
        void SetInformation()
        {

            Total = DataManager.Ins.CurrentInvoice.total;

            var service = DataManager.Ins.InvoicesServices;
            Total = service.FormatMoney(Total);

            Regulation = "This is our regulation:";


        }  
        
        void updateManager()
        {
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
    }
}
