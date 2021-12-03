using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoTour.Database
{
    public class UsersServices
    {
         FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseStorage storage = new FirebaseStorage("gotour-98c79.appspot.com");
        List<User> listUser = new List<User>();
        public UsersServices()
        {
        }
        async public Task<string> saveImage(Stream imgStream, string emailUser, string userName)
        {
           // string a = "clmm";
            var stroageImage = await new FirebaseStorage("gotour-98c79.appspot.com")
                .Child("ProfilePic").Child(emailUser)
                .Child(userName + ".png")
                .PutAsync(imgStream);
            var imgurl = stroageImage;
            return imgurl;
        }

        async public Task addUser(User user)
        {
            await firebase
              .Child("Users")
              .PostAsync(new User()
              {
                  email = user.email,
                  password = user.password,
                  name = user.name,
                  contact = user.contact,
                  birthday = user.birthday,
                  cmnd = user.cmnd,
                  profilePic = user.profilePic,
                  address = user.address,
                  score = user.score,
                  rank = user.rank
              }) ;
        }
        public async Task<List<User>> GetAllUsers()
        {
            return (await firebase
              .Child("Users")
              .OnceAsync<User>()).Select(item => new User
              {
                  email = item.Object.email,
                  password = item.Object.password,
                  name = item.Object.name,
                  contact = item.Object.contact,
                  birthday = item.Object.birthday,
                  cmnd = item.Object.cmnd,
                  profilePic = item.Object.profilePic,
                  address = item.Object.address,
                  score = item.Object.score,
                  rank = item.Object.rank
              }).ToList();
        }
        public async Task UpdateUser(User user)
        {
            var toUpdateUser = (await firebase
              .Child("Users")
              .OnceAsync<User>()).Where(a => a.Object.email == user.email).FirstOrDefault();

            await firebase
              .Child("Users")
              .Child(toUpdateUser.Key)
              .PutAsync(new User
              {
                  email = user.email,
                  password = user.password,
                  name = user.name,
                  contact = user.contact,
                  birthday = user.birthday,
                  cmnd = user.cmnd,
                  profilePic = user.profilePic,
                  address = user.address,
                  score = user.score,
                  rank = user.rank
              });
        }

        public User getUserByEmail(string mail, List<User> listUsers)
        {
            for (int i = 0; i < listUsers.Count(); i++)
            {
                if (listUsers[i].email == mail) return listUsers[i];
            }
            return null;
        }
        public bool ExistEmail(string email, List<User> listUsers)
        {
            for (int i = 0; i < listUsers.Count; i++)
            {
                if (email == listUsers[i].email) return true;
            }
            return false;
        }
        public string Encode(string stringValue)
        {
            MD5 mh = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(stringValue);
            byte[] hash = mh.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public bool IsPhoneNumber(string number)
        {
            if (number.Length > 11 || number.Length < 10) return false;
            for (int i = 0; i < number.Length; i++)
            {
                if (number[i] < 48 || number[i] > 57) return false;
            }
            return true;
        }
        public bool IsCMND(string cmnd)
        {
            if (cmnd.Length != 9 || cmnd.Length != 12) return false;
            for (int i = 0; i < cmnd.Length; i++)
            {
                if (cmnd[i] < 48 || cmnd[i] > 57) return false;
            }
            return true;
        }
        public bool checkEmail(string inputEmail)
        {
            if (inputEmail == null) return false;
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            return re.IsMatch(inputEmail);
        }
        public static string GenerateRandomString(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }
    }
}
