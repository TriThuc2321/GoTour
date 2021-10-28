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

            /*await CrossMedia.Current.Initialize();

            var imgData = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions());

            string url = await DataManager.Ins.UsersServices.saveImage(imgData.GetStream());

            ImgSource = ImageSource.FromStream(imgData.GetStream);*/

            /*Price price = new Price() {
                id = "01",
                name ="",
                price = 0,
                hotleId = "KS001",
            };
            DataManager.Ins.PriceServices.addPrice(price);*/

            List<PlaceId_Duration> temp = new List<PlaceId_Duration>();
            temp.Add(new PlaceId_Duration(2,2,"DL"));
            temp.Add(new PlaceId_Duration(2, 2, "DL"));
            temp.Add(new PlaceId_Duration(2, 2, "DL"));

            TourPlace tourPlace = new TourPlace()
            {
                tourId = "",
                placeDurationList = temp
            };
            DataManager.Ins.TourPlaceServices.addTourPlace(tourPlace);
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
