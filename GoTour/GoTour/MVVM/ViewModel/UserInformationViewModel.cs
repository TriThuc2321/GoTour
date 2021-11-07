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
        public Command AddImageCommand { get; }
        public Command UpdateUserInfo { get; }
        public Command RefreshCommand { get; set; }
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
        public List<User> listUser = new List<User>();

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

        public UserInformationViewModel() { }
        public UserInformationViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.curentShell = curentShell;
            CurrUser = DataManager.Ins.CurrentUser;
            PutData();
            UpdateUserInfo = new Command(updateUser);
            RefreshCommand = new Command(() => PutData());

            AddImageCommand = new Command(changePhoto);
        }

       async void changePhoto(object obj)
        {
            
            await CrossMedia.Current.Initialize();

            var imgData = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions());

            string url = await DataManager.Ins.UsersServices.saveImage(imgData.GetStream(), CurrUser.email, CurrUser.name);

            ProfilePic = url;
            updateUser();
        }
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
            IsRefreshing = false;
        }
         async void updateUser()
        {
            if ( Name == "" || Contact == "" || Address == "" || Birthday == "" || CMND == "")
            {
                DependencyService.Get<IToast>().ShortToast("Please fill out your information");
            } else
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
            }
           
        }

    }
}
