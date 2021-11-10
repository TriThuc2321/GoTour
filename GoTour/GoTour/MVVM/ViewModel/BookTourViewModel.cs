using GoTour.Core;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using GoTour.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class BookTourViewModel: ObservableObject
    {
        INavigation navigation;
        public Command PayingMethodCommand { get; }
        public Command NavigationBack { get; }
        public BookTourViewModel() { }

        public BookTourViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            SelectedTour = DataManager.Ins.currentTour;
            PayingMethodCommand = new Command(openPayingMethodView);

            IncreaseAmountCommand = new Command(increase);
            DescreaseAmountCommand = new Command(decrease);
            Amount = 1;

            NavigationBack = new Command(() => navigation.PopAsync());


            CheckDiscountCommand = new Command(checkDiscount);

            SetInformation();
        }

       bool checkValidation()
        {
            // Check null
            if (Name == null)
            {
                DependencyService.Get<IToast>().ShortToast("Name is null! Please enter your name");
                return false;
            }
            else if (Birthday == null)
            {
                DependencyService.Get<IToast>().ShortToast("Birthday is null! Please enter your birthday");
                return false;
            }
            else if (Contact == "")
            {
                DependencyService.Get<IToast>().ShortToast("Contact is null! Please enter your contact");
                return false;

            }
            else if (Cmnd == "")
            {
                DependencyService.Get<IToast>().ShortToast("Identity card ID is null! Please enter");
                return false;

            }
            else if (Address == "")
            {
                DependencyService.Get<IToast>().ShortToast("Address is null! Please enter your address");
                return false;

            }
            else if (Amount == 0)
            {
                DependencyService.Get<IToast>().ShortToast("Amount is null! Please enter your amount");
                return false;

            }

            //Check amount
            if (Amount == 0)
            {
                AmountNotice = "Choose your amount";
                AmountNoticeColor = Color.Red;
                AmountNoticeVisible = true;
                DependencyService.Get<IToast>().ShortToast("Amount is 0! Please enter your amount");
                return false;


            }
            else if (Amount > int.Parse(selectedTour.remaining)  )
            {
                DiscountNotice = "This discount has no more turn";
                DiscountNoticeVisible = true;
                DiscountNoticeColor = Color.DarkOrange;
                DependencyService.Get<IToast>().ShortToast("This discount has no more turn!");
                return false;

            }

            UsersServices services = new UsersServices();
            //Check contact
            if (!services.IsPhoneNumber(Contact))
            {
                ContactNotice = "Incorrect phone number";
                ContactNoticeColor = Color.Red;
                ContactNoticeVisible = true;
                DependencyService.Get<IToast>().ShortToast("Incorrect phone number!");
                return false;

            }
            //Check cmnd
            if (!services.IsCMND(Cmnd))
            {
                CmndNotice = "Incorrect identity card ID";
                CmndNoticeColor = Color.Red;
                CmndNoticeVisible = true;
                DependencyService.Get<IToast>().ShortToast("Incorrect identity card ID!");
                return false;

            }

            //Check discount 
            if (chosenDiscount == null && DiscountId=="")
            {
                DiscountNotice = "Please check your discount";
                DiscountNoticeColor = Color.Red;
                DiscountNoticeVisible = true;
                DependencyService.Get<IToast>().ShortToast("Check your discount entered, please!");
                return false;
            }
            return true;


        }
        void openPayingMethodView(object obj)
        {
            if (checkValidation())
            {
                BookedTicket ticket = new BookedTicket()
                {
                    id = (new Random().Next(999999)).ToString(),
                    tour = new Tour() { id = selectedTour.id },
                    name = Name,
                    birthday = Birthday,
                    contact = Contact,
                    email = Email,
                    cmnd = Cmnd,
                    address = Address,
                    isCancel = false,
                    invoice = new Invoice
                    {
                        id = (new Random().Next(9999999).ToString()),
                        discount = new Discount { id = DiscountId},
                        discountMoney = DiscountMoney,
                        isPaid = false,
                        amount = Amount.ToString(),
                        total = Total,
                        price = TourPrice
                    }
                };

                DataManager.Ins.CurrentBookedTicket = ticket;
                navigation.PushAsync(new PayingMethodView());
            }
        }

        Discount chosenDiscount; 
        void checkDiscount(object obj)
        {
            if (DiscountId == null)
            {
                DiscountNotice = "Please enter the discount ID";
                DiscountNoticeVisible = true;
                DiscountNoticeColor = Color.Red;
                return;
            }  
            else
            {
                foreach (var discount in DataManager.Ins.ListDiscount)
                    if (DiscountId == discount.id)
                    {
                        if (discount.isUsed == discount.total)
                        {
                            DiscountNotice = "This discount has no more turn";
                            DiscountNoticeVisible = true;
                            DiscountNoticeColor = Color.DarkOrange;
                        }    
                        else
                        {
                            DiscountNotice = "Successfully code applied";
                            DiscountNoticeVisible = true;
                            DiscountNoticeColor = Color.ForestGreen;

                            chosenDiscount = discount;

                            Provisional = (Amount * int.Parse(selectedTour.basePrice)).ToString();
                            if (chosenDiscount != null)
                                DiscountMoney = (int.Parse(chosenDiscount.percent) * int.Parse(Provisional) / 100).ToString();
                            Total = (int.Parse(Provisional) - int.Parse(DiscountMoney)).ToString();
                        }

                        return;
                    }    
            }

            DiscountNotice = "Incorrect discount ID";
            DiscountNoticeVisible = true;
            DiscountNoticeColor = Color.Red;
        }

        


        public Command CheckDiscountCommand { get; }

        #region command Amount
        public Command IncreaseAmountCommand { get; }
        void increase(object obj)
        {
            if (Amount < int.Parse(selectedTour.remaining))
            {
                Amount++;
                Provisional = (Amount * int.Parse(selectedTour.basePrice)).ToString();
                if (chosenDiscount != null)
                    DiscountMoney = (int.Parse(chosenDiscount.percent) * int.Parse(Provisional) / 100).ToString();
                Total = (int.Parse(Provisional) - int.Parse(DiscountMoney)).ToString();
            }
            else
            {
                AmountNotice = "This tour is remaining " + Amount + " tickets";
                AmountNoticeColor = Color.Red;
                AmountNoticeVisible = true;
            }

        }

        public Command DescreaseAmountCommand { get; }
        void decrease(object obj)
        {
            if (Amount > 1)
            {
                Amount--;
                Provisional = (Amount * int.Parse(selectedTour.basePrice)).ToString();
                if (chosenDiscount != null)
                    DiscountMoney = (int.Parse(chosenDiscount.percent) * int.Parse(Provisional) / 100).ToString();
                Total = (int.Parse(Provisional) - int.Parse(DiscountMoney)).ToString();
            }
        }

        #endregion


        #region user information
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _birthday;
        public string Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                OnPropertyChanged("Birthday");
            }
        }

        private string _contact;
        public string Contact
        {
            get { return _contact; }
            set
            {
                _contact = value;
                OnPropertyChanged("Contact");
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        private string _cmnd;
        public string Cmnd
        {
            get { return _cmnd; }
            set
            {
                _cmnd = value;
                OnPropertyChanged("Cmnd");
            }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }

        private string _discountId;
        public string DiscountId
        {
            get { return _discountId; }
            set
            {
                _discountId = value;
                OnPropertyChanged("DiscountId");
            }
        }

        private int _amount;
        public int Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                OnPropertyChanged("Amount");
            }
        }
        #endregion

        #region contact
        private string _contactNotice;
        public string ContactNotice
        {
            get { return _contactNotice; }
            set
            {
                _contactNotice = value;
                OnPropertyChanged("ContactNotice");
            }
        }

        private Color _contactNoticeColor;
        public Color ContactNoticeColor
        {
            get { return _contactNoticeColor; }
            set
            {
                _contactNoticeColor = value;
                OnPropertyChanged("ContactNoticeColor");
            }
        }

        private bool _contactNoticeVisible;
        public bool ContactNoticeVisible
        {
            get { return _contactNoticeVisible; }
            set
            {
                _contactNoticeVisible = value;
                OnPropertyChanged("ContactNoticeVisible");
            }
        }

        #endregion

        #region email
        private string _emailNotice;
        public string EmailNotice
        {
            get { return _emailNotice; }
            set
            {
                _emailNotice = value;
                OnPropertyChanged("EmailNotice");
            }
        }

        private Color _emailNoticeColor;
        public Color EmailNoticeColor
        {
            get { return _emailNoticeColor; }
            set
            {
                _emailNoticeColor = value;
                OnPropertyChanged("EmailNoticeColor");
            }
        }

        private bool _emailNoticeVisible;
        public bool EmailNoticeVisible
        {
            get { return _emailNoticeVisible; }
            set
            {
                _emailNoticeVisible = value;
                OnPropertyChanged("EmailNoticeVisible");
            }
        }

        #endregion

        #region cmnd
        private string _cmndNotice;
        public string CmndNotice
        {
            get { return _cmndNotice; }
            set
            {
                _cmndNotice = value;
                OnPropertyChanged("CmndNotice");
            }
        }

        private Color _cmndNoticeColor;
        public Color CmndNoticeColor
        {
            get { return _cmndNoticeColor; }
            set
            {
                _cmndNoticeColor = value;
                OnPropertyChanged("CmndNoticeColor");
            }
        }

        private bool _cmndNoticeVisible;
        public bool CmndNoticeVisible
        {
            get { return _cmndNoticeVisible; }
            set
            {
                _cmndNoticeVisible = value;
                OnPropertyChanged("CmndNoticeVisible");
            }
        }
        #endregion

        #region discount
        private string _discountNotice;
        public string DiscountNotice
        {
            get { return _discountNotice; }
            set
            {
                _discountNotice = value;
                OnPropertyChanged("DiscountNotice");
            }
        }

        private Color _discountNoticeColor;
        public Color DiscountNoticeColor
        {
            get { return _discountNoticeColor; }
            set
            {
                _discountNoticeColor = value;
                OnPropertyChanged("DiscountNoticeColor");
            }
        }

        private bool _discountNoticeVisible;
        public bool DiscountNoticeVisible
        {
            get { return _discountNoticeVisible; }
            set
            {
                _discountNoticeVisible = value;
                OnPropertyChanged("DiscountNoticeVisible");
            }
        }
        #endregion

        #region amount
        private string _amountNotice;
        public string AmountNotice
        { 
            get { return _amountNotice; }
            set
            {
                _amountNotice = value;
                OnPropertyChanged("AmountNotice");
            }
        }

        private Color _amountNoticeColor;
        public Color AmountNoticeColor
        {
            get { return _amountNoticeColor; }
            set
            {
                _cmndNoticeColor = value;
                OnPropertyChanged("AmountNoticeColor");
            }
        }

        private bool _amountNoticeVisible;
        public bool AmountNoticeVisible
        {
            get { return _amountNoticeVisible; }
            set
            {
                _amountNoticeVisible = value;
                OnPropertyChanged("AmountNoticeVisible");
            }
        }

        
        #endregion

        #region price
        private string _tourPrice;
        public string TourPrice
        {
            get { return _tourPrice; }
            set
            {
                _tourPrice = value;
                OnPropertyChanged("TourPrice");
            }
        }

        private string _provisional;
        public string Provisional
        {
            get { return _provisional; }
            set
            {
                _provisional = value;
                OnPropertyChanged("Provisional");
            }
        }

        private string _discountMoney;
        public string DiscountMoney
        {
            get { return _discountMoney; }
            set
            {
                _discountMoney = value;
                OnPropertyChanged("DiscountMoney");
            }
        }

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

        #region selectedTour
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
        #endregion
        void SetInformation()
        {
            User getUser = DataManager.Ins.CurrentUser;
            Name = getUser.name;
            //Birthday = getUser.birthday;

            Contact = getUser.contact;
            ContactNoticeVisible = false;

            Email = getUser.email;
            EmailNotice = "This email is your register account email";
            EmailNoticeColor = Color.ForestGreen;
            EmailNoticeVisible = true;

            Cmnd = getUser.cmnd;
            ContactNoticeVisible = false;

            Address = getUser.address;

            DiscountNotice = "Enter and press to check the validation of your code";
            DiscountNoticeColor = Color.ForestGreen;
            DiscountNoticeVisible = true;

            TourPrice = Provisional = Total = selectedTour.basePrice;
            DiscountMoney = "0";
        }
    }
}
