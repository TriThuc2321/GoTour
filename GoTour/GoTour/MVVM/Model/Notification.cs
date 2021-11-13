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
        public string title { get; set; }
        public DateTime when { get; set; }
        public Notification() { }

        public Notification(string id, string senderEmail, bool isChecked, List<string> recieverListEmail, int type, string body, string title, DateTime when)
        {
            this.id = id;
            this.senderEmail = senderEmail;
            this.isChecked = isChecked;
            this.recieverListEmail = recieverListEmail;
            this.type = type;
            this.body = body;
            this.title = title;
            this.when = when;
        }
    }
}
