using Firebase.Database;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoTour.Database
{
    public class RuleServices
    {
        FirebaseClient firebase = new FirebaseClient("https://gotour-98c79-default-rtdb.asia-southeast1.firebasedatabase.app/");
        public List<Rule> Rule { get; set; }
       
        public RuleServices()
        {


        }
        public async Task<Rule> GetRule()
        {
            Rule = (await firebase
             .Child("Rule")
             .OnceAsync<Rule>()).Select(item => new Rule
             {
                deduct = item.Object.deduct
             }).ToList();
            

            return Rule.ElementAt(0);
        }

        ////Add a noti into DB
        //public async Task SendNoti(string id, string sender, string reciever, int type, string body, string title, string tourId)
        //{
        //    await firebase
        //      .Child("Notification")
        //      .PostAsync(new Notification()
        //      {
        //          id = id,
        //          senderEmail = sender,
        //          reciever = reciever,
        //          tourId = tourId,
        //          isVisible = "True",
        //          type = type,
        //          body = body,
        //          isChecked = "False",
        //          title = title,
        //          when = DateTime.Now,
        //      });
        //}
    }
}
