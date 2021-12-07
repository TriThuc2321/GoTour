using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoTour.Database
{
    public class PlacesServices
    {
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseStorage storage = new FirebaseStorage("gotour-98c79.appspot.com");

        public List<Place> places;
        public PlacesServices() {
        }
        public async Task<List<Place>> GetAllPlaces()
        {
            return (await firebase
              .Child("Places")
              .OnceAsync<Place>()).Select(item => new Place
              {
                  id = item.Object.id,
                  name = item.Object.name,
                  imgSource = item.Object.imgSource,
                  description = item.Object.description,
              }).ToList();
        }
        public async Task AddPlace(string id, string name, List<string> imgSource, string description)
        {
            await firebase
              .Child("Places")
              .PostAsync(new Place()
              {
                  id = id,
                  name = name,
                  imgSource = imgSource,
                  description = description,
              });
        }
        public async Task UpdatePlace(Place place)
        {
            var toUpdatePlace = (await firebase
              .Child("Places")
              .OnceAsync<Place>()).Where(a => a.Object.id == place.id).FirstOrDefault();

            await firebase
              .Child("Places")
              .Child(toUpdatePlace.Key)
              .PutAsync(new Place
              {
                  id = place.id,
                  name = place.name,
                  imgSource = place.imgSource,
                  description = place.description,
              });
        }
        async public Task<string> saveImage(Stream imgStream, string placeId, int id)
        {
            try
            {
                var stroageImage = await new FirebaseStorage("gotour-98c79.appspot.com")
                .Child("Places").Child(placeId)
                .Child(id + ".png")
                .PutAsync(imgStream);
                var imgurl = stroageImage;
                return imgurl;
            }
            catch (Exception e)
            {
                return null;
            }
            
        }
        
        public async Task DeleteFile(string folderPlaceId, int id)
        {
            try
            {
                await new FirebaseStorage("gotour-98c79.appspot.com")
                 .Child("Places")
                 .Child(folderPlaceId).Child(id + ".png")
                 .DeleteAsync();
            }
            catch { }

            try
            {
                await new FirebaseStorage("gotour-98c79.appspot.com")
                 .Child("Places")
                 .Child(folderPlaceId).Child(id + ".jpg")
                 .DeleteAsync();
            }
            catch { }

        }

        public async Task DeletePlace(Place place)
        {
            var toDeleted = (await firebase
               .Child("Places").OnceAsync<Place>()).FirstOrDefault(p => p.Object.id == place.id);

            await firebase.Child("Places").Child(toDeleted.Key).DeleteAsync();

            for (int i = 0; i < place.imgSource.Count; i++)
            {
                await DeleteFile(place.id, i);
            }

            

            
        }

    }
}