using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class RuleManagerViewModel: ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public Command NavigationBack { get; } 
        public Command ChangeRuleCommand { get; } 
        private readonly IMessageService _messageService;
        public RuleManagerViewModel() { }
        public RuleManagerViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
            NavigationBack = new Command(() => currentShell.FlyoutIsPresented = !currentShell.FlyoutIsPresented);
            ChangeRuleCommand = new Command(ChangeRule);
            Deduct10 = DataManager.Ins.Rule.deduct[0];
            Deduct5 = DataManager.Ins.Rule.deduct[1];
            Deduct3 = DataManager.Ins.Rule.deduct[2];
            Deduct1 = DataManager.Ins.Rule.deduct[3];
            
        }

        private void ChangeRule(object obj)
        {
            if (int.Parse(deduct10) > 100 || int.Parse(deduct10) < 0)
            {
                DependencyService.Get<IToast>().ShortToast("The value must be greater than 0 and less than 100");
                return;
            }
            if (int.Parse(deduct1) > 100 || int.Parse(deduct1) < 0)
            {
                DependencyService.Get<IToast>().ShortToast("The value must be greater than 0 and less than 100");
                return;
            }
            if (int.Parse(deduct3) > 100 || int.Parse(deduct3) < 0)
            {
                DependencyService.Get<IToast>().ShortToast("The value must be greater than 0 and less than 100");
                return;
            }
            if (int.Parse(deduct5) > 100 || int.Parse(deduct5) < 0)
            {
                DependencyService.Get<IToast>().ShortToast("The value must be greater than 0 and less than 100");
                return;
            }
            List<string> newdeduct = new List<string>();
            newdeduct.Add(deduct10);
            newdeduct.Add(deduct5);
            newdeduct.Add(deduct3);
            newdeduct.Add(deduct1);
            Rule newRule = new Rule(newdeduct);
            DataManager.Ins.RuleServices.UpdateRule(newRule);
            DependencyService.Get<IToast>().ShortToast("Change the rule successful !");
        }

        private string deduct10; 
        public string Deduct10
        {
            get { return deduct10; }
            set
            {
                if (value.Length >= 0)
                {
                    deduct10 = value;
                    OnPropertyChanged("Deduct10");
                }
            }
        }
        private string deduct5;
        public string Deduct5
        {
            get { return deduct5; }
            set
            {
                if (value.Length >= 0)
                {
                    deduct5 = value;
                    OnPropertyChanged("Deduct5");
                }
            }
        }
        private string deduct3;
        public string Deduct3
        {
            get { return deduct3; }
            set
            {
                if (value.Length >= 0)
                {
                    deduct3 = value;
                    OnPropertyChanged("Deduct3");
                }
            }
        }
        private string deduct1;
        public string Deduct1
        {
            get { return deduct1; }
            set
            {
                if (value.Length >= 0 )
                {
                    deduct1 = value;
                    OnPropertyChanged("Deduct1");
                }
            }
        }
    }
}
