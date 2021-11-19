using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class StaffManagerViewModel: ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        

        public StaffManagerViewModel() { }
        public StaffManagerViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            Init();
        }
        void Init()
        {
            types = new ObservableCollection<string>();
            types.Add("All");
            types.Add("Admin");
            types.Add("Management");
            types.Add("Tour Guide");
            types.Add("Customer");

            SelectedType = "All";

            
        }

        private ObservableCollection<User> allList;
        public ObservableCollection<User> AllList
        {
            get { return allList; }
            set
            {
                allList = value;
                OnPropertyChanged("AllList");
            }
        }
        private ObservableCollection<string> types;
        public ObservableCollection<string> Types
        {
            get { return types; }
            set
            {
                types = value;
                OnPropertyChanged("Types");
            }
        }

        private string selectedType;
        public string SelectedType
        {
            get { return selectedType; }
            set
            {
                selectedType = value;
                AllList = new ObservableCollection<User>();
                if(value == "All")
                {
                    foreach (var ite in DataManager.Ins.users)
                    {
                        AllList.Add(ite);
                    }
                }
                else if(value == "Admin")
                {
                    foreach (var ite in DataManager.Ins.admins)
                    {
                        AllList.Add(ite);
                    }
                }
                else if (value == "Management")
                {
                    foreach (var ite in DataManager.Ins.managements)
                    {
                        AllList.Add(ite);
                    }
                }
                else if (value == "Tour Guide")
                {
                    foreach (var ite in DataManager.Ins.tourGuides)
                    {
                        AllList.Add(ite);
                    }
                }
                else if (value == "Customer")
                {
                    foreach (var ite in DataManager.Ins.customers)
                    {
                        AllList.Add(ite);
                    }
                }


                OnPropertyChanged("SelectedType");
            }
        }

    }
}
