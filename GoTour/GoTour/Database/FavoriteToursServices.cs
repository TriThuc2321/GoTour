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
    class FavoriteToursServices
    {
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseClient storage = new FirebaseClient("gs://gotour-98c79.appspot.com");

        public List<FavouriteTour> favoritePlaces;
        public FavoriteToursServices() { }
        public async Task<List<FavouriteTour>> GetAllPlaces()
        {
            return (await firebase
              .Child("Favourites")
              .OnceAsync<FavouriteTour>()).Select(item => new FavouriteTour
              {
                  id = item.Object.id,
                  tourId = item.Object.tourId,
                  userId = item.Object.userId
              }).ToList();
        }
        public async Task AddFavouritePlace(FavouriteTour tour)
        {
            await firebase
              .Child("Favourites")
              .PostAsync(new FavouriteTour()
              {
                  id = tour.id,
                  tourId = tour.tourId,
                  userId = tour.userId,
              });
        }

        public async Task DeleteFavoritePlace(string id)
        {
            var toDelete = (await firebase
              .Child("Favourites")
              .OnceAsync<FavouriteTour>()).Where(a => a.Object.id == id).FirstOrDefault();
            await firebase.Child("Favourites").Child(toDelete.Key).DeleteAsync();
        }
    }
}
