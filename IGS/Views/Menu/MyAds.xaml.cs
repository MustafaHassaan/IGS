using IGS.Appdata;
using IGS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGS.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyAds : ContentPage
    {
        public MyAds()
        {
            InitializeComponent();
            GetAddsInfo();
        }
        async void GetAddsInfo()
        {
            var current = Connectivity.NetworkAccess;
            var profiles = Connectivity.ConnectionProfiles;
            if (!profiles.Contains(ConnectionProfile.WiFi) || current != NetworkAccess.Internet)
            {
                await Navigation.PushAsync(new InternetConnection());
            }
            else
            {
                try
                {
                    PU.IsVisible = true;
                    AI.IsRunning = true;
                    AI.IsVisible = true;
                    var UP = "";
                    var Db = App.Dbi().Conn;
                    var UA = Db.Table<AuthUser>().Where(u => u.UserAuthentication == true).FirstOrDefault();
                    if (UA != null)
                    {
                        UP = UA.UserPhone;
                    }
                    Medias Med = new Medias();
                    Med.UserPhone = UP;
                    HttpClient client = new HttpClient();
                    string url = "https://igs-lf8.conveyor.cloud/api/Medias/UserMedia";
                    string jsonData = JsonConvert.SerializeObject(Med);
                    StringContent contentc = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, contentc);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode.ToString() == "OK" )
                    {
                        var UserList = JsonConvert.DeserializeObject<List<Medias>>(result);
                        UList.ItemsSource = UserList;
                    }
                    PU.IsVisible = false;
                    AI.IsRunning = false;
                    AI.IsVisible = false;
                }
                catch (Exception ex)
                {
                    PU.IsVisible = false;
                    AI.IsRunning = false;
                    AI.IsVisible = false;
                    await Navigation.PushAsync(new Serverdowen());
                }
            }
        }
        private async void UList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Medias Med = e.Item as Medias;
            await Navigation.PushAsync(new Browse(Med));
        }
    }
}