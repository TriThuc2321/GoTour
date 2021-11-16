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
    public class InvoicesServices
    {
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseClient storage = new FirebaseClient("gs://gotour-98c79.appspot.com");

        public List<FavouriteTour> favoritePlaces;

        public InvoicesServices() { }
        public async Task<List<Invoice>> GetAllInvoice()
        {
            return (await firebase
              .Child("Invoices")
              .OnceAsync<Invoice>()).Select(item => new Invoice
              {
                  id = item.Object.id,
                  discount = item.Object.discount,
                  discountMoney = item.Object.discountMoney,
                  price = item.Object.price,
                  isPaid = item.Object.isPaid,
                  payingTime = item.Object.payingTime,
                  amount = item.Object.amount,
                  method = item.Object.method,
                  total = item.Object.total
              }).ToList();

        }
        public async Task AddInvoice(Invoice invoice)
        {
            await firebase
              .Child("Invoices")
              .PostAsync(new Invoice()
              {
                  id = invoice.id,
                  discount = invoice.discount,
                  discountMoney = invoice.discountMoney,
                  price = invoice.price,
                  isPaid = invoice.isPaid,
                  payingTime = invoice.payingTime,
                  amount = invoice.amount,
                  method = invoice.method,
                  total = invoice.total
              });
        }

        public async Task DeleteInvoice(string id)
        {
            var toDelete = (await firebase
              .Child("Invoices")
              .OnceAsync<Invoice>()).Where(a => a.Object.id == id).FirstOrDefault();
            await firebase.Child("Invoices").Child(toDelete.Key).DeleteAsync();
        }
    }
}
