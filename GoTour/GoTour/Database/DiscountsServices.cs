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
    public class DiscountsServices
    {
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");

        async public Task addDiscount(Discount discount)
        {
            await firebase
              .Child("Prices")
              .PostAsync(new Discount()
              {
                  id = discount.id,
                  percent = discount.percent,
                  isUsed = discount.isUsed,
                  total = discount.total,
              }) ;
        }

        public async Task<List<Discount>> GetAllDiscounts()
        {
            return (await firebase
              .Child("Discounts")
              .OnceAsync<Discount>()).Select(item => new Discount
              {
                  id = item.Object.id,
                  percent = item.Object.percent,
                  isUsed = item.Object.isUsed,
                  total = item.Object.total,
              }).ToList();
        }

        public async Task DeleteDiscount(string id)
        {
            var toDelete = (await firebase
              .Child("Discounts")
              .OnceAsync<FavouriteTour>()).Where(a => a.Object.id == id).FirstOrDefault();
            await firebase.Child("Discounts").Child(toDelete.Key).DeleteAsync();
        }

        //public async Task<Discount> GetDiscountById(string id)
        //{
        //    List<Discount> list = await GetAllDiscounts();

        //    Discount temp = new Discount();

        //    foreach (var discount in list)
        //    {
        //        if (id == discount.id)
        //            temp = discount;
        //    }

        //    return temp;
        //}


    }
}
