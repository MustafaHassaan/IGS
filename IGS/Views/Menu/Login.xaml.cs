using IGS.Appdata;
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

namespace IGS.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }
        string Lang = Thread.CurrentThread.CurrentUICulture.Name;
        private async void Signin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registration());
        }
        private async void Sign_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Phone.Text))
            {
                if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                {
                    await DisplayAlert("خطأ", "الهاتف مطلوب", "حسناً");
                    return;
                }
                await DisplayAlert("Error", "Phone is Required", "OK");
                return;
            }
            if (string.IsNullOrEmpty(Password.Text))
            {
                if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                {
                    await DisplayAlert("خطأ", "كلمة المرور مطلوبه", "حسناً");
                    return;
                }
                await DisplayAlert("Error", "Password is Required", "OK");
                return;
            }
            try
            {
                if (Phone.Text.Length < 11)
                {
                    if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                    {
                        await DisplayAlert("خطأ", "يجب ان يساوي رقم الهاتف 11 رقم", "حسناً");
                        return;
                    }
                    await DisplayAlert("Error", "Phone Must Be Equal 11 Number ...", "OK");
                    return;
                }
                if (Password.Text.Length < 8)
                {
                    if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                    {
                        await DisplayAlert("خطأ", "كلمة المرور يجب ان تكون مكونه من 8 (احرف/ارقام) او كبر", "حسناً");
                        return;
                    }
                    await DisplayAlert("Error", "Password Must Be Biger Than Or Equal 8 Charcter ...", "OK");
                    return;
                }
                popupLoginView.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                Users user = new Users();
                user.UserPhone = Phone.Text;
                user.UserPassword = Password.Text;

                string url = "https://igs-lf8.conveyor.cloud/api/Users/Users";

                HttpClient client = new HttpClient();
                string jsonData = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();
                
                if (response.StatusCode.ToString() == "OK")
                {
                    Response responseData = JsonConvert.DeserializeObject<Response>(result);
                    var userid = responseData.Id;
                    var userphone = responseData.UserPhone;
                    var userpassword = responseData.UserPassword;
                    var useradmin = responseData.ISAdmin;
                    var Db = App.Dbi().Conn;
                    var UA = Db.Table<AuthUser>().Where(u => u.Id == userid && u.UserPhone == userphone && u.UserPassword == userpassword).FirstOrDefault();
                    if (UA == null)
                    {
                        AuthUser AU = new AuthUser();
                        AU.Id = userid;
                        AU.UserPhone = userphone;
                        AU.UserPassword = userpassword;
                        AU.UserAuthentication = true;
                        AU.UserAdmin = useradmin;
                        Db.Insert(AU);
                        AI.IsRunning = false;
                        AI.IsVisible = false;
                        popupLoginView.IsVisible = false;
                        if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                        {
                            await DisplayAlert("نجاح", "تم تسجيل الدخول بنجاح", "حسناً");
                        }
                        else
                        {
                            await DisplayAlert("Success", "You Are Login Successfully ...", "Ok");
                        }
                        App.Current.MainPage = new IGS.MainPage();
                    }
                }
                else
                {
                    AI.IsRunning = false;
                    AI.IsVisible = false;
                    popupLoginView.IsVisible = false;
                    if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                    {
                        await DisplayAlert("خطأ", "خطأ في تسجيل الدخول حاول مره اخرى", "حسناً");
                    }
                    else
                    {
                        await DisplayAlert("Error", "Wrong Phone Or Password ...", "Ok");
                    }
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
        private void FMP_Clicked(object sender, EventArgs e)
        {
            var Res = "";
            var RA = false;
            async void Cond()
            {
                RA = false;
                if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                {
                    Res = await DisplayPromptAsync("ما هو رقم تليفونك ?", "الغاء", "حسنا", maxLength: 11, keyboard: Keyboard.Numeric);
                    Changepassword(Res);
                }
                else
                {
                    Res = await DisplayPromptAsync("What Is Your Phone Number ?", "Cancel", "Ok", maxLength: 11, keyboard: Keyboard.Numeric);
                    Changepassword(Res);
                }
                var x = Res;
                if (x == "" || x == null)
                {
                    return;
                }
                if (x != "" && x.Length < 11)
                {
                    if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                    {
                        RA = await DisplayAlert("خطأ !", "رقم خاطئ حاول مره اخرى", "حسنا", "الغاء");
                    }
                    RA = await DisplayAlert("Error !", "Wrong Number Please Try Again", "Ok", "Cancel");
                    while (RA)
                    {
                        Cond();
                    }
                    return;
                }
            }
            Cond();
        }
        async void Changepassword(string Phone)
        {
            try
            {
                popupLoginView.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                Users user = new Users();
                user.UserPhone = Phone;
                string url = "https://igs-lf8.conveyor.cloud/api/Users/ForgetPassword";

                HttpClient client = new HttpClient();
                string jsonData = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode.ToString() == "OK")
                {
                    popupLoginView.IsVisible = false;
                    AI.IsRunning = false;
                    AI.IsVisible = false;
                    var Res = "";
                    Response responseData = JsonConvert.DeserializeObject<Response>(result);
                    if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                    {
                        Res = await DisplayPromptAsync("تغيير الرقم السري", null, "حسنا", maxLength: 64, keyboard: Keyboard.Default);
                        if (Res.Length < 8)
                        {
                            if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                            {
                                await DisplayAlert("خطأ","لا يمكن ان تكون كلمة المرور اقل من 8 حروف", "حسنا");
                                return;
                            }
                            else
                            {
                                await DisplayAlert("Error","Cant Password To Be Smaller Than 8 Charcter", "Ok");
                                return;
                            }
                        }
                        else
                        {
                            var userid = responseData.Id;
                            var userphone = responseData.UserPhone;
                            var userisadmin = responseData.ISAdmin;
                            var PW = Res;
                            Change(userid, userphone, PW, userisadmin);
                        }
                    }
                    else
                    {
                        Res = await DisplayPromptAsync("Change Password", null, "Ok", maxLength: 64, keyboard: Keyboard.Default);
                        if (Res.Length < 8)
                        {
                            string coun = "";
                            if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                            {
                                coun = await DisplayPromptAsync("لا يمكن ان تكون كلمة المرور اقل من 8 حروف", "الغاء", "حسنا", maxLength: 11, keyboard: Keyboard.Numeric);
                                return;
                            }
                            else
                            {
                                Res = await DisplayPromptAsync("Cant Password To Be Smaller Than 8 Charcter", "Cancel", "Ok", maxLength: 11, keyboard: Keyboard.Numeric);
                                return;
                            }
                        }
                        else
                        {
                            var userid = responseData.Id;
                            var userphone = responseData.UserPhone;
                            var userisadmin = responseData.ISAdmin;
                            var PW = Res;
                            Change(userid, userphone, PW, userisadmin);
                        }
                    }
                }
            }
            catch (Exception)
            {
                var msg = "";
                if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                {
                    msg = await DisplayPromptAsync("رقم الهاتف غير صحيح", "حسنا");
                    return;
                }
                else
                {
                    msg = await DisplayPromptAsync("Wrong Phone Number", "Ok");
                    return;
                }
            }
        }
        async void Change(int id, string userphone, string PW ,bool admin)
        {
            Users user = new Users();
            user.Id = id;
            user.UserPhone = userphone;
            user.UserPassword = PW;
            var UPR = true;
            user.UserReadPrivacy = UPR;
            user.IsAdmin = admin;
            string url = "https://igs-lf8.conveyor.cloud/api/Users/" + id;
            HttpClient client = new HttpClient();
            string jsonData = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, content);
            if (response.StatusCode.ToString() == "Created")
            {
                if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                {
                    await DisplayAlert("تم","تم تغير كلمة المرور بنجاح", "حسنا");
                    return;
                }
                else
                {
                    await DisplayAlert("Done","The Password Is Changed", "Ok");
                    return;
                }
            }
        }
    }
}