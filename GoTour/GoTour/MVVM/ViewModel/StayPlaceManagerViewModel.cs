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
    public class StayPlaceManagerViewModel : ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        private IMessageService _messageService;

        public StayPlaceManagerViewModel() { }
        public StayPlaceManagerViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
            
            ListStayPlace = DataManager.Ins.ListStayPlace;
            _messageService = DependencyService.Get<IMessageService>();
        }
        public ICommand NavigationBack => new Command<object>((obj) =>
        {
            navigation.PopAsync();
        });
        public ICommand NewStayPlaceCommand => new Command<object>((obj) =>
        {
            navigation.PushAsync(new NewStayPlaceView());
        });
        public ICommand DeleteCommand => new Command<object>(async (obj) =>
        {
            if (selectedStayPlace == null) return;
            List<Tour> listTour = new List<Tour>();
            foreach (Tour ite in DataManager.Ins.ListTour)
                listTour.Add(ite);

            List<Tour> tour_Has_electedStayPlace_List = new List<Tour>();
            //tour_Has_electedStayPlace_List = listTour.FindAll(e => e.SPforPList.Exists(p => p.stayPlaceId == selectedStayPlace.id));
            foreach (var e in listTour)
            {
                foreach (var p in e.SPforPList)
                {
                    if (p.stayPlaceId == selectedStayPlace.id)
                    {
                        tour_Has_electedStayPlace_List.Add(e);
                        break;
                    }
                }
            }

            if (tour_Has_electedStayPlace_List.Count == 0)
            {
                await DataManager.Ins.StayPlacesServices.DeleteStayPlace(selectedStayPlace);
                ListStayPlace.Remove(SelectedStayPlace);
                DependencyService.Get<IToast>().ShortToast("Delete Successful!");
            }
            else
            {
                string message = "Tours: ";
                foreach (Tour ite in tour_Has_electedStayPlace_List)
                    message = message + ite.name + ", ";
                DependencyService.Get<IToast>().LongToast(message + " has selected StayPlace, Please delete before doing this task!");
            }

        });
        public ICommand DeleteCommand1 => new Command<object>(async (obj) =>
        {
            bool answer = await _messageService.ShowOK_Cancel("Question?", "Are you sure you want to delete this stay place?");
            if (!answer) return;

            StayPlace selected = (StayPlace)obj;
            if (selected == null) return;        

            bool flag = false;
            List<Tour> listTour = new List<Tour>();

            foreach (Tour ite in DataManager.Ins.ListTour)
                listTour.Add(ite);

            //tour_Has_electedStayPlace_List = listTour.FindAll(e => e.SPforPList.Exists(p => p.stayPlaceId == place.id));
            foreach (var e in listTour)
            {
                foreach (var p in e.SPforPList)
                {
                    if (p.stayPlaceId == selected.id)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag) break;
            }            

            if (!flag)
            {
                await DataManager.Ins.StayPlacesServices.DeleteStayPlace(selected);
                ListStayPlace.Remove(selected);
            }
            else
            {
                foreach (StayPlace p in DataManager.Ins.ListStayPlace)
                {
                    if (p == selected)
                    {
                        p.isEnable = false;
                        await DataManager.Ins.StayPlacesServices.UpdateStayPlace(p);
                        break;
                    }
                }
                ListStayPlace.Remove(selected);

            }
            DependencyService.Get<IToast>().ShortToast("Delete Successful!");

        });

        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            StayPlace result = obj as StayPlace;
            if (result != null)
            {
                DataManager.Ins.CurrentStayPlaceManager = result;
                navigation.PushAsync(new EditStayPlaceView());
            }
            SelectedStayPlace = null;
        });


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
        private ObservableCollection<StayPlace> listStayPlace;
        public ObservableCollection<StayPlace> ListStayPlace
        {
            get { return listStayPlace; }
            set
            {
                listStayPlace = value;
                OnPropertyChanged("ListStayPlace");
            }
        }       
    }
}
