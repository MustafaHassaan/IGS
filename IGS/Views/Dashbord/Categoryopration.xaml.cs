using Firebase.Storage;
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
using static Xamarin.Essentials.Permissions;

namespace IGS.Views.Dashbord
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Categoryopration : ContentPage
    {
        public Categoryopration()
        {
            InitializeComponent();
            GetCategoriesInfo();
        }
        string Lang = Thread.CurrentThread.CurrentUICulture.Name;
        async void GetCategoriesInfo()
        {
            try
            {
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                string url = "https://igs-lf8.conveyor.cloud/api/Categories";
                HttpClient client = new HttpClient();
                var result = await client.GetStringAsync(url);
                List<Categories> Catlist = new List<Categories>();
                var UserList = JsonConvert.DeserializeObject<List<Categories>>(result);
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
            GetCategoriesInfo();
            UList.IsRefreshing = false;
        }
        private async void UList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                Categories Cat = e.Item as Categories;
                await Navigation.PushAsync(new Categoryupdate(Cat));
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
                string url = "https://igs-lf8.conveyor.cloud/api/Categories/" + id;
                HttpClient client = new HttpClient();
                HttpResponseMessage img = await client.GetAsync(url);
                string result = await img.Content.ReadAsStringAsync();
                var Res = JsonConvert.DeserializeObject<Categories>(result);
                string fileName = Res.CategoryImageName;
                FirebaseStorage FBS = new FirebaseStorage("moustafa-igs.appspot.com");
                await FBS.Child("Categories").Child(fileName).DeleteAsync();
                if (img.StatusCode.ToString() == "OK")
                {
                    HttpResponseMessage response = await client.DeleteAsync(url);
                    Response responseData = JsonConvert.DeserializeObject<Response>(result);
                    PU.IsVisible = false;
                    AI.IsRunning = false;
                    AI.IsVisible = false;
                    if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                    {
                        await DisplayAlert("نجاح", "تم حذف الفئه بنجاح", "حسناً");
                    }
                    else
                    {
                        await DisplayAlert("Success", "The Category Is Deleted Successfully ...", "Ok");
                    }
                    GetCategoriesInfo();
                    UList.IsRefreshing = false;
                }
                else
                {
                    PU.IsVisible = false;
                    AI.IsRunning = false;
                    AI.IsVisible = false;
                    if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                    {
                        await DisplayAlert("خطأ", "رقم هاتف خطأ", "حسناً");
                        return;
                    }
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