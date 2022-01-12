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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class EditStayPlaceViewModel:ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;
        public bool IsLoaded = false;
        int count;
        List<Stream> listStream;
        public Command EditTextCommand { get; }
        public Command AddCommand { get; }
        public Command NavigationBack { get; }

        public EditStayPlaceViewModel() { }
        public EditStayPlaceViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.currentShell = curentShell;
            IsLoading = false;
            Imgs = new ObservableCollection<ImageSource>();

            EditTextCommand = new Command(editTextHandle);
            AddCommand = new Command(addHandleAsync);
            NavigationBack = new Command(() => navigation.PopAsync());

            SelectedStayPlace = DataManager.Ins.CurrentStayPlaceManager;
            listStream = new List<Stream>();

            if (SelectedStayPlace.imgSource != null)
            {
                foreach (var i in SelectedStayPlace.imgSource)
                {
                    Imgs.Add(ImageSource.FromUri(new Uri(i)));
                }
            }


            Name = DataManager.Ins.CurrentStayPlaceManager.name;
            Description = DataManager.Ins.CurrentStayPlaceManager.description;
            IsEdit = false;
            SourceText = "Edit";
            IsText = true;

            count = Imgs.Count();
        }

        public ICommand DeleteImageCommand => new Command<object>((obj) =>
        {
            ImageSource result = obj as ImageSource;
            if (result != null)
            {
                int i = 0;
                for (i = 0; i < Imgs.Count; i++)
                {
                    if (Imgs[i] == result) break;
                }
                Imgs.RemoveAt(i);
                listStream.RemoveAt(i);
            }
        });

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
            IsText = !IsText;
            if (!IsEdit)
            {
                SourceText = "Edit";
                updateData();
            }
            else
            {
                SourceText = "Save";
                if (SelectedStayPlace.imgSource != null)
                {
                    await Task.Run(() => {
                        IsLoading = true;
                        Thread.Sleep(500);
                    });

                    foreach (var i in SelectedStayPlace.imgSource)
                    {
                        listStream.Add(GetStreamFromUrl(i));
                    }

                }
                IsLoaded = true;
            }
            IsLoading = false;
        }
        private async void updateData()
        {
            DataManager.Ins.CurrentStayPlaceManager.description = Description;
            DataManager.Ins.CurrentStayPlaceManager.name = Name;
            DataManager.Ins.CurrentStayPlaceManager.imgSource = new ObservableCollection<string>();

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
        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                isLoading = value;
                OnPropertyChanged("IsLoading");
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

        private string sourceText;
        public string SourceText
        {
            get { return sourceText; }
            set
            {
                sourceText = value;
                OnPropertyChanged("SourceText");

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
