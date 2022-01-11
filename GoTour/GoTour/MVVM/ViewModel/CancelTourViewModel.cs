using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class CancelTourViewModel : ObservableObject
    {
        Shell currentShell;
        INavigation navigation;
        public CancelTourViewModel() { }
        public Command NavigationBack { get; }
        public Command CancelTicket { get; }

        public CancelTourViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            CancelTicket = new Command(cancelTicket);
            NavigationBack = new Command(() => navigation.PopAsync());

            SetInformation();

        }

        async void cancelTicket(object obj)
        {
            if (IsCheckRegulation)
            {
                BookedTicket booked = DataManager.Ins.CurrentBookedTicket;
                booked.isCancel = true;
                DataManager.Ins.CurrentBookedTicket = booked;

                await DataManager.Ins.BookedTicketsServices.UpdateBookedTicket(booked);

                if (DataManager.Ins.CurrentDiscount != null)
                {
                    int isUsed = int.Parse(DataManager.Ins.CurrentDiscount.isUsed);
                    isUsed--;
                    DataManager.Ins.CurrentDiscount.isUsed = isUsed.ToString();

                    await DataManager.Ins.DiscountsServices.UpdateDiscount(DataManager.Ins.CurrentDiscount);

                }

                if (DataManager.Ins.currentTour != null)
                {
                    int remaining = int.Parse(DataManager.Ins.currentTour.remaining);
                    remaining = remaining + int.Parse(DataManager.Ins.CurrentInvoice.amount);
                    DataManager.Ins.currentTour.remaining = remaining.ToString();

                    await DataManager.Ins.TourServices.UpdateTour(DataManager.Ins.currentTour);
                }

                await updateManager();
                DependencyService.Get<IToast>().LongToast("Canceled this tour successfully!");
                navigation.RemovePage(navigation.NavigationStack[navigation.NavigationStack.Count - 2]);
                await navigation.PopAsync();
                //await currentShell.GoToAsync($"//{nameof(HomeView)}");
            }
        }

        async Task updateManager()
        {
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


            for (int i = 0; i < DataManager.Ins.ListTour.Count - 1; i++)
            {
                if (DataManager.Ins.ListTour[i].id == DataManager.Ins.currentTour.id)
                {
                    DataManager.Ins.ListTour[i] = DataManager.Ins.currentTour;
                    break;
                }
            }
            string notiId = DataManager.Ins.GeneratePlaceId(10);
             string noti = DataManager.Ins.GetDeductInformation(DataManager.Ins.CurrentBookedTicket)[0];

            DataManager.Ins.NotiServices.ListMyNoti_System.Add(new Notification(notiId, "System", DataManager.Ins.CurrentUser.email, "False", 1, noti, DateTime.Now, "True", DataManager.Ins.currentTour.id, "Canceled Ticket: " + DataManager.Ins.currentTour.name));

            await DataManager.Ins.NotiServices.SendNoti(
                DataManager.Ins.GeneratePlaceId(10),
                "System",
                DataManager.Ins.CurrentUser.email,
                1,
                noti,
                "Canceled Ticket: " + DataManager.Ins.currentTour.name,
                DataManager.Ins.currentTour.id
                );

        }

        void SetInformation()
        {
            SelectedTicket = DataManager.Ins.CurrentBookedTicket;
            IsCheckRegulation = false;

            var rules = DataManager.Ins.Rule;

            Regulation = "Right after payment or before 10 days: cancel " + rules.deduct[0] + "% of tour price.";

            for (int i = 1; i < rules.deduct.Count - 1; i++)
            {
                Regulation += "\nCancellation 5 days before the start date: cancel " + rules.deduct[i] + "% of tour price.";
            }

            Regulation += "\nCancel 1 day before the start of the day: cancel " + rules.deduct[rules.deduct.Count - 1] + "% of the tour fee case of late arrival feature is canceled 5 days before the start date. ";

        }

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

        private BookedTicket selectedTicket;
        public BookedTicket SelectedTicket
        {
            get { return selectedTicket; }
            set
            {
                selectedTicket = value;
                OnPropertyChanged("SelectedTicket");
            }
        }
    }
}
