using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class BookedTicketDetailViewModel: ObservableObject
    {
        INavigation navigation;
        public Command NavigationBack { get; }
        public BookedTicketDetailViewModel()
        {

        }

        public BookedTicketDetailViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            this.Ticket = DataManager.Ins.CurrentBookedTicket;

            Ticket.invoice.discount = DataManager.Ins.CurrentDiscount;

            NavigationBack = new Command(() => navigation.PopAsync());
            DurationProcess();

        }

        private BookedTicket _ticket;
        public BookedTicket Ticket
        {
            get { return _ticket; }
            set
            {
               Ticket  = value;
               
            }
        }

        private string processedDuration;
        public string ProcessedDuration
        {
            get { return processedDuration; }
            set
            {
                processedDuration = value;
                OnPropertyChanged("ProcessedDuration");
            }
        }
        private void DurationProcess()
        {
            if (DataManager.Ins.currentTour.duration == null) return;
            string[] _ProcessedDuration = DataManager.Ins.currentTour.duration.Split('/');
            string result = _ProcessedDuration[0] + " days - " + _ProcessedDuration[1] + " nights";
            ProcessedDuration = result;
        }
    }
}
