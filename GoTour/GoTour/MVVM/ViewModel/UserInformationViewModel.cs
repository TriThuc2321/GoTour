using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class UserInformationViewModel
    {
        INavigation navigation;
        Shell curentShell;
        public UserInformationViewModel() { }
        public UserInformationViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.curentShell = curentShell;
        }
    }
}
