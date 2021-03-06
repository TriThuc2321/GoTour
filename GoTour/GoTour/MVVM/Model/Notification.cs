using GoTour.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class Notification: ObservableObject
    {
        public string id { get; set; }
        public string senderEmail { get; set; }
        public string isChecked;
        public string IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");

            }
        }
        public string isVisible;
        public string IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                OnPropertyChanged("IsVisible");

            }
        }
        public string reciever { get; set; }
        public int type { get; set; } //1: System notification    2: TourGuider Notification
        public string title { get; set; }
        public string body { get; set; }
        public DateTime when { get; set; }
        public string tourId { get; set; }
        public Notification() { }
        public Notification(string id, string sender, string reciever, string isChecked, int type, string body, DateTime time, string isVisible, string tourId, string title)
        {
            this.id = id;
            this.senderEmail = sender;
            this.reciever = reciever;
            this.isChecked = isChecked;
            this.type = type;
            this.body = body;
            this.when = time;
            this.isVisible = isVisible;
            this.title = title;
            this.tourId = tourId;
        }
    }
}
