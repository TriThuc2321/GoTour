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
        async public Task addTourPlace(TourPlace tourPlace)
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
    }
}
