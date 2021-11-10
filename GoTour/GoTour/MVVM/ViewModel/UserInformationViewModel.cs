using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using Plugin.Media;
using Xamarin.Forms;
namespace GoTour.MVVM.ViewModel
{
    public class UserInformationViewModel : ObservableObject
    {
        INavigation navigation;
        Shell curentShell;

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
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");

        //COMMAND
        public Command AddImageCommand { get; }
        public Command UpdateUserInfo { get; }
        public Command RefreshCommand { get; set; }
        public Command EditTextCommand { get; }

        //USER PROPERTIES
        public string CurrRank { get; set; }
        public string CurrScore { get; set; }
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
        private string profilePic;
        public string ProfilePic
        {
            get { return profilePic; }
            set
            {
                profilePic = value;
                OnPropertyChanged("ProfilePic");
            }
        }

        //LIST USER
        public List<User> listUser = new List<User>();


        //REFRESHING
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            { 
                isRefreshing = value;
                OnPropertyChanged("IsRefreshing");
            }
        }

        //EDIT HANDLE ICON AND ISENABLE ENTRY
        private string iconSource;
        public string IconSource
        {
            get { return iconSource; }
            set
            {
                iconSource = value;
                OnPropertyChanged("IconSource");

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


        //CONSTRUCTOR
        public UserInformationViewModel() { }
        public UserInformationViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.curentShell = curentShell;
            CurrUser = DataManager.Ins.CurrentUser;
            Name = CurrUser.name;
            PutData();
            IsEdit = false;
            IconSource = "editIcon.png";
            EditTextCommand = new Command(editTextHandle);
            UpdateUserInfo = new Command(updateUser);
            RefreshCommand = new Command(() => PutData());
            AddImageCommand = new Command(changePhoto);
        }


        //EDIT TEXT HANDLE
        private void editTextHandle(object obj)
        {           
            if(IconSource.Equals("delete.png"))
            {
                if(!Name.Equals(DataManager.Ins.CurrentUser.name) || string.IsNullOrWhiteSpace(Name) ||
                    !CMND.Equals(CurrUser.cmnd) || string.IsNullOrWhiteSpace(CMND) ||
                    !Contact.Equals(CurrUser.contact) || string.IsNullOrWhiteSpace(Contact) ||
                    !Birthday.Equals(CurrUser.birthday) || !Address.Equals(CurrUser.address) ||
                    string.IsNullOrWhiteSpace(Address))
                {
                    PutData();
                }
                else
                {
                    IconSource = "editIcon.png";
                    IsEdit = !IsEdit;
                }
            }
            else
            {
                IconSource = "delete.png";
                IsEdit = !IsEdit;
            }
        }

        //CHANGE AVT HANDLE
        async void changePhoto(object obj)
        {
            await CrossMedia.Current.Initialize();

            var imgData = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions());
            if(imgData == null)
            {
                return;
            }
            else
            {
                string url = await DataManager.Ins.UsersServices.saveImage(imgData.GetStream(), CurrUser.email, CurrUser.name);

                ProfilePic = url;
                updateUser();
            }
        }

        //PUT DATA INTO CONTROL HANDLE
        async Task PutData()
        {
            IsRefreshing = true;
            listUser =  await DataManager.Ins.UsersServices.GetAllUsers();
            for (int i = 0; i < listUser.Count(); i++)
            {
                if (listUser[i].email == CurrUser.email)
                {
                    CurrRank = "Rank: " + listUser[i].rank.ToString();
                    CurrScore = "Score: " + listUser[i].score.ToString();
                    Name = listUser[i].name;
                    Email = listUser[i].email;
                    Contact = listUser[i].contact;
                    Address = listUser[i].address;
                    Birthday = listUser[i].birthday;
                    CMND = listUser[i].cmnd;
                    ProfilePic = listUser[i].profilePic;
                }
            }
            IsEdit = false;
            IconSource = "editIcon.png";
            IsRefreshing = false;
        }

        //UPDATE DATABASE HANDLE
         async void updateUser()
        {
            if ( string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Contact) || string.IsNullOrWhiteSpace(Address) || string.IsNullOrWhiteSpace(Birthday) || string.IsNullOrWhiteSpace(CMND))
            {
                DependencyService.Get<IToast>().ShortToast("Please fill out your information");              
            } else
            {
                if(ContactValidation(Contact) == false)
                {
                    DependencyService.Get<IToast>().ShortToast("Your contact should have ten numerics and start with number 0");
                }
                else
                {
                    User user = new User { name = Name, address = Address, birthday = Birthday, cmnd = CMND, contact = Contact, email = Email, rank = CurrUser.rank, score = CurrUser.score, profilePic = ProfilePic, password = CurrUser.password };
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
                    DependencyService.Get<IToast>().ShortToast("Saved your profile successfully");
                    IsEdit = false;
                    IconSource = "editIcon.png";
                }              
            }  
        }

        //CONTACT VALIDDATION
        private bool ContactValidation(string contact)
        {
            if(contact.Length == 10 && contact[0].ToString() == "0")
            {
                return true;
            }
            return false;
        }
    }
}
