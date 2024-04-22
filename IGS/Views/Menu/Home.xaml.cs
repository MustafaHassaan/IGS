using Firebase.Storage;
using IGS.Appdata;
using IGS.MenuItems;
using IGS.Models;
using IGS.Resx;
using IGS.Services;
using IGS.Views.Dashbord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;
using Xamarin.Forms.Xaml;
using Image = Xamarin.Forms.Image;
using Label = Xamarin.Forms.Label;

namespace IGS.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        public Home()
        {
            InitializeComponent();
            DeleMed();
        }
        async void DeleMed()
        {
            /*كود حذف الاعلان*/
            string url = "https://igs-lf8.conveyor.cloud/api/Medias/GetMedia";
            HttpClient client = new HttpClient();
            HttpResponseMessage HRM = await client.GetAsync(url);
            string result = await HRM.Content.ReadAsStringAsync();
            if (HRM.IsSuccessStatusCode)
            {
                var MedList = JsonConvert.DeserializeObject<List<Medias>>(result);
                if (MedList != null)
                {
                    foreach (var item in MedList)
                    {
                        if (item.MedImageAname != null)
                        {
                            string fileNamea = item.MedImageAname;
                            FirebaseStorage FBSa = new FirebaseStorage("moustafa-igs.appspot.com");
                            await FBSa.Child("Medias").Child(fileNamea).DeleteAsync();
                        }
                        if (item.MedImageBname != null)
                        {
                            string fileNameb = item.MedImageBname;
                            FirebaseStorage FBSb = new FirebaseStorage("moustafa-igs.appspot.com");
                            await FBSb.Child("Medias").Child(fileNameb).DeleteAsync();
                        }
                        if (item.MedImageCname != null)
                        {
                            string fileNamec = item.MedImageCname;
                            FirebaseStorage FBSc = new FirebaseStorage("moustafa-igs.appspot.com");
                            await FBSc.Child("Medias").Child(fileNamec).DeleteAsync();
                        }
                        if (item.MedImageDname != null)
                        {
                            string fileNamed = item.MedImageDname;
                            FirebaseStorage FBSd = new FirebaseStorage("moustafa-igs.appspot.com");
                            await FBSd.Child("Medias").Child(fileNamed).DeleteAsync();
                        }
                        if (item.MedImageEname != null)
                        {
                            string fileNamee = item.MedImageEname;
                            FirebaseStorage FBSe = new FirebaseStorage("moustafa-igs.appspot.com");
                            await FBSe.Child("Medias").Child(fileNamee).DeleteAsync();
                        }
                        if (item.MedImageFname != null)
                        {
                            string fileNamef = item.MedImageFname;
                            FirebaseStorage FBSf = new FirebaseStorage("moustafa-igs.appspot.com");
                            await FBSf.Child("Medias").Child(fileNamef).DeleteAsync();
                        }
                        var id = item.Id;
                        if (item.MedDele < DateTime.Today)
                        {
                            string urld = "https://igs-lf8.conveyor.cloud/api/Medias/" + id;
                            HttpClient clientd = new HttpClient();
                            HttpResponseMessage data = await clientd.DeleteAsync(urld);
                        }
                    }
                }
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
                await Navigation.PushAsync(new Serverdowen());
            }
        }
        private void UList_Refreshing(object sender, EventArgs e)
        {
            GetCategoriesInfo();
            UList.IsRefreshing = false;
        }
        private async void UList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Categories Cat = e.Item as Categories;
            await Navigation.PushAsync(new Mediashow(Cat));
        }
    }
}