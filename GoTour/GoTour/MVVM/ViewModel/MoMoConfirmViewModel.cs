﻿using GoTour.Core;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class MoMoConfirmViewModel:ObservableObject
    {
        INavigation navigation;
        public MoMoConfirmViewModel() { }
        public Command NavigationBack { get; }
        public Command UploadPhoto { get; }

        public Command Confirm { get; }
        public MoMoConfirmViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            NavigationBack = new Command(() => navigation.PopAsync());
            UploadPhoto = new Command(uploadPhoto);
            Confirm = new Command(confirm);


            Money = "1000000";
            Regulation = "Quy định.....................................\n.......................\n..................";
            ImageLink = "https://static.mservice.io/img/momo-upload-api-210622095003-637599522030862054.jpg";
            ImageVisible = true;
            ConfirmText = "DONE";
        }

        void uploadPhoto(object obj)
        {

        }

        void confirm(object obj)
        {
            navigation.PushAsync(new BookedTicketDetailView());
        }

        #region money
        private string _money;
        public string Money
        {
            get { return _money; }
            set
            {
                _money = value;
                OnPropertyChanged("Money");
            }
        }
        #endregion

        #region image
        private string _imageLink;
        public string ImageLink
        {
            get { return _imageLink; }
            set
            {
                _imageLink = value;
                OnPropertyChanged("ImageLink");
            }
        }

        private bool _imageVisible;
        public bool ImageVisible
        {
            get { return _imageVisible; }
            set
            {
                _imageVisible = value;
                OnPropertyChanged("ImageVisible");
            }
        }
        #endregion

        #region confirm Text
        private string _confirmText;
        public string ConfirmText
        {
            get { return _confirmText; }
            set
            {
                _confirmText = value;
                OnPropertyChanged("ConfirmText");
            }
        }
        #endregion

        #region regulation 
        private string _regulation;
        public string Regulation
        {
            get { return _regulation; }
            set
            {
                _regulation = value;
                OnPropertyChanged("Regulation");
            }
        }

        #endregion
    }
}