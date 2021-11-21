using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class NewDiscountViewModel: ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public Command NavigationBack { get; }
        public Command AddCommand { get; }
        public NewDiscountViewModel() { }
        public NewDiscountViewModel(INavigation navigation, Shell current)
        {
            this.navigation = navigation;
            this.currentShell = current;
            NavigationBack = new Command(() => navigation.PopAsync());

            AddCommand = new Command(Add);

            Notice = "";
            NoticeVisible = false;

            Percent = Total = "";
        }

        async void Add(object obj)
        {
            if (await checkValid())
            {
                Discount discount = new Discount()
                {
                    id = Id,
                    percent = Percent,
                    total = Total,
                    isUsed = "0"
                };

                await DataManager.Ins.DiscountsServices.addDiscount(discount);
                DataManager.Ins.ListDiscount.Add(discount);

                DependencyService.Get<IToast>().ShortToast("Adding discount successfully");


                await navigation.PopAsync();

            }    
        }

        async Task<bool> checkValid()
        {
            Notice = "";
            NoticeVisible = false;

            if (Id == "")
            {
                Notice += "Please enter discount ID. ";
                NoticeVisible = true;
                return false;
            }

            if (Percent == "")
            {
                Notice += "Please enter percent. ";
                NoticeVisible = true;
                return false;
            }

            if (Total == "")
            {
                Notice += "Please enter total. ";
                NoticeVisible = true;
                return false;
            }

            if (Id.Length > 10)
            {
                Notice += "Discount ID length is less than 10. ";
                NoticeVisible = true;
                return false;
            }

            Discount tmp = await DataManager.Ins.DiscountsServices.FindDiscountById(Id);
            if (tmp != null)
            {
                Notice += "This iscount ID is existing. Please change the new one.";
                NoticeVisible = true;
                return false;
            }

            if (int.Parse(Percent) > 100)
            {
                Notice += "Percent must be less than 100. ";
                NoticeVisible = true;
                return false;
            }   
           

            if (Percent.Contains(".") || Percent.Contains(","))
            {
                Notice += "Please enter an interger number for percent. ";
                NoticeVisible = true;
                return false;
            }

            if (Total.Contains(".") || Total.Contains(","))
            {
                Notice += "Please enter an interger number for total. ";
                NoticeVisible = true;
                return false;
            }

            return true;
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");

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

        private string _percent;
        public string Percent
        {
            get { return _percent; }
            set
            {
                _percent = value;
                OnPropertyChanged("Percent");

            }
        }

        private string _notice;
        public string Notice
        {
            get { return _notice; }
            set
            {
                _notice = value;
                OnPropertyChanged("Notice");

            }
        }

        private bool _noticeVisible;
        public bool NoticeVisible
        {
            get { return _noticeVisible; }
            set
            {
                _noticeVisible = value;
                OnPropertyChanged("NoticeVisible");

            }
        }

         bool IsNumber(string pValue)
        {
            foreach (Char c in pValue)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }

    }
}
