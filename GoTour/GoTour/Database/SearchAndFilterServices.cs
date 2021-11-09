using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using GoTour.MVVM.Model;
using Xamarin.Forms;

namespace GoTour.Database
{
    public class SearchAndFilterServices
    {
        //Input from User only be Place name where they want to find
        public string PlaceToSearch { get; set; }

        //This part is filter properties
        public string CostFilter { get; set; }
        public string AreFilter { get; set; }
        public string StartTimeFilter { get; set; }
        public string DurationFilter { get; set; }

        public ObservableCollection<Tour> ResultList { get; set; }
        public void RefreshDataSearch()
        {
            PlaceToSearch = "";
        }
        public void GetResult()
        {
            ResultList = new ObservableCollection<Tour>();
            List<Place> temp1 = new List<Place>(); //Lisrt place
            List<Tour> temp2 = new List<Tour>(); //List Tour
            foreach (Place ite in DataManager.Ins.ListPlace)
            {
                temp1.Add(ite);
            }
            foreach (Tour ite in DataManager.Ins.ListTour)
            {
                temp2.Add(ite);
            }

            List<Tour> temp = new List<Tour>();
            List<Tour> result = new List<Tour>();


            //Lay Id cua Place do ng dung nhap vao
            string currentPlaceId = "";
            foreach(Place i in temp1)
            {
                if (PlaceToSearch.Equals(i.name, StringComparison.CurrentCultureIgnoreCase))
                {
                    currentPlaceId = i.id;
                }
            }
          
            //Loc ID 
            temp = temp2.FindAll(e => e.placeDurationList.Exists(p => p.placeId == currentPlaceId));
                foreach (var plc in temp)
                    if (!result.Contains(plc))
                        result.Add(plc);

            foreach (Tour ite3 in result)
            {
                ResultList.Add(ite3);
            }
        }

    }
}
