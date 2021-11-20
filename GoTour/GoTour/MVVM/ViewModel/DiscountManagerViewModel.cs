using GoTour.Core;
using GoTour.MVVM.View;
using GoTour.MVVM.ViewModel;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using GoTour.Database;
using System.Windows.Input;

namespace GoTour.MVVM.ViewModel
{
    public class DiscountManagerViewModel: ObservableObject
    {
        Shell currentShell;
        INavigation navigation;
        public Command NavigationBack { get; }
        public Command NewDiscountCommand { get; }
        public Command DeleteCommand { get; }
        public DiscountManagerViewModel() { }
        public DiscountManagerViewModel(INavigation navigation, Shell shell)
        {
            this.navigation = navigation;
            this.currentShell = shell;
            Refresh = new Command(RefreshView);
            NewDiscountCommand = new Command(addNew);
            NavigationBack = new Command(() => navigation.PopAsync());
            DeleteCommand = new Command(delete);

            SetInformation();

        }

        async void delete(object obj)
        {
            Discount result = obj as Discount;

            if (result != null && result.isUsed != "0")
            {
                DependencyService.Get<IToast>().ShortToast("Cannot delete this discount!");
                return;
            }
            else if (result != null )
            {
                Discount tmp = await DataManager.Ins.DiscountsServices.FindDiscountById(result.id);
                if (tmp!=null)
                {
                    if (tmp.isUsed != "0")
                    {
                        DependencyService.Get<IToast>().ShortToast("Cannot delete this discount! Someone used this account");
                        return;
                    }
                }    
                DiscountList.Remove(result);
                DataManager.Ins.ListDiscount.Remove(result);
                await DataManager.Ins.DiscountsServices.DeleteDiscount(result.id);
            }
        }
        void addNew(object obj)
        {
            navigation.PushAsync(new NewDiscountView());
        }   
        
        void SetInformation()
        {
            DiscountList = new ObservableCollection<Discount>();
            foreach(var dc in DataManager.Ins.ListDiscount)
            {
                DiscountList.Add(dc);
            }
            SortingDiscount();
        }

        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            Discount result = obj as Discount;
            if (result != null)
            {
                DataManager.Ins.CurrentDiscount = result;
                SelectedDiscount = null;

                navigation.PushAsync(new EditDiscountView());

            }
        });

        private ObservableCollection<Discount> _discountList;
        public ObservableCollection<Discount> DiscountList
        {
            get { return _discountList; }
            set
            {
                _discountList = value;
                OnPropertyChanged("DiscountList");
            }
        }

        private Discount selectedDiscount;
        public Discount SelectedDiscount
        {
            get { return selectedDiscount; }
            set
            {
                selectedDiscount = value;
                OnPropertyChanged("SelectedDiscount");

            }
        }

        #region Refresh
        private bool _isRefresh;
        public bool IsRefresh
        {
            get
            {
                return _isRefresh;
            }

            set
            {
                _isRefresh = value;
                OnPropertyChanged("IsRefresh");
            }
        }

        public Command Refresh { get; }
        void RefreshView(object obj)
        {
            IsRefresh = true;
            SetInformation();
            IsRefresh = false;
        }
        #endregion

        void SortingDiscount()
        {
            // Xep giam dan
            for (int i = 0; i < DiscountList.Count; i++)
            {
                for (int j = i + 1; j < DiscountList.Count - 1; j++)
                {

                    if (int.Parse(DiscountList[i].isUsed) > int.Parse(DiscountList[j].isUsed))
                    {
                        Discount tmp = new Discount();
                        tmp = DiscountList[i];
                        DiscountList[i] = DiscountList[j];
                        DiscountList[j] = tmp;
                    }
                }
            }
        }

    }
}
