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
    public class TourPlaceServices
    {
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");
        async public Task AddTourPlace(TourPlace tourPlace)
        {
            await firebase
              .Child("TourPlaces")
              .PostAsync(new TourPlace()
              {
                  tourId = tourPlace.tourId,
                  placeDurationList = tourPlace.placeDurationList
        });
        }

        public async Task<List<TourPlace>> GetAllTourPlaces()
        {
            return (await firebase
              .Child("TourPlaces")
              .OnceAsync<TourPlace>()).Select(item => new TourPlace
              {
                  tourId = item.Object.tourId,
                  placeDurationList = item.Object.placeDurationList
              }).ToList();
        }

        public async Task UpdateTourPlace(TourPlace place)
        {
            var toUpdatePlace = (await firebase
              .Child("TourPlaces")
              .OnceAsync<TourPlace>()).Where(a => a.Object.tourId == place.tourId).FirstOrDefault();
            if(toUpdatePlace != null)
            {
                await firebase
              .Child("TourPlaces")
              .Child(toUpdatePlace.Key)
              .PutAsync(new TourPlace
              {
                  tourId = place.tourId,
                  placeDurationList = place.placeDurationList
              });
            }
            else
            {
                await AddTourPlace(place);
            }
            
        }

        public async Task DeleteTourPlace(string id)
        {
            var toDeleted = (await firebase
               .Child("TourPlaces").OnceAsync<TourPlace>()).FirstOrDefault(p => p.Object.tourId == id);

            await firebase.Child("TourPlaces").Child(toDeleted.Key).DeleteAsync();
        }
    }
}
