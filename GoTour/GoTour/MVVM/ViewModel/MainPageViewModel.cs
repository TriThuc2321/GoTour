using GoTour.Core;
using GoTour.Database;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class MainPageViewModel: ObservableObject
    {
        Shell currentShell;
        public MainPageViewModel() { }
        public MainPageViewModel(Shell currentShell)
        {
            this.currentShell = currentShell;          
        }
       
    }
}
