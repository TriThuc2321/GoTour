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
        public ICommand NewCommand => new Command<object>((obj) =>
        {
            DataManager.Ins.currentUserManager = null;
            DataManager.Ins.IsNewUser = true;
            navigation.PushAsync(new EditStaffManagerView());
        });
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

        public ICommand NavigationBack => new Command<object>((obj) =>
        {
            navigation.PopAsync();
        });
        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            User result = obj as User;
            if (result != null)
            {
                DataManager.Ins.currentUserManager = result;
                DataManager.Ins.IsNewUser = false;
                navigation.PushAsync(new EditStaffManagerView());
                SelectedUser = null;
            }
        });
        public ICommand DeleteCommand => new Command<object>(async (obj) =>
        {
            User result = obj as User;
            if (result != null)
            {
                if(result.email == DataManager.Ins.CurrentUser.email)
                {
                    DependencyService.Get<IToast>().ShortToast("Your account cannot be deleted");
                    return;
                }
                
                AllList.Remove(result);

                if (result.rank == 0) DataManager.Ins.admins.Remove(result);
                else if (result.rank == 1) DataManager.Ins.managements.Remove(result);
                else if (result.rank == 2) DataManager.Ins.tourGuides.Remove(result);
                else if (result.rank == 3) DataManager.Ins.customers.Remove(result);

                result.isEnable = false;
                await DataManager.Ins.UsersServices.UpdateUser(result);
            }
        });
        private User selectedUser;
        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
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
                if(value == "All" )
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
