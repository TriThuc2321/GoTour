using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class RevenueManagerViewModel : ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        Hashtable monthHashtable = new Hashtable();
        public Command SortByTotalPrice { get; }
        public Command SortByPercent { get; }
        public Command Filter { get; }
        public Command NavigationBack { get; }

        private readonly IMessageService _messageService;
        public RevenueManagerViewModel() { }
        public RevenueManagerViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
            SortByTotalPrice = new Command(SortListByTotal);
            SortByPercent = new Command(SortListByPercent);
            Filter = new Command(FilterList);
            NavigationBack = new Command(() => navigation.PopAsync());
            InitList();
        }

        private void FilterList(object obj)
        {
            if (selectedYear == null && SelectedMonth != null)
            {
                ResetListValue();
                SortByMonth();
            }
            else if (selectedYear != null && selectedMonth == null)
            {
                ResetListValue();
                SortByYear();
            }
            else if (selectedYear == null && selectedMonth == null)
            {
                return;
            }
            else
            {
                ResetListValue();
                SortByMonth();
                SortByYear();
            }
        }

        private void SortByMonth()
        {
            if (selectedMonth == "None") return;
            ObservableCollection<SupportRevenue> newResult = new ObservableCollection<SupportRevenue>();
            foreach (SupportRevenue ite in ListRevenue)
            {
                if (ite.Month == selectedMonth) newResult.Add(ite);
            }
            ListRevenue.Clear();
            ListRevenue = newResult;
        }

        private void SortByYear()
        {
            if (selectedYear == "None") return;
            ObservableCollection<SupportRevenue> newResult = new ObservableCollection<SupportRevenue>();
            foreach (SupportRevenue ite in ListRevenue)
            {
                if (ite.Year == selectedYear) newResult.Add(ite);
            }
            ListRevenue.Clear();
            ListRevenue = newResult;
        }

        private void InitList()
        {
            //init hashtable and list month for picker
            monthHashtable.Add("January", "1");
            monthHashtable.Add("February", "2");
            monthHashtable.Add("March", "3");
            monthHashtable.Add("April", "4");
            monthHashtable.Add("May", "5");
            monthHashtable.Add("June", "6");
            monthHashtable.Add("July", "7");
            monthHashtable.Add("August", "8");
            monthHashtable.Add("September", "9");
            monthHashtable.Add("Octorber", "10");
            monthHashtable.Add("November", "11");
            monthHashtable.Add("December", "12");
            monthHashtable.Add("None", "None");
            listMonth = new List<string>();
            listMonth.Add("January");
            listMonth.Add("February");
            listMonth.Add("March");
            listMonth.Add("April");
            listMonth.Add("May");
            listMonth.Add("June");
            listMonth.Add("July");
            listMonth.Add("August");
            listMonth.Add("September");
            listMonth.Add("Octorber");
            listMonth.Add("November");
            listMonth.Add("December");
            listMonth.Add("None");

            /// init hashtable list year 
            ListYear = new List<string>();
            for (int i = 2020; i <= DateTime.Now.AddDays(0).Year; i++)
            {
                listYear.Add(i.ToString());
            }
            listYear.Add("None");

            ListTour = DataManager.Ins.ListTour;
            ListRevenue = new ObservableCollection<SupportRevenue>();

            foreach (Tour ite in ListTour)
            {
                if (ite.isOccured == true)
                {
                    string[] temp = ite.startTime.Split(' ');
                    string[] TourStartTime = temp[0].Split('/');
                    SupportRevenue supportRevenue = new SupportRevenue(ite, TourStartTime[0], TourStartTime[2], "0", "0", "0","Yellow" ,"99");
                    ListRevenue.Add(supportRevenue);
                }
            }
            ListInvoice = new ObservableCollection<Invoice>();

            ListBookedSticket = new ObservableCollection<BookedTicket>();

            foreach (BookedTicket ite in DataManager.Ins.ListBookedTickets)
            {
                if (ite.invoice.IsPaid == true)
                {
                    foreach (SupportRevenue ite2 in ListRevenue)
                    {
                        if (ite2.host.id == ite.tour.id)
                        {
                            ite2.totalPrice = (int.Parse(ite2.totalPrice) + int.Parse(ite.invoice.total)).ToString();
                            ite2.totalticket = (int.Parse(ite2.totalticket) + int.Parse(ite.invoice.amount)).ToString();
                            ite2.percent = ((int.Parse(ite2.totalticket) * 100) / int.Parse(ite2.host.passengerNumber)).ToString();                           
                        }
                    }
                    ListBookedSticket.Add(ite);
                }
            }
            foreach (SupportRevenue ite2 in ListRevenue)
            {             
                    if (int.Parse(ite2.percent) > 75) ite2.color = "LightGreen";
                    if (int.Parse(ite2.percent) < 50) ite2.color = "Red";           
            }

            InitBackupList();
        


        }

        private void InitBackupList()
        {
            ListBackupRevenue = new ObservableCollection<SupportRevenue>();

            foreach (SupportRevenue ite2 in ListRevenue)
                ListBackupRevenue.Add(ite2);
        }
        private void ResetListValue()
        {
            ListRevenue = new ObservableCollection<SupportRevenue>();

            foreach (SupportRevenue ite2 in ListBackupRevenue)
                ListRevenue.Add(ite2);
        }

        void SortListByTotal(object obj)
        {
            SupportRevenue temp;
            for (int i = 0; i < ListRevenue.Count-1; i++)
            {
                for (int j = i + 1; j < ListRevenue.Count; j++)
                {
                    if (int.Parse(ListRevenue[i].totalPrice) < int.Parse(ListRevenue[j].totalPrice))
                    {
                        temp = ListRevenue[i];
                        ListRevenue[i] = ListRevenue[j];
                        ListRevenue[j] = temp;
                    }
                }
            }
            for (int i = 0; i < ListRevenue.Count; i++)
                ListRevenue[i].Rank = (i+1).ToString();
            


        }
        void SortListByPercent()
        {
            SupportRevenue temp;            for (int i = 0; i < ListRevenue.Count -1; i++)
            {
                for (int j = i + 1; j < ListRevenue.Count; j++)
                {
                    if (int.Parse(ListRevenue[i].percent) < int.Parse(ListRevenue[j].percent))
                    {
                        temp = ListRevenue[i];
                       
                        ListRevenue[i] = ListRevenue[j];
                        ListRevenue[j] = temp;
                    }
                }
            }
            for (int i = 0; i < ListRevenue.Count; i++)
                ListRevenue[i].Rank = (i+1).ToString();
        }

        private ObservableCollection<BookedTicket> listBookedSticket;
        public ObservableCollection<BookedTicket> ListBookedSticket
        {
            get { return listBookedSticket; }
            set
            {
                listBookedSticket = value;
                OnPropertyChanged("ListBookedSticket");
            }
        }
        private ObservableCollection<SupportRevenue> listRevenue;
        public ObservableCollection<SupportRevenue> ListRevenue
        {
            get { return listRevenue; }
            set
            {
                listRevenue = value;
                OnPropertyChanged("ListRevenue");
            }
        }
        private ObservableCollection<SupportRevenue> listBackupRevenue;
        public ObservableCollection<SupportRevenue> ListBackupRevenue
        {
            get { return listBackupRevenue; }
            set
            {
                listBackupRevenue = value;
                OnPropertyChanged("ListBackupRevenue");
            }
        }
        private ObservableCollection<Tour> listTour;
        public ObservableCollection<Tour> ListTour
        {
            get { return listTour; }
            set
            {
                listTour = value;
                OnPropertyChanged("ListTour");
            }
        }
        private ObservableCollection<Invoice> listInvoice;
        public ObservableCollection<Invoice> ListInvoice
        {
            get { return listInvoice; }
            set
            {
                listInvoice = value;
                OnPropertyChanged("ListInvoice");
            }
        }

        private List<string> listMonth;
        public List<string> ListMonth
        {
            get { return listMonth; }
            set
            {
                listMonth = value;
                OnPropertyChanged("ListMonth");
            }
        }

        private string selectedMonth;
        public string SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = (string)monthHashtable[value];             
            }
        }
        private List<string> listYear;
        public List<string> ListYear
        {
            get { return listYear; }
            set
            {
                listYear = value;
                OnPropertyChanged("ListYear");
            }
        }

        private string selectedYear;
        public string SelectedYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;
                OnPropertyChanged("SelectedYear");
            }
        }

    }
    public class SupportRevenue :ObservableObject
    {
      

        public Tour host { set; get; }
        public string Month { set; get; }
        public string Year { set; get; }
        public string totalticket { set; get; }
        public string totalPrice { set; get; }
        public string percent { set; get; }
        public string color { set; get; }
        public string rank;

        public SupportRevenue(Tour host, string month, string year, string totalticket, string totalPrice, string percent, string color, string rank)
        {
            this.host = host;
            Month = month;
            Year = year;
            this.totalticket = totalticket;
            this.totalPrice = totalPrice;
            this.percent = percent;
            this.color = color;
            Rank = rank; 
        }

        public string Rank
        {
            get { return rank; }
            set
            {
                rank = value;
                OnPropertyChanged("Rank");
            }
        }
    }
}
