using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IGS.Appdata;
using IGS.Droid;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(Close))]
namespace IGS.Droid
{
    public class Close : IClose
    {
        public void CloseApp()
        {
            Activity activity = (Activity)Forms.Context;
            activity.FinishAndRemoveTask();
            Java.Lang.JavaSystem.Exit(0);
        }
    }
}