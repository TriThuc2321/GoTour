using GoTour.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class EditDiscountViewModel:ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public EditDiscountViewModel() { }
        public EditDiscountViewModel(INavigation navigation, Shell current)
        {
            this.navigation = navigation;
            this.currentShell = current;
        }
    }
}
