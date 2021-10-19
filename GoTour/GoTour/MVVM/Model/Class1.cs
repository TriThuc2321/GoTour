using System;
using System.Collections.Generic;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class Class1
    {
        public string id { get; set; }
        public string name { get; set; }
        public string contact { get; set; }
        public string birthday { get; set; }
        public string cmnd { get; set; }
        public string profilePic { get; set; }
        public List<string> temp { get; set; }
        public Class1() { }
        public Class1(string id, string name, string contact, string birthday, string cmnd, string profilePic, List<string> temp)
        {
            this.id = id;
            this.name = name;
            this.contact = contact;
            this.birthday = birthday;
            this.cmnd = cmnd;
            this.profilePic = profilePic;
            this.temp = temp;
        }
    }
}
