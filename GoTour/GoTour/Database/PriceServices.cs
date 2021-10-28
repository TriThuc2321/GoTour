using Firebase.Database;
using Firebase.Database.Query;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoTour.Database
{
    public class PriceServices
    {
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");

        async public Task addPrice(Price price)
        {
            await firebase
              .Child("Prices")
              .PostAsync(new Price()
              {
                    id = price.id,
                    name = price.name,
                    price = price.price,
                    hotleId = price.hotleId,
               });
        }
    }
}
