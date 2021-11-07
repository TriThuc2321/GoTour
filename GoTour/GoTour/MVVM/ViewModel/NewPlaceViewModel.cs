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
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class NewPlaceViewModel : ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;
        int count;
        List<Stream> listStream;

        public Command EditTextCommand { get; }
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

            EditTextCommand = new Command(editTextHandle);
            AddCommand = new Command(addHandleAsync);
            DeleteCommand = new Command(deleteHandle);

            SelectedPlace = DataManager.Ins.CurrentPlaceManager;
            listStream = new List<Stream>();

            if (SelectedPlace.imgSource != null)
            {
                foreach (var i in SelectedPlace.imgSource)
                {
                    Imgs.Add(ImageSource.FromUri(new Uri(i)));
                    listStream.Add(GetStreamFromUrl(i));
                }
            }


            Name = DataManager.Ins.CurrentPlaceManager.name;
            Description = DataManager.Ins.CurrentPlaceManager.description;
            IsEdit = false;
            SourceIcon = "editIcon.png";
            IsText = true;

            count = Imgs.Count();
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

            //string url = await DataManager.Ins.UsersServices.saveImage(imgData.GetStream());

            //ImgSource = ImageSource.FromStream(imgData.GetStream);
            Imgs.Add(ImageSource.FromStream(imgData.GetStream));
            Stream s = imgData.GetStream();
            listStream.Add(s);
        }

        private async void editTextHandle(object obj)
        {
            IsEdit = !IsEdit;
            if (!IsEdit)
            {
                SourceIcon = "editIcon.png";
                updateData();
            }
            else
            {
                SourceIcon = "tickIcon.png";
            }
            IsText = !IsText;
        }
        private async void updateData()
        {
            DataManager.Ins.CurrentPlaceManager.description = Description;
            DataManager.Ins.CurrentPlaceManager.name = Name;
            DataManager.Ins.CurrentPlaceManager.imgSource = new List<string>();

            for (int i = 0; i < count; i++)
            {
                await DataManager.Ins.PlacesServices.DeleteFile(DataManager.Ins.CurrentPlaceManager.id, i);
            }

            for (int i = 0; i < listStream.Count(); i++)
            {
                string url = await DataManager.Ins.PlacesServices.saveImage(listStream[i], DataManager.Ins.CurrentPlaceManager.id, i);
                DataManager.Ins.CurrentPlaceManager.imgSource.Add(url);
            }
            await DataManager.Ins.PlacesServices.UpdatePlace(DataManager.Ins.CurrentPlaceManager);

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
        private bool isText;
        public bool IsText
        {
            get { return isText; }
            set
            {
                isText = value;
                OnPropertyChanged("IsText");
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

        private string sourceIcon;
        public string SourceIcon
        {
            get { return sourceIcon; }
            set
            {
                sourceIcon = value;
                OnPropertyChanged("SourceIcon");
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
