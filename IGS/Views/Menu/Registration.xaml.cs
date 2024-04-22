using IGS.Models;
using IGS.Resx;
using IGS.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGS.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registration : ContentPage
    {
        public Registration()
        {
            InitializeComponent();
        }
        string Lang = Thread.CurrentThread.CurrentUICulture.Name;

        private async void Register_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Phone.Text))
            {
                if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                {
                    await DisplayAlert("خطأ", "رقم الهاتف مطلوب", "حسناً");
                    return;
                }
                else
                {
                    await DisplayAlert("Error", "Phone is Required", "OK");
                    return;
                }
            }
            if (string.IsNullOrEmpty(Password.Text))
            {
                if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                {
                    await DisplayAlert("خطأ", "كلمة المرور مطلوبه", "حسناً");
                    return;
                }
                else
                {
                    await DisplayAlert("Error", "Password is Required", "OK");
                    return;
                }
            }
            if (CB.IsChecked != true)
            {
                if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                {
                    await DisplayAlert("خطأ", "يجب قراءة الخصوصيه وهي مطلوبه", "حسناً");
                    return;
                }
                else
                {
                    await DisplayAlert("Error", "Privacy is Required", "OK");
                    return;
                }
            }
            var XCB = CB.IsChecked;
            try
            {
                if(Phone.Text.Length < 11)
                {
                    if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                    {
                        await DisplayAlert("خطأ", "يجب أن يساوي الهاتف 11 رقم", "حسناً");
                        return;
                    }
                    else
                    {
                        await DisplayAlert("Error", "Phone Must Be Equal 11 Number ...", "OK");
                        return;
                    }
                }
                if (Password.Text.Length < 8)
                {
                    if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                    {
                        await DisplayAlert("خطأ", "يجب أن تكون كلمة المرور اكبر من او تساوي 8 حرف او رقم", "حسناً");
                        return;
                    }
                    else
                    {
                        await DisplayAlert("Error", "Phone Must Be Biger Than Or Equal 8 Charcter ...", "OK");
                        return;
                    }
                }
                popupLoginView.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                Users user = new Users();
                user.UserPhone = Phone.Text;
                user.UserPassword = Password.Text;
                user.UserReadPrivacy = XCB;

                string url = "https://igs-lf8.conveyor.cloud/api/Users";

                HttpClient client = new HttpClient();
                string jsonData = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();
                Response responseData = JsonConvert.DeserializeObject<Response>(result);
                if (response.StatusCode.ToString() == "Created")
                {
                    AI.IsRunning = false;
                    AI.IsVisible = false;
                    popupLoginView.IsVisible = false;
                    if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                    {
                        await DisplayAlert("نجاح", "تم التسجيل بنجاح", "حسناً");
                    }
                    else
                    {
                        await DisplayAlert("Success", "You Are Registration Successfully ...", "Ok");
                    }
                    await Navigation.PushAsync(new Login());
                }
                else
                {
                    AI.IsRunning = false;
                    AI.IsVisible = false;
                    popupLoginView.IsVisible = false;
                    await DisplayAlert("Error", "This Phone Is Using Befor ...", "Ok");
                    return;
                }
            }
            catch (Exception ex)
            {
                AI.IsRunning = false;
                AI.IsVisible = false;
                popupLoginView.IsVisible = false;
                await Navigation.PushAsync(new Serverdowen());
            }
        }
        private async void Privacy_Clicked(object sender, EventArgs e)
        {
            var TU = AppResources.TU;
            if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
            {
                await DisplayAlert("قواعد المستخدمين هي", TU, "حسناً");
            }
            else
            {
                await DisplayAlert("Terms of use", TU, "Ok");
            }
        }
    }
}