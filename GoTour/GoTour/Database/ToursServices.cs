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
                  remaining = item.Object.remaining
              }).ToList();
        }
        public async Task AddTour(string _id, string _name, List<string> _imgSource, List<PlaceId_StayPlace> _SPforPList, string _startTime, string _duration, List<string> _tourGuide, string _passengerNumber, string _description,string basePrice, bool _isOccured, string _remaining)
        {
            await firebase
              .Child("Tours")
              .PostAsync(new Tour()
              {
                  id = _id,
                  name = _name,
                  imgSource = _imgSource,
                  startTime = _startTime,
                  duration = _duration,
                  tourGuide = _tourGuide,
                  passengerNumber = _passengerNumber,
                  description = _description,
                  isOccured = _isOccured,
                  basePrice = basePrice,
                  SPforPList = _SPforPList,
                  remaining = _remaining
              }) ;
        }

        public async Task UpdateTour(Tour tour)
        {
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
                  placeDurationList = tour.placeDurationList,
                  basePrice =tour.basePrice,
                  SPforPList = tour.SPforPList,
                  remaining = tour.remaining
              });

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
