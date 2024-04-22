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
    public partial class AllAds : ContentPage
    {
        public AllAds()
        {
            InitializeComponent();
            GetAllMedias();
        }
        async void GetAllMedias()
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
                    string url = "https://igs-lf8.conveyor.cloud/api/Medias";
                    HttpClient client = new HttpClient();
                    var result = await client.GetStringAsync(url);
                    var UserList = JsonConvert.DeserializeObject<List<Medias>>(result);
                    UList.ItemsSource = UserList;
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