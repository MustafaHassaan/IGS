using IGS.Models;
using IGS.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class Usersoprations : ContentPage
    {
        string Lang = Thread.CurrentThread.CurrentUICulture.Name;

        public Usersoprations(Users user)
        {
            InitializeComponent();
            var x = Convert.ToString(user.Id);
            if (x != null &&
                user.UserPhone != null)
            {
                txtId.Text = Convert.ToString(user.Id);
                Phone.Text = user.UserPhone;
                Password.Text = user.UserPassword;
                if (user.UserReadPrivacy)
                {
                    Privacy.IsChecked = true;
                }
                if (user.IsAdmin)
                {
                    Admin.IsChecked = true;
                }
                //Position.Text = Convert.ToString(user.PosId);
                BtnUpdate.IsVisible = true;
            }
            else
            {
                BtnUpdate.IsVisible = false;
            }
        }
        private async void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Phone.Text))
            {
                if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                {
                    await DisplayAlert("خطأ", "الهاتف مطلوب", "حسناً");
                    return;
                }
                await DisplayAlert("Input Error", "Phone is Required", "OK");
                return;
            }
            try
            {
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                BtnUpdate.IsEnabled = false;
                Users user = new Users();
                user.Id = Convert.ToInt32(txtId.Text);
                user.UserPhone = Phone.Text;
                if (Privacy.IsChecked)
                {
                    user.UserReadPrivacy = true ;
                }
                else
                {
                    user.UserReadPrivacy = false;
                }

                if (Admin.IsChecked)
                {
                    user.IsAdmin = true;
                }
                else
                {
                    user.IsAdmin = false;
                }

                user.UserPassword = Password.Text;
                var id = user.Id;
                string url = "https://igs-lf8.conveyor.cloud/api/users/" + id;
                HttpClient client = new HttpClient();
                string jsonData = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();
                Response responseData = JsonConvert.DeserializeObject<Response>(result);
                if (response.StatusCode.ToString() == "Created")
                {
                    PU.IsVisible = false;
                    AI.IsRunning = false;
                    AI.IsVisible = false;
                    if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                    {
                        await DisplayAlert("نجاح", "تم تعديل الهاتف بنجاح", "حسناً");
                    }
                    await DisplayAlert("Success", "The Phone Updated Successfully ...", "Ok");
                    await Navigation.PushAsync(new Usersview());
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