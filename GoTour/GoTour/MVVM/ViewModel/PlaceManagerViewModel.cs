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

            ListPlace = DataManager.Ins.ListPlace;
            _messageService = DependencyService.Get<IMessageService>();

            //Show the dialog with next line
           
        }
        public ICommand NavigationBack => new Command<object>((obj) =>
        {
            navigation.PopAsync();
        });
        public ICommand DeleteCommand => new Command<object>(async (obj) =>
        {
            var place = obj as Place;
            if (place == null) return;
            List<Tour> listTour = new List<Tour>();
            foreach (Tour ite in DataManager.Ins.ListTour)
                listTour.Add(ite);

            List<Tour> tour_Has_electedStayPlace_List = new List<Tour>();
            //tour_Has_electedStayPlace_List = listTour.FindAll(e => e.SPforPList.Exists(p => p.stayPlaceId == place.id));
            foreach (var e in listTour)
            {
                foreach (var p in e.SPforPList)
                {
                    if (p.stayPlaceId == place.id)
                    {
                        tour_Has_electedStayPlace_List.Add(e);
                        break;
                    }
                }
            }


            List<StayPlace> stayPlaces = new List<StayPlace>();
            foreach (StayPlace ite in DataManager.Ins.ListStayPlace)
                stayPlaces.Add(ite);

            List<StayPlace> placeHasStayPlaceList = new List<StayPlace>();           
            
             for(int i =0; i<stayPlaces.Count; i++)
             {
                 string[] idList = stayPlaces[i].placeId.Split(',');
                 for(int k = 0; k< idList.Length; k++)
                 {
                     if (idList[k] == place.id)
                     {
                        placeHasStayPlaceList.Add(stayPlaces[i]);
                         break;
                     }
                 }
                 
             }

            if (tour_Has_electedStayPlace_List.Count == 0 && placeHasStayPlaceList.Count == 0)
            {
                await DataManager.Ins.PlacesServices.DeletePlace(place);
                ListPlace.Remove(place);
                DependencyService.Get<IToast>().ShortToast("Delete Successful!");
            }
            else
            {
                string message1 = "";
                string message2 = "";

                foreach (Tour ite in tour_Has_electedStayPlace_List)
                    message1 = message1 + ite.name + ", ";
                if (message1 != "")
                {
                    message1 = message1.Remove(message1.Length - 2, 2);
                    message1 += "tours has been conflicted, please delete before doing this task!/n";
                }
                
                //DependencyService.Get<IToast>().LongToast(message + " has selected Place, please delete before doing this task!");

                foreach (StayPlace ite in placeHasStayPlaceList)
                    message2 = message2 + ite.name + ", ";
                if (message2 != "")
                {
                    message2 = message2.Remove(message2.Length - 2, 2);
                    message2 += "stay places has been conflicted, please delete before doing this task!";
                }

                await _messageService.ShowAsync("Warning", message1 + message2);
            }                     
            
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
