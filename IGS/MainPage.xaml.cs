using Firebase.Storage;
using IGS.Appdata;
using IGS.MenuItems;
using IGS.Models;
using IGS.Resx;
using IGS.Views;
using IGS.Views.Dashbord;
using IGS.Views.Menu;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IGS
{
    public partial class MainPage : MasterDetailPage
    {
        public List<MasterPageItem> menuList { get; set; }
        string Lang = Thread.CurrentThread.CurrentUICulture.Name;
        async void Getindex()
        {
            string url = "https://igs-lf8.conveyor.cloud/WeatherForecast";
            HttpClient client = new HttpClient();
            HttpResponseMessage HR = await client.GetAsync(url);
            if (!HR.IsSuccessStatusCode)
            {
                await Navigation.PushModalAsync(new Serverdowen());
                return;
            }
        }
        public MainPage()
        {
            InitializeComponent();
            if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
            {
                Nav.FlowDirection = FlowDirection.RightToLeft;
            }
            var current = Connectivity.NetworkAccess;
            var profiles = Connectivity.ConnectionProfiles;
            if (!profiles.Contains(ConnectionProfile.WiFi) || current != NetworkAccess.Internet)
            {
                Navigation.PushModalAsync(new InternetConnection());
                return;
            }
            Getindex();
            menuList = new List<MasterPageItem>();

            var Db = App.Dbi().Conn;
            var UA = Db.Table<AuthUser>().Where(u => u.UserAuthentication == true).FirstOrDefault();
            if (UA == null)
            {
                menuList.Add(new MasterPageItem() { Title = AppResources.LOR, Icon = "Sign.png", TargetType = typeof(Login) });
            }
            else
            {
                menuList.Add(new MasterPageItem() { Title = AppResources.Hello +" "+ UA.UserPhone, Icon = "USH.png", TargetType = typeof(Home) });
            }

            // Adding menu items to menuList and you can define title ,page and icon
            menuList.Add(new MasterPageItem() { Title = AppResources.HomeTitle, Icon = "Home.png", TargetType = typeof(Home) });
            menuList.Add(new MasterPageItem() { Title = AppResources.BA, Icon = "Search.png", TargetType = typeof(AllAds) });

            if (UA != null)
            {
                menuList.Add(new MasterPageItem() { Title = AppResources.MA, Icon = "Ads.png", TargetType = typeof(MyAds) });
                menuList.Add(new MasterPageItem() { Title = AppResources.PA, Icon = "PlaceAdd.png", TargetType = typeof(PlaceAd) });
            }
            //About
            menuList.Add(new MasterPageItem() { Title = AppResources.more, Icon = "Info.png", TargetType = typeof(Info) });
            if (UA != null && UA.UserAdmin)
            {
                menuList.Add(new MasterPageItem() { Title = AppResources.Dashbord, Icon = "Controlpanal.png", TargetType = typeof(Dashbord) });
            }
            if (UA != null)
            {
                menuList.Add(new MasterPageItem() { Title = AppResources.LogOut, Icon = "Exit.png", TargetType = typeof(Logout) });
            }

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = menuList;
            if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
            {
                navigationDrawerList.FlowDirection = FlowDirection.RightToLeft;
            }

            // Initial navigation, this can be used for our home page
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Home))) { Title = "Home", BarBackgroundColor = Color.FromHex("#BF043055"), BarTextColor = Color.White };
        }

        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            var item = (MasterPageItem)e.SelectedItem;
            Type page = item.TargetType;



            Detail = new NavigationPage((Page)Activator.CreateInstance(page)) {Title = "Home", BarBackgroundColor = Color.FromHex("#BF043055")};
            IsPresented = false;
        }
    }
}
