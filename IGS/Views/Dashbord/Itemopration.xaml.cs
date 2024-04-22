using IGS.Models;
using IGS.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGS.Views.Dashbord
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Itemopration : ContentPage
    {
        public Itemopration()
        {
            InitializeComponent();
            GetdetiatilesInfo();
        }
        string Lang = Thread.CurrentThread.CurrentUICulture.Name;
        async void GetdetiatilesInfo()
        {
            try
            {
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                string url = "https://igs-lf8.conveyor.cloud/api/CategoryDetailes";
                HttpClient client = new HttpClient();
                var result = await client.GetStringAsync(url);

                var UserList = JsonConvert.DeserializeObject<List<CategoryDetailes>>(result);
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
                await DisplayAlert("Error", "The Server Is Dowen Now Please Try Again Lettar ...", "Ok");
                return;
            }
        }
        private void UList_Refreshing(object sender, EventArgs e)
        {
            GetdetiatilesInfo();
            UList.IsRefreshing = false;
        }
        private async void UList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                CategoryDetailes CatDet = e.Item as CategoryDetailes;
                await Navigation.PushAsync(new Itemupdate(CatDet));
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
        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                var menu = sender as MenuItem;
                var id = menu.CommandParameter;
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                string url = "https://igs-lf8.conveyor.cloud/api/CategoryDetailes/" + id;
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync(url);
                if (response.StatusCode.ToString() == "OK")
                {
                    PU.IsVisible = false;
                    AI.IsRunning = false;
                    AI.IsVisible = false;
                    if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                    {
                        await DisplayAlert("نجاح", "تم حذف الصنف بنجاح", "حسناً");
                    }
                    else
                    {
                        await DisplayAlert("Success", "The Item Is Deleted Successfully ...", "Ok");
                    }
                    GetdetiatilesInfo();
                    UList.IsRefreshing = false;
                }
                else
                {
                    PU.IsVisible = false;
                    AI.IsRunning = false;
                    AI.IsVisible = false;
                    await DisplayAlert("Error", "Wrong Phone  ...", "Ok");
                    return;
                }
            }
            catch (Exception ex)
            {
                PU.IsVisible = false;
                AI.IsRunning = false;
                AI.IsVisible = false;
                await DisplayAlert("Input Error", ex.Message, "OK");
                return;
            }
        }
    }
}