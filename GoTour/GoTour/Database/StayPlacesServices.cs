using Firebase.Database;
using Firebase.Database.Query;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoTour.Database
{
    public class StayPlacesServices
    {
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseClient storage = new FirebaseClient("gs://gotour-98c79.appspot.com");

        public List<StayPlace> places;
        public StayPlacesServices() { }
        public async Task<List<StayPlace>> GetAllStayPlaces()
        {
            return (await firebase
              .Child("StayPlaces")
              .OnceAsync<StayPlace>()).Select(item => new StayPlace
              {
                  id = item.Object.id,
                  name = item.Object.name,                
                  imgSource = item.Object.imgSource,
                  address = item.Object.address,
                  description = item.Object.description,                 
                  placeId = item.Object.placeId,
              }).ToList();
        }
        public async Task AddStayPlace(string id, string name, List<string> imgSource, string description, string placeId, string address)
        {
            await firebase
              .Child("StayPlaces")
              .PostAsync(new StayPlace()
              {
                  id = id,
                  name = name,
                  address = address,
                  placeId = placeId,
                  imgSource = imgSource,
                  description = description,
              });
        }
    }
}
