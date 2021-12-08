using Firebase.Database;
using Firebase.Database.Query;
using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class EditStaffManagerViewModel : ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");

        //COMMAND
        public Command AddImageCommand { get; }
        public Command UpdateUserInfo { get; }
        public Stream imgStream;

        public EditStaffManagerViewModel() { }
        public EditStaffManagerViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.currentShell = curentShell;

            
            if(DataManager.Ins.currentUserManager != null && DataManager.Ins.currentUserManager.rank == 3)
            {
                IsEdit = false;
            }
            else
            {
                IsEdit = true;
            }
            
            CurrUser = DataManager.Ins.currentUserManager;
            if (CurrUser == null) CurrUser = new User();
            Name = CurrUser.name;
            SelectedRank = CurrUser.rank;
            Email = CurrUser.email;
            Contact = CurrUser.contact;
            Address = CurrUser.address;
            Birthday = CurrUser.birthday;
            CMND = CurrUser.cmnd;
            if (CurrUser.profilePic != null)
            {
                ProfilePic = ImageSource.FromUri(new Uri(CurrUser.profilePic));
            }
            else ProfilePic = null;

            UpdateUserInfo = new Command(updateUser);
            AddImageCommand = new Command(changePhoto);

            types = new ObservableCollection<string>();
            types.Add("Admin");
            types.Add("Management");
            types.Add("Tour Guide");
            types.Add("Customer");

            if(CurrUser.rank == null || CurrUser.rank == 0)
            {
                SelectedType = "Admin";
            }
            else if(CurrUser.rank == 1)
            {
                SelectedType = "Management";
            }
            else if (CurrUser.rank == 2)
            {
                SelectedType = "Tour Guide";
            }
            else if (CurrUser.rank == 3)
            {
                SelectedType = "Customer";
            }

        }
        public ICommand NavigationBack => new Command<object>((obj) =>
        {
            navigation.PopAsync();
        });
        

        async void changePhoto(object obj)
        {
            await CrossMedia.Current.Initialize();

            var imgData = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions());
            if (imgData == null)
            {
                return;
            }
            else
            {                
                imgStream = imgData.GetStream();
                ProfilePic = ImageSource.FromStream(imgData.GetStream);
            }
        }

        async void updateUser()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Contact) || string.IsNullOrWhiteSpace(Address) || string.IsNullOrWhiteSpace(Birthday) || string.IsNullOrWhiteSpace(CMND) || ProfilePic== null)
            {
                DependencyService.Get<IToast>().ShortToast("Please fill out your information");
            }
            else
            {
                if (ContactValidation(Contact) == false)
                {
                    DependencyService.Get<IToast>().ShortToast("Your contact should have ten numerics and start with number 0");
                }
                else if (!DataManager.Ins.UsersServices.IsCMND(CMND))
                {
                    DependencyService.Get<IToast>().ShortToast("Your Id Card should have 9 or 12 numerics");
                }
                else
                {
                    if (!DataManager.Ins.IsNewUser)
                    {
                        if(imgStream != null)
                        {
                            CurrUser.profilePic = await DataManager.Ins.UsersServices.saveImage(imgStream, CurrUser.email, CurrUser.name);
                        }                        

                        User user = new User { name = Name, address = Address, birthday = Birthday, cmnd = CMND, contact = Contact, email = Email, rank = CurrUser.rank, score = CurrUser.score, profilePic = CurrUser.profilePic, password = CurrUser.password };
                        var toUpdateUser = (await firebase
                     .Child("Users")
                     .OnceAsync<User>()).Where(a => a.Object.email == user.email).FirstOrDefault();

                        await firebase
                          .Child("Users")
                          .Child(toUpdateUser.Key)
                          .PutAsync(new User
                          {
                              email = user.email,
                              password = user.password,
                              name = user.name,
                              contact = user.contact,
                              birthday = user.birthday,
                              cmnd = user.cmnd,
                              profilePic = user.profilePic,
                              address = user.address,
                              score = user.score,
                              rank = user.rank
                          });

                        for(int i =0; i< DataManager.Ins.users.Count; i++)
                        {
                            if(DataManager.Ins.users[i].email == Email)
                            {
                                DataManager.Ins.users[i] = user;
                                break;
                            }
                        }
                        DataManager.Ins.ClassifyUser();
                        DependencyService.Get<IToast>().ShortToast("Saved your profile successfully");

                        navigation.RemovePage(navigation.NavigationStack[navigation.NavigationStack.Count - 2]);
                        await navigation.PushAsync(new StaffManagerView());
                        navigation.RemovePage(navigation.NavigationStack[navigation.NavigationStack.Count - 2]);
                    }
                    else
                    {
                        if(DataManager.Ins.UsersServices.ExistEmail(Email, DataManager.Ins.users))
                        {
                            DependencyService.Get<IToast>().ShortToast("Email already in use");
                            return;
                        }
                        CurrUser.password = DataManager.Ins.UsersServices.Encode("123456");
                        if (imgStream != null)
                        {
                            CurrUser.profilePic = await DataManager.Ins.UsersServices.saveImage(imgStream, Email, Name);
                        }
                        User user = new User { name = Name, address = Address, birthday = Birthday, cmnd = CMND, contact = Contact, email = Email, rank = CurrUser.rank, score = CurrUser.score, profilePic = CurrUser.profilePic, password = CurrUser.password };
                        
                        await DataManager.Ins.UsersServices.addUser(user);
                        DataManager.Ins.users.Add(user);
                        DataManager.Ins.ClassifyUser();
                        DependencyService.Get<IToast>().ShortToast("Insert new user successfully");
                    }

                }
            }
        }

        //CONTACT VALIDDATION
        private bool ContactValidation(string contact)
        {
            if (contact.Length == 10 && contact[0].ToString() == "0")
            {
                return true;
            }
            return false;
        }
        private User currUser;
        public User CurrUser
        {
            get { return currUser; }
            set
            {
                currUser = value;
                OnPropertyChanged("CurrUser");
            }
        }

        private int selectedRank;
        public int SelectedRank
        {
            get { return selectedRank; }
            set
            {
                selectedRank = value;
                OnPropertyChanged("SelectedRank");
            }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }
        private string contact;
        public string Contact
        {
            get { return contact; }
            set
            {
                contact = value;
                OnPropertyChanged("Contact");
            }
        }
        private string address;
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }
        private string cmnd;
        public string CMND
        {
            get { return cmnd; }
            set
            {
                cmnd = value;
                OnPropertyChanged("CMND");
            }
        }
        private string birthday;
        public string Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                OnPropertyChanged("Birthday");
            }
        }
        private ImageSource profilePic;
        public ImageSource ProfilePic
        {
            get { return profilePic; }
            set
            {
                profilePic = value;
                OnPropertyChanged("ProfilePic");
            }
        }


        private bool isEdit;
        public bool IsEdit
        {
            get { return isEdit; }
            set
            {
                isEdit = value;
                OnPropertyChanged("IsEdit");

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
                
                if(value == "Admin")
                {
                    CurrUser.rank = 0;
                }
                else if (value == "Management")
                {
                    CurrUser.rank = 1;
                }
                else if (value == "Tour Guide")
                {
                    CurrUser.rank = 2;
                }
                else if (value == "Customer")
                {
                    CurrUser.rank = 3;
                }
                OnPropertyChanged("SelectedType");
            }
        }
    }
}
