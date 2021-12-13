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

        public Command PlaceCommand { get; }
        public Command StayPlaceCommand { get; }
        public Command TourCommand { get; }
        public Command ReviewCommand { get; }
        public MenuViewModel() { }
        public MenuViewModel(INavigation navigation)
        {
            this.navigation = navigation;

            PlaceCommand = new Command(placeHandle);
            StayPlaceCommand = new Command(stayPlaceHandle);
            TourCommand = new Command(tourHandle);
            ReviewCommand = new Command(reviewHandle);
        }

        private async void reviewHandle(object obj)
        {
            //Review review = new Review("11", "trithuc23232@gmail.com", "message", DateTime.Now, 5);

            //await firebase
            //  .Child("Reviews")
            //  .PostAsync(new Review()
            //  {
            //      tourId = review.tourId,
            //      email = review.email,
            //      message = review.message,
            //      time = review.time,
            //      starNumber = review.starNumber,
            //  });
        }

        private async void stayPlaceHandle(object obj)
        {
            /*temp1.Clear();

            temp1.Add("11");
            temp1.Add("11");
            temp1.Add("11");
            temp1.Add("11");

            await firebase
              .Child("StayPlaces")
              .PostAsync(new StayPlace()
              {
                  id = "Thuc",
                  name = "",
                  address = "",
                  placeId = "",
                  imgSource = temp1,
                  description = "",
              });*/
        }

        private async void placeHandle(object obj)
        {
            /*temp1.Clear();

            temp1.Add("11");
            temp1.Add("11");
            temp1.Add("11");
            temp1.Add("11");

            await firebase
               .Child("Places")
               .PostAsync(new Place()
               {
                   id = "Thuc",
                   name = "",
                   imgSource = temp1,
                   description = "",
               });*/
        }

        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");
        List<string> temp1 = new List<string>();
        List<string> temp2 = new List<string>();
        List<PlaceId_StayPlace> temp3 = new List<PlaceId_StayPlace>();

        public async void tourHandle(object sender)
        {
            temp1.Clear();
            temp2.Clear();
            temp3.Clear();

            temp1.Add("11");
            temp1.Add("11");
            temp1.Add("11");
            temp1.Add("11");

            temp2.Add("11");
            temp2.Add("11");

            temp3.Add(new PlaceId_StayPlace("DL", "SP001"));
            temp3.Add(new PlaceId_StayPlace("DL", "SP001"));

            /*await firebase
              .Child("Tours")
              .PostAsync(new Tour()
              {
                  id = "Thuc",
                  name = "",
                  imgSource = temp1,
                  startTime = "08/11/2021",
                  duration = "3/4",
                  tourGuide = temp2,
                  passengerNumber = "10",
                  description = "",
                  isOccured = false,
                  basePrice = "10000",
                  SPforPList = temp3,
                  remaining = "10"
              });*/
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

            /*List<PlaceId_Duration> temp = new List<PlaceId_Duration>();
            temp.Add(new PlaceId_Duration(2,2,"DL"));
            temp.Add(new PlaceId_Duration(2, 2, "DL"));
            temp.Add(new PlaceId_Duration(2, 2, "DL"));

            TourPlace tourPlace = new TourPlace()
            {
                tourId = "",
                placeDurationList = temp
            };
            DataManager.Ins.TourPlaceServices.addTourPlace(tourPlace);*/

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
