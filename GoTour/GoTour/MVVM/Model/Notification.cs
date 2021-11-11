using System;
using System.Collections.Generic;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class Notification
    {
        public string id { get; set; }
        public string senderEmail { get; set; }
        public bool isChecked { get; set; }
        public List<string> recieverListEmail { get; set; }
        public int type { get; set; } //1: System notification    2: TourGuider Notification
        public string body { get; set; }
        public DateTime when { get; set; }
        public Notification() { }
        public Notification(string id, string sender, List<string> recievers, bool isChecked, int type, string body, DateTime time)
        {
            this.id = id;
            this.senderEmail = sender;
            this.recieverListEmail = recievers;
            this.isChecked = isChecked;
            this.type = type;
            this.body = body;
            this.when = time;
        }
    }
}
