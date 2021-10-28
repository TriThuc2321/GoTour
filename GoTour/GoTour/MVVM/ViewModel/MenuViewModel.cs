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

        public Command AddImageCommand { get; }
        public MenuViewModel(){}
        public MenuViewModel(INavigation navigation)
        {
            this.navigation = navigation;

            AddImageCommand = new Command(_addImg);
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
