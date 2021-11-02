﻿using GoTour.Core;
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
        public BookTourViewModel() { }

        public BookTourViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            SelectedTour = DataManager.Ins.currentTour;
            PayingMethodCommand = new Command(openPayingMethodView);
        }

        void openPayingMethodView(object obj)
        {
            navigation.PushAsync(new PayingMethodView());
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

        private string _amount;
        public string Amount
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

        private string _discount;
        public string Discount
        {
            get { return _discount; }
            set
            {
                _discount = value;
                OnPropertyChanged("Discount");
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
    }
}
