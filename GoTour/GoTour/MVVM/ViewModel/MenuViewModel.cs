using Firebase.Database;
using Firebase.Database.Query;
using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class MenuViewModel : ObservableObject
    {
        INavigation navigation;
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");
        UsersServices usersServices;

        public Command AddImageCommand { get; }
        public MenuViewModel(){}
        public MenuViewModel(INavigation navigation)
        {
            this.navigation = navigation;

            AddImageCommand = new Command(_addImg);
            usersServices = new UsersServices();
        }

        async void _addImg(object sender)
        {

            await CrossMedia.Current.Initialize();

            var imgData = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions());

            string url = await usersServices.saveImage(imgData.GetStream());

            ImgSource = ImageSource.FromStream(imgData.GetStream);

            User temp = new User("01", "Thien", "078382112", "3/10", "234222222", url);
            usersServices.addUser(temp);
        }

        private ImageSource imgSource;
        public ImageSource ImgSource
        {
            get { return imgSource; }
            set {
                imgSource = value;
                OnPropertyChanged("ImgSource");
            }
        }
    }
}
