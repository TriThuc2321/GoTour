using GoTour.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoTour.MVVM.Model
{
    public class User: ObservableObject
    {
        private string _email;
        public string email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged("email");
            }
        }

        private string _password;
        public string password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("password");
            }
        }
        private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }

        private string _contact;
        public string contact
        {
            get { return _contact; }
            set
            {
                _contact = value;
                OnPropertyChanged("contact");
            }
        }

        private string _birthday;
        public string birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                OnPropertyChanged("birthday");
            }
        }
    

        private string _cmnd;
        public string cmnd
        {
            get { return _cmnd; }
            set
            {
                _cmnd = value;
                OnPropertyChanged("cmnd");
            }
        }

        private string _profilePic;
        public string profilePic
        {
            get { return _profilePic; }
            set
            {
                _profilePic = value;
                OnPropertyChanged("profilePic");
            }
        }

        private string _address;
        public string address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged("address");
            }
        }

        private int _score;
        public int score
        {
            get { return _score; }
            set
            {
                _score = value;
                OnPropertyChanged("score");
            }
        }

        private int _rank;
        public int rank
        {
            get { return _rank; }
            set
            {
                _rank = value;
                OnPropertyChanged("rank");
            }
        }
        private bool _isEnable;
        public bool isEnable
        {
            get { return _isEnable; }
            set
            {
                _isEnable = value;
                OnPropertyChanged("isEnable");
            }
        }
        public User() { }

        public User(string email, string password, string name, string contact, string birthday, string cmnd, string profilePic, string address, int score, int rank, bool isEnable)
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
            this.isEnable = isEnable;
        }
    }
}
