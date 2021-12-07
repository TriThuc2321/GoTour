using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class PlaceManagerViewModel: ObservableObject
    {
        INavigation navigation;
        Shell currentShell;

        private readonly IMessageService _messageService;
        public PlaceManagerViewModel() { }
        public PlaceManagerViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            ListPlace = new ObservableCollection<Place>();
            foreach(Place p in DataManager.Ins.ListPlace)
            {
                if (p.isEnable) ListPlace.Add(p);
            }
            _messageService = DependencyService.Get<IMessageService>();

            //Show the dialog with next line
           
        }
        public ICommand NavigationBack => new Command<object>((obj) =>
        {
            navigation.PopAsync();
        });
        public ICommand DeleteCommand => new Command<object>(async (obj) =>
        {
            bool answer = await _messageService.ShowOK_Cancel("Question?", "Are you sure you want to delete this place?");
            if (!answer) return;
            var place = obj as Place;
            if (place == null) return;            

            bool flag = false;
            List<Tour> listTour = new List<Tour>();
            foreach (Tour ite in DataManager.Ins.ListTour)
                listTour.Add(ite);

            //tour_Has_electedStayPlace_List = listTour.FindAll(e => e.SPforPList.Exists(p => p.stayPlaceId == place.id));
            foreach (var e in listTour)
            {
                foreach (var p in e.SPforPList)
                {
                    if (p.placeId == place.id)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag) break;
            }

            if(!flag)
            {
                List<StayPlace> stayPlaces = new List<StayPlace>();
                foreach (StayPlace ite in DataManager.Ins.ListStayPlace)
                    stayPlaces.Add(ite);

                for (int i = 0; i < stayPlaces.Count; i++)
                {
                    string[] idList = stayPlaces[i].placeId.Split(',');
                    for (int k = 0; k < idList.Length; k++)
                    {
                        if (idList[k] == place.id)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag) break;

                }
            }

            if (!flag)
            {
                await DataManager.Ins.PlacesServices.DeletePlace(place);
                ListPlace.Remove(place);                
            }
            else
            {
                foreach (Place p in DataManager.Ins.ListPlace)
                {
                    if (p == place)
                    {
                        p.isEnable = false;
                        await DataManager.Ins.PlacesServices.UpdatePlace(p);
                        break;
                    }
                }
                ListPlace.Remove(place);

            }
            DependencyService.Get<IToast>().ShortToast("Delete Successful!");

        });
        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            Place result = obj as Place;
            if(result != null)
            {
                DataManager.Ins.CurrentPlaceManager = result;
                navigation.PushAsync(new EditPlaceView());
            }      
            SelectedPlace = null;
        });

        public ICommand NewPlaceCommand => new Command<object>((obj) =>
        {
            navigation.PushAsync(new NewPlaceView());

        });
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
        private ObservableCollection<Place> listPlace;
        public ObservableCollection<Place> ListPlace
        {
            get { return listPlace; }
            set
            {
                listPlace = value;
                OnPropertyChanged("ListPlace");
            }
        }
    }
}
