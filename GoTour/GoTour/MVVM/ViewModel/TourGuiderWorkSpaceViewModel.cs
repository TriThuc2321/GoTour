using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class TourGuiderWorkSpaceViewModel : ObservableObject
    {
        INavigation navigation;
        Shell currentShell;


        //KHAI BAO COMMAND
        public Command MenuCommand { get; }
        public Command OpenSendNotiView { get; }


        //KHAI BAO LIST REUSLT
        private ObservableCollection<Tour> listTourYouWorkOn;
        public ObservableCollection<Tour> ListTourYouWorkOn
        {
            get { return listTourYouWorkOn; }
            set
            {
                listTourYouWorkOn = value;
                OnPropertyChanged("ListTourYouWorkOn");
            }
        }


        //CONSTRUCTOR
        public TourGuiderWorkSpaceViewModel() { }


        //KHAI BAO SELECTED TOUR
        private Tour selectedTour;
        public Tour SelectedTour
        {
            get { return selectedTour; }
            set
            {
                selectedTour = value;
                OnPropertyChanged("SelectedTour");

            }
        }



        //SELECTED ITEM COMMAND
        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            Tour result = obj as Tour;
            if (result != null)
            {
                DataManager.Ins.currentTour = result;
                navigation.PushAsync(new DetailTourView());
                SelectedTour = null;
            }
        });


        //TOUR GUIDER WORK SPACE VIEWMODEL
        public TourGuiderWorkSpaceViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            OpenSendNotiView = new Command(() => navigation.PushAsync(new SendNotificationView()));
            MenuCommand = new Command(() => currentShell.FlyoutIsPresented = !currentShell.FlyoutIsPresented);



            this.navigation = navigation;

            GetListTourYouWorkOn();
        }

        //LAY DANH SACH CAC TIUR MA BAN LAM VIEC
        void GetListTourYouWorkOn()
        {
            ListTourYouWorkOn = new ObservableCollection<Tour>();
            if (DataManager.Ins.CurrentUser.rank == 1)
            {
                foreach(Tour i in DataManager.Ins.ListTour)
                {
                    ListTourYouWorkOn.Add(i);
                }
            }
            else
            {   
                List<Tour> temp1 = new List<Tour>(); //List Tour

                foreach (Tour ite in DataManager.Ins.ListTour)
                {
                    temp1.Add(ite);
                }

                List<Tour> temp = new List<Tour>();
                List<Tour> result = new List<Tour>();


                //Lay email guider
                string yourEmail = DataManager.Ins.CurrentUser.email;


                //Loc email
                temp = temp1.FindAll(e => e.tourGuide.Exists(p => p == yourEmail));
                foreach (var plc in temp)
                    if (!result.Contains(plc))
                        result.Add(plc);

                foreach (Tour ite3 in result)
                {
                    ListTourYouWorkOn.Add(ite3);
                }
            }           
        }
    }
}
