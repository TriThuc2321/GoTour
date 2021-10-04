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
    public class PlacesServices
    {
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");

        public List<Place> places;
        public  PlacesServices()
        {
            
        }
        public async Task<List<Place>> GetAllPlaces()
        {
            return (await firebase
              .Child("Places")
              .OnceAsync<Place>()).Select(item => new Place
              {
                  id = item.Object.id,
                  country = item.Object.country,
                  title = item.Object.title,
                  imgSource = item.Object.imgSource
              }).ToList();
        }
        public async Task AddPlace(string id, string country, string title, string imgSource)
        {
            await firebase
              .Child("Places")
              .PostAsync(new Place()
              {
                  id = id,
                  country = country,
                  title = title,
                  imgSource = imgSource
              });
        }
    }
}
