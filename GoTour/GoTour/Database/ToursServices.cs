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
    public class ToursServices
    {
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseClient storage = new FirebaseClient("gs://gotour-98c79.appspot.com");

        List<Tour> tours;
        public ToursServices() { }

        public async Task<List<Tour>> GetAllTours()
        {
            return (await firebase
              .Child("Tours")
              .OnceAsync<Tour>()).Select(item => new Tour
              {
                  id = item.Object.id,
                  name = item.Object.name,
                  imgSource = item.Object.imgSource,
                  startTime = item.Object.startTime,
                  duration = item.Object.duration,
                  tourGuide = item.Object.tourGuide,
                  passengerNumber = item.Object.passengerNumber,
                  description = item.Object.description,
                  isOccured = item.Object.isOccured,
                  placeDurationList = null,
                  basePrice = item.Object.basePrice,
                  SPforPList = item.Object.SPforPList,
                  remaining = item.Object.remaining,
                  reviewList = null,
                  starNumber = item.Object.starNumber
              }).ToList();
        }
        public async Task AddTour(Tour tour)
        {
            await firebase
              .Child("Tours")
              .PostAsync(new Tour()
              {
                  id = tour.id,
                  name = tour.name,
                  imgSource = tour.imgSource,
                  startTime = tour.startTime,
                  duration = tour.duration,
                  tourGuide = tour.tourGuide,
                  passengerNumber = tour.passengerNumber,
                  description = tour.description,
                  isOccured = false,
                  basePrice = tour.basePrice,
                  SPforPList = tour.SPforPList,
                  remaining = tour.passengerNumber,
                  starNumber = "0", 
                  reviewList = null,
              });
        }

        public async Task UpdateTour(Tour tour)
        {
            int day = 0;
            int night = 0;
            for(int i =0; i< tour.placeDurationList.Count; i++)
            {
                day += tour.placeDurationList[i].day;
                night += tour.placeDurationList[i].night;
            }
            tour.duration = day + "/" + night;

            var toUpdateTour = (await firebase
                 .Child("Tours")
                 .OnceAsync<Tour>()).Where(a => a.Object.id == tour.id).FirstOrDefault();

            await firebase
              .Child("Tours")
              .Child(toUpdateTour.Key)
              .PutAsync(new Tour
              {
                  id = tour.id,
                  name = tour.name,
                  imgSource = tour.imgSource,
                  startTime = tour.startTime,
                  duration = tour.duration,
                  tourGuide = tour.tourGuide,
                  passengerNumber = tour.passengerNumber,
                  description = tour.description,
                  isOccured = tour.isOccured,
                  placeDurationList = null,
                  basePrice =tour.basePrice,
                  SPforPList = tour.SPforPList,
                  remaining = tour.remaining,
                  starNumber = tour.starNumber
              });

        }

        async public Task<string> saveImage(Stream imgStream, string placeId, int id)
        {
            var stroageImage = await new FirebaseStorage("gotour-98c79.appspot.com")
                .Child("Tours").Child(placeId)
                .Child(id + ".png")
                .PutAsync(imgStream);
            var imgurl = stroageImage;
            return imgurl;
        }

        public async Task DeleteFile(string folderPlaceId, int id)
        {
            try
            {
                await new FirebaseStorage("gotour-98c79.appspot.com")
                 .Child("Tours")
                 .Child(folderPlaceId).Child(id + ".png")
                 .DeleteAsync();
            }
            catch { }

            try
            {
                await new FirebaseStorage("gotour-98c79.appspot.com")
                 .Child("Tours")
                 .Child(folderPlaceId).Child(id + ".jpg")
                 .DeleteAsync();
            }
            catch { }

        }

        public async Task DeletePlace(Tour place)
        {
            var toDeleted = (await firebase
               .Child("Tours").OnceAsync<Tour>()).FirstOrDefault(p => p.Object.id == place.id);

            await firebase.Child("Tours").Child(toDeleted.Key).DeleteAsync();

            for (int i = 0; i < place.imgSource.Count; i++)
            {
                await DeleteFile(place.id, i);
            }




        }

        public async Task<Tour> FindTourById(string id)
        {
            var all = await GetAllTours();
            await firebase
                .Child("Tours")
                .OnceAsync<Tour>();
            return all.Where(a => a.id == id).FirstOrDefault();
        }
    }
}
