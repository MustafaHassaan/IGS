using IGS.Appdata;
using IGS.Views;
using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace IGS
{
    public partial class App : Application
    {
        private static IGSDb _Dbi;
        public static IGSDb Dbi()
        {
            if (_Dbi == null)
                _Dbi = new IGSDb();
            return _Dbi;
        }
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
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
