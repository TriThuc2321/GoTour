using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoTour.Droid;
using System.Runtime.CompilerServices;

[assembly: Xamarin.Forms.Dependency(typeof(ToastMessage))]
namespace GoTour.Droid
{
   
    public class ToastMessage: IToast
    {
        public void ShortToast(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
        public void LongToast(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }
    }
}