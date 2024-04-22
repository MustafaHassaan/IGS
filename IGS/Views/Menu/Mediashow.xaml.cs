using IGS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGS.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mediashow : ContentPage
    {
        string x;
        public Mediashow(Categories Cat)
        {
            InitializeComponent();
            x = Cat.CategoryName;
            GetMedinfo();
        }
        async void GetMedinfo()
        {
            Categories Cat = new Categories();
            Cat.CategoryName = x;
            try
            {
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                string url = "https://igs-lf8.conveyor.cloud/api/Medias/Categoryitems";

                HttpClient client = new HttpClient();
                string jsonData = JsonConvert.SerializeObject(Cat);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode.ToString() == "OK")
                {
                    var Meddet = JsonConvert.DeserializeObject<List<Medias>>(result);
                    UList.ItemsSource = Meddet;
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
                await DisplayAlert("Error", "The Server Is Dowen Now Please Try Again Lettar ...", "Ok");
                return;
            }
        }
        private async void UList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Medias Med = e.Item as Medias;
            await Navigation.PushAsync(new Browse(Med));
        }
    }
}