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
        public ObservableCollection<Notification> ListMyNoti_TourGuider { get; set; }
        public ObservableCollection<Notification> ListMyNoti_System { get; set; }

        public NotificationServices() {
          

        }
        public async Task<List<Notification>> GetAllNotification()
        {
            ListAllNoti = (await firebase
              .Child("Notification")
              .OnceAsync<Notification>()).Select(item => new Notification
              {
                  id = item.Object.id,
                  senderEmail = item.Object.senderEmail,
                  recieverListEmail = item.Object.recieverListEmail,
                  type = item.Object.type,
                  title = item.Object.title,
                  isChecked = item.Object.isChecked,
                  body = item.Object.body,
                  when = item.Object.when,
              }).ToList();

            return ListAllNoti;
        }

        //Add a noti into DB
        public async Task SendNoti(string id, string sender, List<string> recievers, int type, string body,string title)
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
                  title = title,
                  when = DateTime.Now,
              });
        }

        public async Task UpdatePlace(Notification notification)
        {
            var toUpdateNotification = (await firebase
              .Child("Notification")
              .OnceAsync<Place>()).Where(a => a.Object.id == notification.id).FirstOrDefault();

            await firebase
              .Child("Notification")
              .Child(toUpdateNotification.Key)
              .PutAsync(new Notification
              {
                  id = notification.id,
                  senderEmail = notification.senderEmail,
                  isChecked = notification.isChecked,
                  recieverListEmail = notification.recieverListEmail,
                  type = notification.type,
                  body = notification.body,
                  when = notification.when,
                  title = notification.title,
              });
        }

        //Get MY SYSTEM NOTIFICATION
        public ObservableCollection<Notification> GetMySystemNoti(string yourEmail)
        {
            ListMyNoti_System = new ObservableCollection<Notification>();
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
                ListMyNoti_System.Add(ite3);
            }
            return ListMyNoti_System;
        }

        //GET MY GUIDER NOTIFICATION
        public ObservableCollection<Notification> GetMyGuiderNoti(string yourEmail)
        {
            ListMyNoti_TourGuider = new ObservableCollection<Notification>();
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
                ListMyNoti_TourGuider.Add(ite3);
            }
            return ListMyNoti_TourGuider;
        }
    }
}
