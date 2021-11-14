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
    public class ReviewService
    {
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");

        public async Task<List<Review>> GetAllReviews()
        {
            return (await firebase
              .Child("Reviews")
              .OnceAsync<Review>()).Select(item => new Review
              {
                  tourId = item.Object.tourId,
                  email = item.Object.email,
                  message = item.Object.message,
                  time = item.Object.time,
                  starNumber = item.Object.starNumber,
              }).ToList();
        }
        public async Task Add(Review review)
        {
            await firebase
              .Child("Reviews")
              .PostAsync(new Review()
              {
                  tourId = review.tourId,
                  email = review.email,
                  message = review.message,
                  time = review.time,
                  starNumber = review.starNumber,
              });
        }

        public async Task UpdateReview(Review review)
        {
            var toUpdateReview = (await firebase
                 .Child("Reviews")
                 .OnceAsync<Review>()).Where(a => a.Object.tourId == review.tourId && a.Object.email == review.email).FirstOrDefault();

            await firebase
              .Child("Reviews")
              .Child(toUpdateReview.Key)
              .PutAsync(new Review
              {
                  tourId = review.tourId,
                  email = review.email,
                  message = review.message,
                  time = review.time,
                  starNumber = review.starNumber,
              });

        }
    }
}
