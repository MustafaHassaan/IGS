using IGS.Models;
using IGS.Resx;
using IGS.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGS.Views.Dashbord
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Usersview : ContentPage
    {
        public Usersview()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            GetUsersInfo();
        }
        async void GetUsersInfo()
        {
            try
            {
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;


                string url = "https://igs-lf8.conveyor.cloud/api/Users";
                HttpClient client = new HttpClient();
                var result = await client.GetStringAsync(url);

                var UserList = JsonConvert.DeserializeObject<List<Users>>(result);

                UList.ItemsSource = null;
                PU.IsVisible = false;
                AI.IsRunning = false;
                AI.IsVisible = false;
                UList.ItemsSource = new ObservableCollection<Users>(UserList);
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
        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                var menu = sender as MenuItem;
                var id = menu.CommandParameter;
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                string url = "https://igs-lf8.conveyor.cloud/api/users/" + id;
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync(url);
                string result = await response.Content.ReadAsStringAsync();
                Response responseData = JsonConvert.DeserializeObject<Response>(result);
                if (response.StatusCode.ToString() == "OK")
                {
                    PU.IsVisible = false;
                    AI.IsRunning = false;
                    AI.IsVisible = false;
                    await DisplayAlert("Success", "The Phone Is Updated Successfully ...", "Ok");
                    GetUsersInfo();
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
                await DisplayAlert("Input Error", ex.Message, "OK");
                return;
            }
        }
        private async void UList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                Users user = e.Item as Users;
                await Navigation.PushAsync(new Usersoprations(user));
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
        private void UList_Refreshing(object sender, EventArgs e)
        {
            GetUsersInfo();
            UList.IsRefreshing = false;
        }
    }
}