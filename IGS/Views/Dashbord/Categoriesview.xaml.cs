using Firebase.Storage;
using IGS.Appdata;
using IGS.Models;
using IGS.Services;
using IGS.Views.Menu;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class Categoriesview : ContentPage
    {
        public Categoriesview()
        {
            InitializeComponent();
        }
        public MediaFile MF;
        private Stream stream;
        string Lang = Thread.CurrentThread.CurrentUICulture.Name;
        private async void btnSelectPic_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                {
                    await DisplayAlert("خطأ", "غير مدعوم في هذا الجهاز", "حسناً");
                    return;
                }
                else
                {
                    await DisplayAlert("Error", "This is not support on your device.", "OK");
                    return;
                }
            }
            else
            {
                var mediaOption = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                MF = await CrossMedia.Current.PickPhotoAsync();
                if (MF == null) return;
                stream = MF.GetStream();
                imageView.Source = ImageSource.FromStream(() => MF.GetStream());

                btnSelectPic.IsVisible = false;
                GI.IsVisible = false;
                Category.IsVisible = true;
                Add.IsVisible = true;
            }
        }
        private void CP_Clicked(object sender, EventArgs e)
        {
            btnSelectPic.IsVisible = true;
            Category.IsVisible = false;
            GI.IsVisible = true;
            Add.IsVisible = false;
        }
        private async void Add_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Cattext.Text))
            {
                if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                {
                    await DisplayAlert("خطأ", "حقل الفئه مطلوب", "حسناً");
                    return;
                }
                else
                {
                    await DisplayAlert("Error", "Category is Required", "OK");
                    return;
                }
            }
            try
            {
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                FirebaseStorage FBS = new FirebaseStorage("moustafa-igs.appspot.com");
                var imageUrl = await FBS.Child("Categories").Child(Path.GetFileName(MF.Path)).PutAsync(MF.GetStream());
                var UP = "";
                var Db = App.Dbi().Conn;
                var UA = Db.Table<AuthUser>().Where(u => u.UserAuthentication == true).FirstOrDefault();
                if (UA != null)
                {
                    UP = UA.UserPhone;
                }
                Categories Cat = new Categories();
                Cat.CategoryName = Cattext.Text;
                Cat.UserPhone = UP;
                Cat.CategoryImageName = Path.GetFileName(MF.Path);
                Cat.CategoryImagePath = imageUrl;
                HttpClient client = new HttpClient();
                string urlc = "https://igs-lf8.conveyor.cloud/api/Categories";
                string jsonData = JsonConvert.SerializeObject(Cat);
                StringContent contentc = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(urlc, contentc);
                if (response.StatusCode.ToString() == "Created")
                {
                    AI.IsRunning = false;
                    AI.IsVisible = false;
                    PU.IsVisible = false;
                    if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                    {
                        await DisplayAlert("نجاح", "تمت اضافة الفئه بنجاح", "حسناً");
                    }
                    else
                    {
                        await DisplayAlert("Success", "Your Category is Saved Successfully ...", "Ok");
                    }
                    Cattext.Text = "";
                    btnSelectPic.IsVisible = true;
                    GI.IsVisible = true;
                    Category.IsVisible = false;
                    Add.IsVisible = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                PU.IsVisible = false;
                AI.IsRunning = false;
                AI.IsVisible = false;
                await Navigation.PushAsync(new Serverdowen());
            }
        }
        private async void Btngac_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Categoryopration());
        }
    }
}