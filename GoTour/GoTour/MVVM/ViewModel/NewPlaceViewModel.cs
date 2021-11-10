using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class NewPlaceViewModel : ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        List<Stream> listStream;

        public Command SaveCommand { get; }
        public Command AddCommand { get; }
        public Command DeleteCommand { get; }
        public NewPlaceViewModel() { }
        public NewPlaceViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.currentShell = curentShell;
            this.navigation = navigation;
            this.currentShell = curentShell;

            Imgs = new ObservableCollection<ImageSource>();

            SaveCommand = new Command(saveHandleAsync);
            AddCommand = new Command(addHandleAsync);
            DeleteCommand = new Command(deleteHandle);

            listStream = new List<Stream>();           

        }



        private async void saveHandleAsync(object obj)
        {
            if(Name == null || Name == "" )
            {
                DependencyService.Get<IToast>().ShortToast("Please enter place's name");
                return;
            }
            else if (Name == null || Description == null || Name == "" || Description == "" || listStream.Count() == 0)
            {
                DependencyService.Get<IToast>().ShortToast("Please enter place's description");
                return;
            }
            else if (listStream.Count() == 0)
            {
                DependencyService.Get<IToast>().ShortToast("Please insert place's image");
                return;
            }

            string id = DataManager.Ins.GeneratePlaceId();           

            List<string> imgSource = new List<string>();

            for (int i = 0; i < listStream.Count(); i++)
            {
                string url = await DataManager.Ins.PlacesServices.saveImage(listStream[i], id, i);
                imgSource.Add(url);
            }
            await DataManager.Ins.PlacesServices.AddPlace(id, Name, imgSource, Description);
            DataManager.Ins.ListPlace.Add(new Place(id, Name, imgSource, Description));
            navigation.PopAsync();
        }

        private void deleteHandle(object obj)
        {
            if (Imgs.Count() > 0)
            {
                Imgs.RemoveAt(0);
                listStream.RemoveAt(0);
            }

        }

        private async void addHandleAsync(object obj)
        {
            await CrossMedia.Current.Initialize();
            var imgData = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions());

            if (imgData != null)
            {
                Imgs.Add(ImageSource.FromStream(imgData.GetStream));
                Stream s = imgData.GetStream();
                listStream.Add(s);
            }
        }

        
       
        private MemoryStream GetStreamFromUrl(string url)
        {
            byte[] imageData = null;
            MemoryStream ms;

            ms = null;

            try
            {
                using (var wc = new System.Net.WebClient())
                {
                    imageData = wc.DownloadData(url);
                }
                ms = new MemoryStream(imageData);
            }
            catch (Exception ex)
            {

            }

            return ms;
        }

        private Place selectedPlace;
        public Place SelectedPlace
        {
            get { return selectedPlace; }
            set
            {
                selectedPlace = value;
                OnPropertyChanged("SelectedPlace");
            }
        }
        
        private ObservableCollection<ImageSource> imgs;
        public ObservableCollection<ImageSource> Imgs
        {
            get { return imgs; }
            set
            {
                imgs = value;
                OnPropertyChanged("Imgs");
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
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }
    }
}
