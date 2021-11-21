using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class SearchResultViewModel : ObservableObject
    {
        INavigation navigation;


        //KHAI BAO COMMAND
        public Command NavigationBack { get; }
        public Command OpenSortTypeListCommand { get; }


        //KHAI BAO LIST REUSLT
        private ObservableCollection<Tour> listTourFromSelectedPlace;
        public ObservableCollection<Tour> ListTourFromSelectedPlace
        {
            get { return listTourFromSelectedPlace; }
            set
            {
                listTourFromSelectedPlace = value;
                OnPropertyChanged("ListTourFromSelectedPlace");
            }
        }


        //CONSTRUCTOR
        public SearchResultViewModel() { }
        

        //KHAI BAO SELECTED TOUR
        private Tour selectedTour;
        public Tour SelectedTour
        {
            get { return selectedTour; }
            set
            {
                selectedTour = value;
                OnPropertyChanged("SelectedTour");

            }
        }



        //SELECTED ITEM COMMAND
        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            Tour result = obj as Tour;
            if (result != null)
            {
                DataManager.Ins.currentTour = result;
                navigation.PushAsync(new DetailTourView());
                SelectedTour = null;
            }
        });


        //SESARCH RESULT VIEWMODEL
        public SearchResultViewModel(INavigation navigation)
        {
            OpenSortTypeListCommand = new Command(openSortMenu);


            ListTourFromSelectedPlace = new ObservableCollection<Tour>();
            NavigationBack = new Command(() => navigation.PopAsync());
            this.navigation = navigation;
            foreach (Tour ite in DataManager.Ins.SearchServices.ResultList)
            {
                ListTourFromSelectedPlace.Add(ite);
            }
        }



                 //SORT BACKEND
        async void openSortMenu()
        {
            //DependencyService.Get<IToast>().ShortToast("Here is Open Sort");
            string action = await Application.Current.MainPage.DisplayActionSheet("Sort with: ", "Cancel", null, "Increasing price", "Decreasing price","Soon", "Late");
            switch (action)
            {
                case "Increasing price":
                    IncreasingPriceSort();
                    break;
                case "Decreasing price":
                    DecreasingPriceSort();
                    break;
                case "Soon":
                    OccurSoonSort();
                    break;
                case "Late":
                    OccurLateSort();
                    break;
            }
        }
        private void IncreasingPriceSort()
        {
            if(ListTourFromSelectedPlace.Count == 1)
            {
                return;
            }
            else
            {
                for(int i = 0; i < ListTourFromSelectedPlace.Count; i++)
                {
                    for(int j = i + 1; j < ListTourFromSelectedPlace.Count; j++)
                    {
                        if (Int32.Parse(ListTourFromSelectedPlace[j].basePrice) < Int32.Parse(ListTourFromSelectedPlace[i].basePrice))
                        {
                            Tour temp = new Tour();
                            temp = ListTourFromSelectedPlace[i];
                            ListTourFromSelectedPlace[i] = ListTourFromSelectedPlace[j];
                            ListTourFromSelectedPlace[j] = temp;
                        }
                    }    
                }
                return;
            }
            
        }
        private void DecreasingPriceSort()
        {
            if (ListTourFromSelectedPlace.Count == 1)
            {
                return;
            }
            else
            {
                for (int i = 0; i < ListTourFromSelectedPlace.Count; i++)
                {
                    for (int j = i + 1; j < ListTourFromSelectedPlace.Count; j++)
                    {
                        if (Int32.Parse(ListTourFromSelectedPlace[j].basePrice) > Int32.Parse(ListTourFromSelectedPlace[i].basePrice))
                        {
                            Tour temp = new Tour();
                            temp = ListTourFromSelectedPlace[i];
                            ListTourFromSelectedPlace[i] = ListTourFromSelectedPlace[j];
                            ListTourFromSelectedPlace[j] = temp;
                        }
                    }
                }
                return;
            }
        }
        private void OccurSoonSort()
        {
            DateTime now = DateTime.Now;
            if(ListTourFromSelectedPlace.Count == 1)
            {
                return;
            }else
            {
                for(int i = 0; i < ListTourFromSelectedPlace.Count; i++)
                {
                    for(int j = i + 1; j < ListTourFromSelectedPlace.Count; j++)
                    {
                        DateTime dti = DateTime.Parse(ListTourFromSelectedPlace[i].startTime);
                        DateTime dtj = DateTime.Parse(ListTourFromSelectedPlace[j].startTime);
                        if (dtj < dti)
                        {
                            Tour temp = new Tour();
                            temp = ListTourFromSelectedPlace[i];
                            ListTourFromSelectedPlace[i] = ListTourFromSelectedPlace[j];
                            ListTourFromSelectedPlace[j] = temp;
                        }
                    }
                }
            }
        }
        private void OccurLateSort()
        {
            DateTime now = DateTime.Now;
            if (ListTourFromSelectedPlace.Count == 1)
            {
                return;
            }
            else
            {
                for (int i = 0; i < ListTourFromSelectedPlace.Count; i++)
                {
                    for (int j = i + 1; j < ListTourFromSelectedPlace.Count; j++)
                    {
                        DateTime dti = DateTime.Parse(ListTourFromSelectedPlace[i].startTime);
                        DateTime dtj = DateTime.Parse(ListTourFromSelectedPlace[j].startTime);
                        if (dtj > dti)
                        {
                            Tour temp = new Tour();
                            temp = ListTourFromSelectedPlace[i];
                            ListTourFromSelectedPlace[i] = ListTourFromSelectedPlace[j];
                            ListTourFromSelectedPlace[j] = temp;
                        }
                    }
                }
            }
        }
    }
}
