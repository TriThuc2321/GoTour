using System;
using System.Collections.Generic;
using System.IO;
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
        public Stream imgStream;

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
        public Command MenuCommand { get; }

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
                if (value == false)
                {
                    VisibleSaveBtn = false;
                    EnableImageBtn = false;
                }
                OnPropertyChanged("IsEdit");

            }
        }

        //VISIBLE SAVE BUTTON
        private bool visibleSaveBtn;
        public bool VisibleSaveBtn
        {
            get { return visibleSaveBtn; }
            set
            {
                visibleSaveBtn = value;
                OnPropertyChanged("VisibleSaveBtn");
            }
        }


        //ENABLE IMAGEBUTTON
        private bool enableImageBtn;
        public bool EnableImageBtn
        {
            get { return enableImageBtn; }
            set
            {
                enableImageBtn = value;
                OnPropertyChanged("EnableImageBtn");
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
            VisibleSaveBtn = false;
            EnableImageBtn = false;
            IconSource = "editIcon.png";
            MenuCommand = new Command(() => curentShell.FlyoutIsPresented = !curentShell.FlyoutIsPresented);
            EditTextCommand = new Command(editTextHandle);
            UpdateUserInfo = new Command(updateUser);
            RefreshCommand = new Command(() => PutData());
            AddImageCommand = new Command(changePhoto);
        }


        //EDIT TEXT HANDLE
        private void editTextHandle(object obj)
        {
            if (IconSource.Equals("delete.png"))
            {
                if (!Name.Equals(DataManager.Ins.CurrentUser.name) || string.IsNullOrWhiteSpace(Name) ||
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
                VisibleSaveBtn = !VisibleSaveBtn;
                EnableImageBtn = !EnableImageBtn;
            }
        }

        //CHANGE AVT HANDLE
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
                //string url = await DataManager.Ins.UsersServices.saveImage(imgData.GetStream(), CurrUser.email, CurrUser.name);

                ProfilePic = ImageSource.FromStream(imgData.GetStream);
                //updateUser();
            }
        }

        //PUT DATA INTO CONTROL HANDLE
        async Task PutData()
        {
            IsRefreshing = true;
            listUser = await DataManager.Ins.UsersServices.GetAllUsers();
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
            VisibleSaveBtn = false;
            EnableImageBtn = false;
            IconSource = "editIcon.png";
            IsRefreshing = false;
        }

        //UPDATE DATABASE HANDLE
        async void updateUser()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Contact) || string.IsNullOrWhiteSpace(Address) || string.IsNullOrWhiteSpace(Birthday) || string.IsNullOrWhiteSpace(CMND))
            {
                DependencyService.Get<IToast>().ShortToast("Hãy điền đầy đủ thông tin");
            }
            else
            {
                if (ContactValidation(Contact) == false)
                {
                    DependencyService.Get<IToast>().ShortToast("Số điện thoại cần có 10 chữ số và bắt đầu bằng 0");
                }
                else
                {
                    if (IDCardValidation(CMND) == false)
                    {
                        DependencyService.Get<IToast>().ShortToast("CMND hay CCCD cần có 9 chữ số hoặc 12 chữ số");
                    }
                    else
                    {
                        if (imgStream == null)
                        {
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
                            DependencyService.Get<IToast>().ShortToast("Lưu hồ sơ thành công");
                            IsEdit = false;
                            VisibleSaveBtn = false;
                            EnableImageBtn = false;
                            IconSource = "editIcon.png";
                        }
                        else
                        {
                            CurrUser.profilePic = await DataManager.Ins.UsersServices.saveImage(imgStream, CurrUser.email, CurrUser.name);
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
                            DependencyService.Get<IToast>().ShortToast("Lưu hồ sơ thành công");
                            IsEdit = false;
                            VisibleSaveBtn = false;
                            EnableImageBtn = false;
                            IconSource = "editIcon.png";
                        }
                    }
                }
            }
        }

        //CONTACT VALIDDATION
        private bool ContactValidation(string contact)
        {
            if (contact.Length == 10 && contact[0].ToString() == "0" && contact.All(char.IsDigit))
            {
                return true;
            }
            return false;
        }

        //CMND VALIDATION
        private bool IDCardValidation(string vcmnd)
        {
            bool ex;
            if (!vcmnd.All(char.IsDigit))
            {
                ex = false;
            }
            else
            {
                if (vcmnd.Length < 9)
                {
                    ex = false;
                }
                else if (vcmnd.Length > 9)
                {
                    if (vcmnd.Length < 12)
                    {
                        ex = false;
                    }
                    else if (vcmnd.Length > 12)
                    {
                        ex = false;
                    }
                    else
                    {
                        ex = true;
                    }
                }
                else
                {
                    ex = true;
                }
            }
            return ex;
        }

    }
}
