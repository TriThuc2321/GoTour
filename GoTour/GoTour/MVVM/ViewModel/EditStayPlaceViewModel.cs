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
    class EditStayPlaceViewModel:ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        int count;
        List<Stream> listStream;

        public Command EditTextCommand { get; }
        public Command AddCommand { get; }
        public Command DeleteCommand { get; }
        public Command NavigationBack { get; }


        public EditStayPlaceViewModel() { }
        public EditStayPlaceViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.currentShell = curentShell;

            Imgs = new ObservableCollection<ImageSource>();

            EditTextCommand = new Command(editTextHandle);
            AddCommand = new Command(addHandleAsync);
            DeleteCommand = new Command(deleteHandle);
            NavigationBack = new Command(() => navigation.PopAsync());

            SelectedStayPlace = DataManager.Ins.CurrentStayPlaceManager;
            listStream = new List<Stream>();

            if (SelectedStayPlace.imgSource != null)
            {
                foreach (var i in SelectedStayPlace.imgSource)
                {
                    Imgs.Add(ImageSource.FromUri(new Uri(i)));
                    listStream.Add(GetStreamFromUrl(i));

                }
            }


            Name = DataManager.Ins.CurrentStayPlaceManager.name;
            Description = DataManager.Ins.CurrentStayPlaceManager.description;
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

            if (imgData != null)
            {
                Imgs.Add(ImageSource.FromStream(imgData.GetStream));
                Stream s = imgData.GetStream();
                listStream.Add(s);
            }

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
            DataManager.Ins.CurrentStayPlaceManager.description = Description;
            DataManager.Ins.CurrentStayPlaceManager.name = Name;
            DataManager.Ins.CurrentStayPlaceManager.imgSource = new List<string>();

            for (int i = 0; i < count; i++)
            {
                await DataManager.Ins.StayPlacesServices.DeleteFile(DataManager.Ins.CurrentStayPlaceManager.id, i);
            }

            for (int i = 0; i < listStream.Count(); i++)
            {
                string url = await DataManager.Ins.StayPlacesServices.saveImage(listStream[i], DataManager.Ins.CurrentStayPlaceManager.id, i);
                DataManager.Ins.CurrentStayPlaceManager.imgSource.Add(url);
            }
            await DataManager.Ins.StayPlacesServices.UpdateStayPlace(DataManager.Ins.CurrentStayPlaceManager);

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

        private StayPlace selectedStayPlace;
        public StayPlace SelectedStayPlace
        {
            get { return selectedStayPlace; }
            set
            {
                selectedStayPlace = value;
                OnPropertyChanged("SelectedStayPlace");

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
