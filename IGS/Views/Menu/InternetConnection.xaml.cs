using IGS.Appdata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGS.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InternetConnection : ContentPage
    {
        public InternetConnection()
        {
            InitializeComponent();
        }
        private void TA_Clicked(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            var profiles = Connectivity.ConnectionProfiles;
            if (!profiles.Contains(ConnectionProfile.WiFi) || current != NetworkAccess.Internet)
            {
                return;
            }
            App.Current.MainPage = new IGS.MainPage();
        }
        string Lang = Thread.CurrentThread.CurrentUICulture.Name;
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                bool Res;
                if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                {
                    Res = await DisplayAlert("سؤال", "هل تريد الخروج", "نعم", "لا");
                    if (Res)
                    {
                        var closer = DependencyService.Get<IClose>();
                        closer?.CloseApp();
                    }
                }
                else
                {
                    Res = await DisplayAlert("Qustion", "Are u want Exit ?", "Yes", "No");
                    if (Res)
                    {
                        var closer = DependencyService.Get<IClose>();
                        closer?.CloseApp();
                    }
                }
            });
            return true;
        }
    }
}