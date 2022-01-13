using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class EditPlaceViewModel: ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        int count;
        bool IsLoaded = false;
        List<Stream> listStream;

        public Command EditTextCommand { get; }
        public Command AddCommand { get; }
        public Command DeleteCommand { get; }
        public Command NavigationBack { get; }

        public EditPlaceViewModel() { }
        public EditPlaceViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.currentShell = curentShell;
            IsLoading = false;

            Imgs = new ObservableCollection<ImageSource>();

            EditTextCommand = new Command(editTextHandle);
            AddCommand = new Command(addHandleAsync);
            NavigationBack = new Command(() => navigation.PopAsync());

            SelectedPlace = DataManager.Ins.CurrentPlaceManager;
            listStream = new List<Stream>();

            if(SelectedPlace.imgSource!= null)
            {
                foreach (var i in SelectedPlace.imgSource)
                {
                    //Imgs.Add(ImageSource.FromUri(new Uri(i)));
                    Imgs.Add(i);
                }
            }            

            Name = DataManager.Ins.CurrentPlaceManager.name;
            Description = DataManager.Ins.CurrentPlaceManager.description;
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

            if(imgData!= null)
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
                if (Name == null || Name == "")
                {
                    DependencyService.Get<IToast>().ShortToast("Please enter place's name");
                    IsEdit = !IsEdit;
                    IsText = !IsText;
                    return;
                }
                else if (Name == null || Description == null || Name == "" || Description == "")
                {
                    DependencyService.Get<IToast>().ShortToast("Please enter place's description");
                    IsEdit = !IsEdit;
                    IsText = !IsText;
                    return;
                }
                else if (Imgs.Count() == 0)
                {
                    DependencyService.Get<IToast>().ShortToast("Please insert place's image");
                    IsEdit = !IsEdit;
                    IsText = !IsText;
                    return;
                }                
                updateData();
                SourceText = "Edit";
            }
            else
            {
                SourceText = "Save";
                if (!IsLoaded)
                {
                    if (SelectedPlace.imgSource != null)
                    {    
                        
                        await Task.Run(() => {
                            IsLoading = true;
                            Thread.Sleep(100);
                        });
                        
                        foreach (var i in SelectedPlace.imgSource)
                        {
                            listStream.Add(GetStreamFromUrl(i));
                        }
                        
                    }
                    IsLoaded = true;
                }                
            }
            IsLoading = false;
        }
        
        private async void updateData()
        {

            DataManager.Ins.CurrentPlaceManager.description = Description;
            DataManager.Ins.CurrentPlaceManager.name = Name;
            DataManager.Ins.CurrentPlaceManager.imgSource = new ObservableCollection<string>();

            for (int i = 0; i < count; i++)
            {
                await DataManager.Ins.PlacesServices.DeleteFile(DataManager.Ins.CurrentPlaceManager.id, i);
            }

            Imgs = new ObservableCollection<ImageSource>();
            for (int i = 0; i < listStream.Count(); i++)
            {
                string url = await DataManager.Ins.PlacesServices.saveImage(listStream[i], DataManager.Ins.CurrentPlaceManager.id, i);
                DataManager.Ins.CurrentPlaceManager.imgSource.Add(url);
                Imgs.Add(url);
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
