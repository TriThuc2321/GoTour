using Firebase.Database;
using Firebase.Database.Query;
using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class MenuViewModel : ObservableObject
    {
        INavigation navigation;

        public Command AddImageCommand { get; }
        public Command PushDatacommand { get; }
        public MenuViewModel() { }
        public MenuViewModel(INavigation navigation)
        {
            this.navigation = navigation;

            AddImageCommand = new Command(_addImg);
            PushDatacommand = new Command(pushData);
        }
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");
        List<string> temp1 = new List<string>();
        
        public async void pushData(object sender)
        {
            temp1.Add("11");
            temp1.Add("11");
            temp1.Add("11");
            temp1.Add("11");
            await firebase
             .Child("Tours")
             .PostAsync(new Class1()
             {
            id = "",
            name = "",
            imgSource = temp1,
            startTime = "",
            duration = "",
            tourGuide = temp1,
            passengerNumber = "10",
            description = "",
            isOccured = false,
        });

            /*  navigation.PushAsync(new DetailTourView());*/

        }

        
        async void _addImg(object sender)
        {

            await CrossMedia.Current.Initialize();

            var imgData = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions());

            string url = await DataManager.Ins.UsersServices.saveImage(imgData.GetStream());

            ImgSource = ImageSource.FromStream(imgData.GetStream);

            /*User temp = new User("01", "Thien", "078382112", "3/10", "234222222", url);

            await DataManager.Ins.UsersServices.addUser(temp);*/
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
