using IGS.Appdata;
using IGS.Models;
using IGS.Services;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGS.Views.Dashbord
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Itemupdate : ContentPage
    {
        public Itemupdate(CategoryDetailes CatDet)
        {
            InitializeComponent();
            var x = Convert.ToString(CatDet.Id);
            if (x != null)
            {
                LId.Text = Convert.ToString(CatDet.Id);
                CBText.Text = CatDet.DetailesName;
                BtnUpdate.IsVisible = true;
            }
            else
            {
                BtnUpdate.IsVisible = false;
            }
            GetCategoriesInfo();
        }

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
                var UserList = JsonConvert.DeserializeObject<List<Categories>>(result);
                picker.ItemsSource = UserList;
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
        private async void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CBText.Text))
            {
                await DisplayAlert("Input Error", "Item is Required", "OK");
                return;
            }
            try
            {
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                BtnUpdate.IsEnabled = false;
                CategoryDetailes CD = new CategoryDetailes();
                CD.Id = Convert.ToInt32(LId.Text);
                CD.DetailesName = CBText.Text;
                CD.CategoryName = picker.SelectedItem.ToString();
                var id = CD.Id;
                string url = "https://igs-lf8.conveyor.cloud/api/CategoryDetailes/" + id;
                HttpClient client = new HttpClient();
                string jsonData = JsonConvert.SerializeObject(CD);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(url, content);
                //string result = await response.Content.ReadAsStringAsync();
                //Response responseData = JsonConvert.DeserializeObject<Response>(result);
                if (response.StatusCode.ToString() == "Created")
                {
                    PU.IsVisible = false;
                    AI.IsRunning = false;
                    AI.IsVisible = false;
                    await DisplayAlert("Success", "The Item Updated Successfully ...", "Ok");
                    await Navigation.PushAsync(new Itemsview());
                }
                else
                {
                    PU.IsVisible = false;
                    AI.IsRunning = false;
                    AI.IsVisible = false;
                    await DisplayAlert("Error", "Wrong Phone Or Password ...", "Ok");
                    return;
                }
            }
            catch (Exception ex)
            {
                PU.IsVisible = false;
                AI.IsRunning = false;
                AI.IsVisible = false;
                await DisplayAlert("Message", "Server Is Dowen Now Please Try Again Lettar ...", "ok");
                BtnUpdate.IsEnabled = true;
                return;
            }
        }
    }
}