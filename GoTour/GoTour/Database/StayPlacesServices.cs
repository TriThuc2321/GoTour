using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
                  isEnable = item.Object.isEnable
              }).ToList();
        }

        public async Task DeleteStayPlace(StayPlace stayplace)
        {
            var toDeleted = (await firebase
               .Child("StayPlaces").OnceAsync<StayPlace>()).FirstOrDefault(p => p.Object.id == stayplace.id);

            await firebase.Child("StayPlaces").Child(toDeleted.Key).DeleteAsync();

            for (int i = 0; i < stayplace.imgSource.Count; i++)
            {
                await DeleteFile(stayplace.id, i);
            }
            
    
        } 

        public async Task AddStayPlace(string id, string name, ObservableCollection<string> imgSource, string description, string placeId, string address, bool isEnable)
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
                  isEnable = isEnable
              });
        }
        public async Task DeleteFile(string folderStayPlaceId, int id)
        {
            try
            {
                await new FirebaseStorage("gotour-98c79.appspot.com")
                 .Child("StayPlace")
                 .Child(folderStayPlaceId).Child(id + ".jpg")
                 .DeleteAsync();
            }
            catch
            {
            }
            try
            {
                await new FirebaseStorage("gotour-98c79.appspot.com")
                 .Child("StayPlace")
                 .Child(folderStayPlaceId).Child(id + ".jpg")
                 .DeleteAsync();
            }
            catch { }
            
        }
        async public Task<string> saveImage(Stream imgStream, string stayPlaceId, int id)
        {
            var stroageImage = await new FirebaseStorage("gotour-98c79.appspot.com")
                .Child("StayPlace").Child(stayPlaceId)
                .Child(id + ".png")
                .PutAsync(imgStream);
            var imgurl = stroageImage;
            return imgurl;
        }
        public async Task UpdateStayPlace(StayPlace place)
        {
            var toUpdatePlace = (await firebase
              .Child("StayPlaces")
              .OnceAsync<StayPlace>()).Where(a => a.Object.id == place.id).FirstOrDefault();

            await firebase
              .Child("StayPlaces")
              .Child(toUpdatePlace.Key)
              .PutAsync(new StayPlace
              {
                  id = place.id,
                  name = place.name,
                  imgSource = place.imgSource,
                  description = place.description,
                  address = place.address,
                  placeId = place.placeId,
                  isEnable = place.isEnable
              });
        }
    }
}
