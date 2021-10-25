using System;
using System.Collections.Generic;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class User
    {
        public string email { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string contact { get; set; }
        public string birthday { get; set; }
        public string cmnd { get; set; }
        public string profilePic { get; set; }
        public string address { get; set; }
        public int score { get; set; }
        public int rank { get; set; }
        public User() { }

        public User(string email, string password, string name, string contact, string birthday, string cmnd, string profilePic, string address, int score, int rank)
        {
            this.email = email;
            this.password = password;
            this.name = name;
            this.contact = contact;
            this.birthday = birthday;
            this.cmnd = cmnd;
            this.profilePic = profilePic;
            this.address = address;
            this.score = score;
            this.rank = rank;
        }
    }
}
