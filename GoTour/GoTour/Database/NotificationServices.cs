using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using GoTour.MVVM.Model;

namespace GoTour.Database
{
    public class NotificationServices
    {
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");
        public List<Notification> ListAllNoti { get; set; }
        public List<Notification> ListMyNoti { get; set; }
        public NotificationServices() {
            GetAllNotification();

        }
        public async Task<List<Notification>> GetAllNotification()
        {
            ListAllNoti = new List<Notification>();
            return (await firebase
              .Child("Notification")
              .OnceAsync<Notification>()).Select(item => new Notification
              {
                  id = item.Object.id,
                  senderEmail = item.Object.senderEmail,
                  recieverListEmail = item.Object.recieverListEmail,
                  type = item.Object.type,
                  isChecked = item.Object.isChecked,
                  body = item.Object.body,
                  when = item.Object.when,
              }).ToList();
        }

        //Add a noti into DB
        public async Task SendNoti(string id, string sender, List<string> recievers, int type, string body)
        {
            await firebase
              .Child("Notification")
              .PostAsync(new Notification()
              {
                  id = id,
                  senderEmail = sender,
                  recieverListEmail = recievers,
                  type = type,
                  body = body,
                  isChecked = false,
                  when = DateTime.Now,
              });
        }

        //Get MY SYSTEM NOTIFICATION
        public async Task GetMySystemNoti(string yourEmail)
        {
            ListMyNoti = new List<Notification>();
            List<Notification> temp = new List<Notification>();
            foreach(Notification x in ListAllNoti)
            {
                if(x.type == 1)
                {
                    temp.Add(x);

                }
            }
            List<Notification> temp2 = new List<Notification>();
            List<Notification> result = new List<Notification>();
            temp2 = temp.FindAll(e => e.recieverListEmail.Exists(p => p.Equals(yourEmail)));
            foreach (var ntf in temp2)
            {
                if (!result.Contains(ntf))
                    result.Add(ntf);
            }
            foreach (Notification ite3 in result)
            {
                ListMyNoti.Add(ite3);
            }
        }

        //GET MY GUIDER NOTIFICATION
        public async Task GetMyGuiderNoti(string yourEmail)
        {
            ListMyNoti = new ObservableCollection<Notification>();
            List<Notification> temp = new List<Notification>();
            foreach (Notification x in ListAllNoti)
            {
                if (x.type == 2)
                {
                    temp.Add(x);

                }
            }
            List<Notification> temp2 = new List<Notification>();
            List<Notification> result = new List<Notification>();
            temp2 = temp.FindAll(e => e.recieverListEmail.Exists(p => p.Equals(yourEmail)));
            foreach (var ntf in temp2)
            {
                if (!result.Contains(ntf))
                    result.Add(ntf);
            }
            foreach (Notification ite3 in result)
            {
                ListMyNoti.Add(ite3);
            }
        }
    }
}
