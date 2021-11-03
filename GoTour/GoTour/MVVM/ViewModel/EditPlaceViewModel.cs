using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class EditPlaceViewModel: ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        public Command EditTextCommand { get; }

        public EditPlaceViewModel() { }
        public EditPlaceViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.currentShell = curentShell;

            Imgs = new ObservableCollection<string>();

            EditTextCommand = new Command(editTextHandle);

            SelectedPlace = DataManager.Ins.CurrentPlaceManager;
            foreach(var i in SelectedPlace.imgSource)
            {
                Imgs.Add(i);
            }

            IsEdit = false;
            IsText = true;
        }

        private void editTextHandle(object obj)
        {
            IsEdit = !IsEdit;
            IsText = !IsText;
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
        private ObservableCollection<string> imgs;
        public ObservableCollection<string> Imgs
        {
            get { return imgs; }
            set
            {
                imgs = value;
                OnPropertyChanged("Imgs");
            }
        }
    }
}
