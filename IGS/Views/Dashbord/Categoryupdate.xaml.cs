using Firebase.Storage;
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
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGS.Views.Dashbord
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Categoryupdate : ContentPage
    {
        public Categoryupdate(Categories Cat)
        {
            InitializeComponent();

            var x = Convert.ToString(Cat.Id);
            if (x != null &&
                Cat.CategoryImagePath != null)
            {
                txtId.Text = Convert.ToString(Cat.Id);
                CatName.Text = Cat.CategoryName;
                var GBS = Cat.CategoryImagePath;
                BtnUpdate.IsVisible = true;
            }
            else
            {
                BtnUpdate.IsVisible = false;
            }
        }
        string Lang = Thread.CurrentThread.CurrentUICulture.Name;

        public MediaFile MF;
        private Stream stream;
        private async void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CatName.Text))
            {
                if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                {
                    await DisplayAlert("خطأ", "الفئه مطلوبه", "حسناً");
                    return;
                }
                await DisplayAlert("Input Error", "Category is Required", "OK");
                return;
            }
            try
            {
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                BtnUpdate.IsEnabled = false;
                var UP = "";
                var Db = App.Dbi().Conn;
                var UA = Db.Table<AuthUser>().Where(u => u.UserAuthentication == true).FirstOrDefault();
                if (UA != null)
                {
                    UP = UA.UserPhone;
                }
                Categories Cat = new Categories();
                Cat.Id = Convert.ToInt32(txtId.Text);
                Cat.CategoryName = CatName.Text;
                Cat.UserPhone = UP;
                var id = Cat.Id;
                string url = "https://igs-lf8.conveyor.cloud/api/Categories/" + id;
                HttpClient client = new HttpClient();
                if (MF != null)
                {
                    HttpResponseMessage img = await client.GetAsync(url);
                    string result = await img.Content.ReadAsStringAsync();
                    var Res = JsonConvert.DeserializeObject<Categories>(result);
                    string fileName = Res.CategoryImageName;
                    FirebaseStorage FBS = new FirebaseStorage("moustafa-igs.appspot.com");
                    await FBS.Child("Categories").Child(fileName).DeleteAsync();
                    if (img.StatusCode.ToString() == "OK")
                    {
                        var imageUrlN = await FBS.Child("Categories").Child(Path.GetFileName(MF.Path)).PutAsync(MF.GetStream());
                        Cat.CategoryImageName = Path.GetFileName(MF.Path);
                        Cat.CategoryImagePath = imageUrlN;
                        string jsonData = JsonConvert.SerializeObject(Cat);
                        StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PutAsync(url, content);
                        PU.IsVisible = false;
                        AI.IsRunning = false;
                        AI.IsVisible = false;
                        if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                        {
                            await DisplayAlert("نجاح", "تم تعديل الفئه بنجاح", "حسناً");
                        }
                        else
                        {
                            await DisplayAlert("Success", "The Category Update Successfully ...", "Ok");
                        }
                    }
                }
                else
                {

                    string jsonData = JsonConvert.SerializeObject(Cat);
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync(url, content);
                    if (response.StatusCode.ToString() == "Created")
                    {
                        PU.IsVisible = false;
                        AI.IsRunning = false;
                        AI.IsVisible = false;
                        if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                        {
                            await DisplayAlert("نجاح", "تم تعديل الفئه بنجاح", "حسناً");
                        }
                        else
                        {
                            await DisplayAlert("Success", "The Category Update Successfully ...", "Ok");
                        }
                        await Navigation.PushAsync(new Categoryopration());
                    }
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
                await DisplayAlert("Error", "This is not support on your device.", "OK");
                return;
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
                imageView.IsVisible = true;
                btnSelectPic.IsVisible = false;
            }
        }
    }
}