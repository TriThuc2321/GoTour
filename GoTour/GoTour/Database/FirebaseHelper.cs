﻿using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using System.Linq;
using System.IO;
using Firebase.Storage;

namespace GoTour.Database
{
    class FirebaseHelper
    {
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseStorage storage = new FirebaseStorage("gs://gotour-98c79.appspot.com");

        public FirebaseHelper()
        {

        }
        public async Task<List<Place>> GetAllPlaces()
        {
            return (await firebase
              .Child("Places")
              .OnceAsync<Place>()).Select(item => new Place
              {
                  id = item.Object.id,
                  name = item.Object.name,
                  description = item.Object.description,
                  imgSource = item.Object.imgSource
              }).ToList();
        }
        public async Task AddPlace(string id, string name, string description, List<string> imgSource)
        {
            await firebase
              .Child("Places")
              .PostAsync(new Place()
              {
                  id = id,
                  name = name,
                  description = description,
                  imgSource = imgSource
              }); 
        }

        async public Task<string> saveImage(Stream imgStream)
        {
            var stroageImage = await new FirebaseStorage("gotour-98c79.appspot.com")
                .Child("ProfilePic")
                .Child("test.png")
                .PutAsync(imgStream);
            var imgurl = stroageImage;
            return imgurl;
        }
    }
}
