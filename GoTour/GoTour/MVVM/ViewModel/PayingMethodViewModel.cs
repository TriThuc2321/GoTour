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
            PermitCheckCash = true;
            LaterNotice = "";
            if (DataManager.Ins.BookedTicketsServices.countBookTourRegulation(DataManager.Ins.currentTour) < 5)
            {
                PermitCheckCash = false;
                LaterNotice = "Since you book the tour 5 days before it starts, you are required to pay for it by Momo/Banking ";
                MoMo = true;
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

        private string _laterNoticeVisible;
        public string LaterNoticeVisible
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

            Total = DataManager.Ins.CurrentInvoice.total;

            var service = DataManager.Ins.InvoicesServices;
            Total = service.FormatMoney(Total);

            Regulation = "\tIn case of service cancellation from customers: For instance, you can not use your service/tour, you must notify the Company in writing or by email (Do not handle with the case of contact transfer/cancellation by phone). At the same time, please bring the tour/service registration record & payment receipt to GoTour office for cancelling/transfering tour procedures. In case of service / tour cancellation: You must bear the cost of canceling the tour / service in accordance with GoTour's regulations and the entire budget for online payment work. \nRight after payment or before 10 days: cancel 30% of tour price. \nCancellation 5 days before the start date: cancel 50% of the tour price. \nCancellation 3 days before the start date: cancel 70% of the tour fee \nCancel 1 day before the start of the day: cancel 100% of the tour fee case of late arrival feature is canceled 5 days before the start date. \n\tYou can pay for the tour right away with momo e-wallet, the aggregate unit will be converted into VND. If you choose to pay in cash, please pay 5 days in advance from the time of tour start. \n\tOTHER NOTES \nOn departure day, please gather at the designated pick-up point. If you arrive late, the Company does not bear any related responsibility. At the end of the tour, Vietnam Booking will drop off guests at a single point, the Hanoi Opera House. Please take a taxi to your hotel or accommodation. In case there are 6 or less passengers on the departure date, the driver can act as a guide. These are experienced drivers, knowledgeable about routes and local culture. As a mountain tour, often ride small cars, so it is recommended that you do not bring bulky, oversized and do not carry a lot of luggage. Should bring backpacks instead of carrying hard suitcases. There are options to sleep at a homestay to increase the experience with the mountain tour. It is recommended that you bring personal items to use when staying at the Homestay (if necessary). In case the guest is traveling alone, we cannot arrange a roommate with other people to travel alone. must pay a separate sleeping fee equal to 50% of the room rate/night during the tour. \n\tYou must bring: legal identification (ID card or Passport). If you are a vegetarian, please bring more vegetarian food with you to ensure your taste. Any services in the tour if you do not use them, will not be refunded. The guide has the right to rearrange the items. own the attractions to suit the conditions of each specific departure date, but still ensure all the attractions in the program. You should bring: insect repellent, common cold medicine or medicine prescribed by a doctor individually. \n(*) Customer's responsibility: Customers are solely responsible for their health and chronic diseases (cardiovascular, blood pressure, diabetes, bone and joint...), congenital diseases, underlying diseases, HIV AIDS, mental and neurological disorders, pregnant women... are diseases not covered by insurance. When necessary, you must write a commitment about your illness when participating in the tour. The tour organizer is not responsible for cases where you do not declare your illness, declare dishonestly as well as cases outside the travel insurance coverage on the tour. Customers take care of their own property in all cases and in all places during the trip. The tour organizer is not responsible for the loss of money, valuables, airline tickets, and customer's private property during the trip. In objective cases such as: terrorism, natural disaster...or due to an incident, there is a change in the schedule of public transport such as: plane, train, etc., Vietnam Booking will reserve the right to change the schedule. change the route at any time for the convenience and safety of customers and will not be responsible for compensation for damages incurred. *If the number of participants does not meet the minimum number to depart, the Company will support to move to the nearest departure date and notify you in advance or refund the tour fee as deposit for you* .";
        }  

    }
}
