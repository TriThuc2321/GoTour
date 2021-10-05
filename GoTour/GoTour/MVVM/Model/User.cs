using System;
using System.Collections.Generic;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public string contact { get; set; }
        public string birthday { get; set; }
        public string cmnd { get; set; }
        public string profilePic { get; set; }
        public User() { }
        public User(string id, string name, string contact, string birthday, string cmnd, string profilePic)
        {
            this.id = id;
            this.name = name;
            this.contact = contact;
            this.birthday = birthday;
            this.cmnd = cmnd;
            this.profilePic = profilePic;
        }
    }
}
