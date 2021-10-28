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
    class ToursServices
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
              }).ToList();
        }
        public async Task AddTour(string _id, string _name, List<string> _imgSource, string _startTime, string _duration, List<string> _tourGuide, string _passengerNumber, string _description, bool _isOccured)
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
              });
        }
    }
}
