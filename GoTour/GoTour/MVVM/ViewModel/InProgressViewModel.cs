using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class InProgressViewModel: ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public Command MenuCommand { get; }

        public InProgressViewModel() { }

        public InProgressViewModel(INavigation navigation, Shell shell)
        {
            this.currentShell = shell;
            this.navigation = navigation;
            TicketList = new ObservableCollection<BookedTicket>();
            Refresh = new Command(RefreshList);
            MenuCommand = new Command(openMenu);

            SetInformation();
            SortingTicket();
           
        }
        void SetInformation()
        {
            foreach (var ticket in DataManager.Ins.ListBookedTickets)
            {
                if (ticket.email == DataManager.Ins.CurrentUser.email && checkTourOccuring(ticket))
                {
                    TicketList.Add(ticket);
                }
            }
        }

        private void openMenu(object obj)
        {
            currentShell.FlyoutIsPresented = !currentShell.FlyoutIsPresented;
        }
        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            BookedTicket result = obj as BookedTicket;
            if (result != null)
            {
                DataManager.Ins.CurrentBookedTicket = result;
                DataManager.Ins.currentTour = result.tour;
                DataManager.Ins.CurrentInvoice = result.invoice;
                navigation.PushAsync(new TourScheduleView());
                SelectedTicket = null;
            }
        });

        private ObservableCollection<BookedTicket> _ticketList;
        public ObservableCollection<BookedTicket> TicketList
        {
            get { return _ticketList; }
            set
            {
                _ticketList = value;
                OnPropertyChanged("TicketList");
            }
        }


        private BookedTicket selectedTicket;
        public BookedTicket SelectedTicket
        {
            get { return selectedTicket; }
            set
            {
                selectedTicket = value;
                OnPropertyChanged("SelectedTour");

            }
        }

        public bool checkTourOccuring(BookedTicket ticket)
        {
            Tour tour = ticket.tour;

            if (ticket.isCancel)
                return false;

            string[] tourStartTime = tour.startTime.Split('/');

            string[] splitYear = tourStartTime[2].Split(' ');
            DateTime timeStart = new DateTime(
                int.Parse(splitYear[0]),
                int.Parse(tourStartTime[0]),
                int.Parse(tourStartTime[1])
                );

            string[] duration = tour.duration.Split('/');


            DateTime currentTime = DateTime.Now.AddDays(0);
            TimeSpan interval = timeStart.Subtract(currentTime);

            string maxDuration = int.Parse(duration[0]) > int.Parse(duration[1]) ? duration[0] : duration[1];
            maxDuration = (int.Parse(maxDuration) * 24 * 60 * 60).ToString();

            double count = interval.Days * 60 * 60 *24;

            if (count <= 0 && Math.Abs(count) <= int.Parse(maxDuration))
            {
                return true;
            }

            return false;
        }

        void SortingTicket()
        {
            // Xep giam dan
            for (int i =0; i<TicketList.Count -1; i++)
            {
                for (int j = i+1; j< TicketList.Count; j ++)
                {
                    if (DateTime.Parse(TicketList[i].bookTime) < DateTime.Parse(TicketList[j].bookTime))
                    {
                        BookedTicket tmp = new BookedTicket();
                        tmp = TicketList[i];
                        TicketList[i] = TicketList[j];
                        TicketList[j] = tmp;
                    }    
                }    
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
        void RefreshList(object obj)
        {
            IsRefresh = true;
            TicketList.Clear();
            TicketList = new ObservableCollection<BookedTicket>();
            SetInformation();
            IsRefresh = false;
        }
        #endregion
    }
}



