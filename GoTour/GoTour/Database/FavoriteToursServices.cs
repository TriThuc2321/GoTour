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
    public class FavoriteToursServices
    {
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseClient storage = new FirebaseClient("gs://gotour-98c79.appspot.com");

        public List<FavouriteTour> favoritePlaces;

        public FavoriteToursServices() { }
        public async Task<List<FavouriteTour>> GetAllFavourite()
        {
             return (await firebase
              .Child("Favourites")
              .OnceAsync<FavouriteTour>()).Select(item => new FavouriteTour
              {
                  id = item.Object.id,
                  tour = item.Object.tour,
                  email = (string)item.Object.email,
                  //.Where(email => email.ToString() == DataManager.Ins.CurrentUser.email),
              }).ToList();

        }
        public async Task AddFavouriteTour(FavouriteTour favourite)
        {
            await firebase
              .Child("Favourites")
              .PostAsync(new FavouriteTour()
              {
                  id = favourite.id,
                  tour = new Tour { id = favourite.tour.id },
                  email = favourite.email,
              });
        }

        public async Task DeleteFavoriteTour(string id)
        {
            var toDelete = (await firebase
              .Child("Favourites")
              .OnceAsync<FavouriteTour>()).Where(a => a.Object.id == id).FirstOrDefault();
            await firebase.Child("Favourites").Child(toDelete.Key).DeleteAsync();
        }
    }
}
