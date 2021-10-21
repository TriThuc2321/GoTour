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
    public class PlacesServices
    {
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseClient storage = new FirebaseClient("gs://gotour-98c79.appspot.com");

        public List<Place> places;
        public  PlacesServices(){}
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
    }
}
