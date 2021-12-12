﻿using GoTour.MVVM.View;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoTour
{
    public partial class App : Application
    {
        public App()
        {
            DependencyService.Register<IMessageService, MessageService>();

            InitializeComponent();
            MainPage = new SendReviewView();
          //  MainPage = new MainPage();
          //  MainPage = new SuccessBookView();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
