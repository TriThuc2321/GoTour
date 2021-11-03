﻿using Firebase.Database;
using Firebase.Database.Query;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoTour.Database
{
    public class BookedTicketServices
    {
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseClient storage = new FirebaseClient("gs://gotour-98c79.appspot.com");

        public List<BookedTicketServices> bookedTickets;

        public BookedTicketServices() { }
        public async Task<List<BookedTicket>> GetAllBookedTicket()
        {
            return (await firebase
              .Child("BookedTickets")
              .OnceAsync<BookedTicket>()).Select(item => new BookedTicket
              {
                  id = item.Object.id,
                  tour = item.Object.tour,
                  name = item.Object.name,
                  birthday = item.Object.birthday,
                  contact = item.Object.contact,
                  email = item.Object.email,
                  cmnd = item.Object.cmnd,
                  address = item.Object.address,
                  bookTime = item.Object.bookTime,
                  invoice = item.Object.invoice
                 
              }).ToList();
        }
        public async Task AddBookedTicket(BookedTicket bookedTicket)
        {
            await firebase
              .Child("BookedTickets")
              .PostAsync(new BookedTicket()
              {
                  id = bookedTicket.id,
                  tour = new Tour(){ id = bookedTicket.tour.id },
                  name = bookedTicket.name,
                  birthday = bookedTicket.birthday,
                  contact = bookedTicket.contact,
                  email = bookedTicket.email,
                  cmnd = bookedTicket.cmnd,
                  address = bookedTicket.address,
                  bookTime = bookedTicket.bookTime,
                  invoice = bookedTicket.invoice

              });
        }

        public async Task DeleteBookedTicket(string id)
        {
            var toDelete = (await firebase
              .Child("BookedTickets")
              .OnceAsync<BookedTicket>()).Where(a => a.Object.id == id).FirstOrDefault();
            await firebase.Child("BookedTickets").Child(toDelete.Key).DeleteAsync();
        }
    }
}

