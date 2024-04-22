using IGS.Models;
using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Location = Xamarin.Essentials.Location;

namespace IGS.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Browse : ContentPage
    {
        int x;
        public Browse(Medias Med)
        {
            InitializeComponent();
            x = Med.Id;
            GetMedinfo();
        }
        async void GetMedinfo()
        {
            try
            {
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                string url = "https://igs-lf8.conveyor.cloud/api/Medias/"+x;
                HttpClient client = new HttpClient();
                HttpResponseMessage HR = await client.GetAsync(url);
                string result = await HR.Content.ReadAsStringAsync();
                var Med = JsonConvert.DeserializeObject<Medias>(result);           
                if (HR.StatusCode.ToString() == "OK")
                {
                    Medias MC = new Medias();
                    MC.Id = Med.Id;
                    MC.MedName = Med.MedName;
                    MC.AdvertiserName = Med.AdvertiserName;
                    MC.MedDescription = Med.MedDescription;
                    MC.MedConteact = Med.MedConteact;
                    MC.MedLcoationname = Med.MedLcoationname;
                    MC.MedLcoationlat = Med.MedLcoationlat;
                    MC.MedLcoationlon = Med.MedLcoationlon;
                    if (MC.MedLcoationlon == 0 && MC.MedLcoationlat == 0)
                    {
                        MC.MedStuation = Med.MedLcoationname;
                    }
                    else
                    {
                        MC.MedStuation = Med.MedStuation;
                    }
                    MC.MedPrice = Med.MedPrice;
                    BindingContext = MC;
                    if (Med.MedImageApath != null)
                    {
                        var IN = new List<string> {Med.MedImageApath};
                        MCV.ItemsSource = IN;
                    }
                    if (Med.MedImageBpath != null)
                    {
                        var IN = new List<string> { 
                            Med.MedImageApath, 
                            Med.MedImageBpath };
                        MCV.ItemsSource = IN;
                    }
                    if (Med.MedImageCpath != null)
                    {
                        var IN = new List<string> {
                            Med.MedImageApath,
                            Med.MedImageBpath,
                            Med.MedImageCpath };
                        MCV.ItemsSource = IN;
                    }
                    if (Med.MedImageDpath != null)
                    {
                        var IN = new List<string> {
                            Med.MedImageApath,
                            Med.MedImageBpath,
                            Med.MedImageCpath,
                            Med.MedImageDpath };
                        MCV.ItemsSource = IN;
                    }
                    if (Med.MedImageEpath != null)
                    {
                        var IN = new List<string> {
                            Med.MedImageApath,
                            Med.MedImageBpath,
                            Med.MedImageCpath,
                            Med.MedImageDpath,
                            Med.MedImageEpath};
                        MCV.ItemsSource = IN;
                    }
                    if (Med.MedImageFpath != null)
                    {
                        var IN = new List<string> {
                            Med.MedImageApath,
                            Med.MedImageBpath,
                            Med.MedImageCpath,
                            Med.MedImageDpath,
                            Med.MedImageEpath,
                            Med.MedImageFpath};
                        MCV.ItemsSource = IN;
                    }
                    PU.IsVisible = false;
                    AI.IsRunning = false;
                    AI.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                PU.IsVisible = false;
                AI.IsRunning = false;
                AI.IsVisible = false;
                await DisplayAlert("Message", "Server Is Dowen Now Please Try Again Lettar ...", "ok");
                return;
            }
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (Lat.Text == "0" && Lon.Text == "0")
            {
                return;
            }
            else
            {
                var MyPosition = new Position();
                MyPosition.Latitude = Convert.ToDouble(Lat.Text);
                MyPosition.Longitude = Convert.ToDouble(Lon.Text);
                double MLT = Convert.ToDouble(MyPosition.Latitude.ToString().Replace(",", "."));
                double MLG = Convert.ToDouble(MyPosition.Longitude.ToString().Replace(",", "."));
                var location = new Location(MLT, MLG);
                var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
                await Map.OpenAsync(location, options);
            }
        }
        private void BC_Clicked(object sender, EventArgs e)
        {
            if (Oper.IsVisible == false)
            {
                Oper.IsVisible = true;
            }
            else
            {
                Oper.IsVisible = false;
            }
        }
        private void CallPhone_Clicked(object sender, EventArgs e)
        {
            PhoneDialer.Open(Phone.Text);
        }
    }
}