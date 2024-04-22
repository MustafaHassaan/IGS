using Firebase.Storage;
using IGS.Appdata;
using IGS.Models;
using IGS.Services;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using Xamarin.Forms.Xaml;
using Location = Xamarin.Essentials.Location;

namespace IGS.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaceAd : ContentPage
    {
        string Lang = Thread.CurrentThread.CurrentUICulture.Name;

        public PlaceAd()
        {
            InitializeComponent();
            GetCategory();
        }
        public MediaFile MFa;
        public MediaFile MFb;
        public MediaFile MFc;
        public MediaFile MFd;
        public MediaFile MFe;
        public MediaFile MFf;
        private Stream streama;
        private Stream streamb;
        private Stream streamc;
        private Stream streamd;
        private Stream streame;
        private Stream streamf;
        async void GetCategory()
        {
            try
            {
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                string url = "https://igs-lf8.conveyor.cloud/api/Categories";
                HttpClient client = new HttpClient();
                var result = await client.GetStringAsync(url);

                var Cat = JsonConvert.DeserializeObject<List<Categories>>(result);
                MedCat.ItemsSource = Cat;
                PU.IsVisible = false;
                AI.IsRunning = false;
                AI.IsVisible = false;
            }
            catch (Exception)
            {
                PU.IsVisible = false;
                AI.IsRunning = false;
                AI.IsVisible = false;
                await Navigation.PushAsync(new Serverdowen());
            }
        }
        private async void MedSave_Clicked(object sender, EventArgs e)
        {
            if (MedName.Text == null || MedDisc.Text == null ||
                MedItem.SelectedItem.ToString() == null ||
                MedCat.SelectedItem.ToString() == null ||
                Meduser.Text == null || MedContect.Text == null)
            {
                if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                {
                    await DisplayAlert("خطأ", "يجب اضافة جميع الحقول", "حسناً");
                    return;
                }
                await DisplayAlert("Error", "Please Write All Filds ...", "Ok");
                return;
            }
            PU.IsVisible = true;
            AI.IsRunning = true;
            AI.IsVisible = true;
            Medias Med = new Medias();
            Med.MedName = MedName.Text;
            Med.AdvertiserName = Meduser.Text;
            Med.MedDescription = MedDisc.Text;
            Med.MedConteact = MedContect.Text;
            Med.DetailesName = MedItem.SelectedItem.ToString();
            Med.CategoryName = MedCat.SelectedItem.ToString();
            string url = "https://igs-lf8.conveyor.cloud/api/MediasUploads";
            HttpClient client = new HttpClient();
            FirebaseStorage FBS = new FirebaseStorage("moustafa-igs.appspot.com");
            if (Ia.Source.ToString() != "File: Pic.png")
            {
                var imageUrla = await FBS.Child("Medias").Child(Path.GetFileName(MFa.Path)).PutAsync(MFa.GetStream());
                Med.MedImageAname = Path.GetFileName(MFa.Path);
                Med.MedImageApath = imageUrla;
            }
            if (Ib.Source.ToString() != "File: Pic.png")
            {
                var imageUrlb = await FBS.Child("Medias").Child(Path.GetFileName(MFb.Path)).PutAsync(MFb.GetStream());
                Med.MedImageBname = Path.GetFileName(MFb.Path);
                Med.MedImageBpath = imageUrlb;
            }
            if (Ic.Source.ToString() != "File: Pic.png")
            {
                var imageUrlc = await FBS.Child("Medias").Child(Path.GetFileName(MFc.Path)).PutAsync(MFc.GetStream());
                Med.MedImageCname = Path.GetFileName(MFc.Path);
                Med.MedImageCpath = imageUrlc;
            }
            if (Id.Source.ToString() != "File: Pic.png")
            {
                var imageUrld = await FBS.Child("Medias").Child(Path.GetFileName(MFd.Path)).PutAsync(MFd.GetStream());
                Med.MedImageDname = Path.GetFileName(MFd.Path);
                Med.MedImageDpath = imageUrld;
            }
            if (Ie.Source.ToString() != "File: Pic.png")
            {
                var imageUrle = await FBS.Child("Medias").Child(Path.GetFileName(MFe.Path)).PutAsync(MFe.GetStream());
                Med.MedImageEname = Path.GetFileName(MFe.Path);
                Med.MedImageEpath = imageUrle;
            }
            if (If.Source.ToString() != "File: Pic.png")
            {
                var imageUrlf = await FBS.Child("Medias").Child(Path.GetFileName(MFf.Path)).PutAsync(MFf.GetStream());
                Med.MedImageFname = Path.GetFileName(MFf.Path);
                Med.MedImageFpath = imageUrlf;
            }
            Med.MedLcoationname = MedLocation.Text;
            if (Medlat.Text != null && Medlon.Text != null)
            {
                Med.MedLcoationlat = float.Parse(Medlat.Text);
                Med.MedLcoationlon = float.Parse(Medlon.Text);
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);
                if (location != null)
                {
                    var latitude = location.Latitude;
                    Medlat.Text = Convert.ToString(latitude);
                    var longitude = location.Longitude;
                    Medlon.Text = Convert.ToString(longitude);
                    var placemarks = await Geocoding.GetPlacemarksAsync(latitude, longitude);
                    var placemark = placemarks?.FirstOrDefault();
                    if (placemark != null)
                    {
                        var PAE = placemark.AdminArea;
                        Med.MedStuation = PAE;
                    }
                }
            }
            Med.MedPrice = Convert.ToDecimal(MedPrice.Text);
            Med.MedDate = DateTime.Today;
            Med.MedDele = DateTime.Today.AddDays(5);
            var Db = App.Dbi().Conn;
            var UA = Db.Table<AuthUser>().Where(u => u.UserAuthentication == true).FirstOrDefault();
            if (UA != null)
            {
                Med.UserPhone = UA.UserPhone;
            }
            try
            {
                string urlc = "https://igs-lf8.conveyor.cloud/api/Medias";

                HttpClient clientc = new HttpClient();
                string jsonData = JsonConvert.SerializeObject(Med);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(urlc, content);
                if (response.StatusCode.ToString() == "OK")
                {
                    AI.IsRunning = false;
                    AI.IsVisible = false;
                    PU.IsVisible = false;
                    if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                    {
                        await DisplayAlert("خطأ", "الهاتف مطلوب", "حسناً");
                    }
                    else
                    {
                        await DisplayAlert("Success", "Your Adds is Saved Successfully ...", "Ok");
                    }
                    App.Current.MainPage = new IGS.MainPage();
                }
            }
            catch (Exception)
            {
                PU.IsVisible = false;
                AI.IsRunning = false;
                AI.IsVisible = false;
                await Navigation.PushAsync(new Serverdowen());
            }
        }
        private async void Imga_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "This is not support on your device.", "OK");
                return;
            }
            else
            {
                MFa = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions 
                {
                    Name = "Pictures" + ".jpg"
                });
                if (MFa == null) return;
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                streama = MFa.GetStream();
                Ia.Source = ImageSource.FromStream(() =>
                {
                    return MFa.GetStream();
                });
                Imga.IsVisible = false;
                Ia.IsVisible = true;
                PU.IsVisible = false;
                AI.IsRunning = false;
                AI.IsVisible = false;
            }
        }
        private async void Imgb_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "This is not support on your device.", "OK");
                return;
            }
            else
            {
                MFb = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Name = "Pictures" + ".jpg"
                });
                if (MFb == null) return;
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                streamb = MFb.GetStream();
                Ib.Source = ImageSource.FromStream(() =>
                {
                    return MFb.GetStream();
                });
                Imgb.IsVisible = false;
                Ib.IsVisible = true;
                PU.IsVisible = false;
                AI.IsRunning = false;
                AI.IsVisible = false;
            }
        }
        private async void Imgc_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "This is not support on your device.", "OK");
                return;
            }
            else
            {
                MFc = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Name = "Pictures" + ".jpg"
                });
                if (MFc == null) return;
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                streamc = MFc.GetStream();
                Ic.Source = ImageSource.FromStream(() =>
                {
                    return MFc.GetStream();
                });
                Imgc.IsVisible = false;
                Ic.IsVisible = true;
                PU.IsVisible = false;
                AI.IsRunning = false;
                AI.IsVisible = false;
            }
        }
        private async void Imgd_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "This is not support on your device.", "OK");
                return;
            }
            else
            {
                MFd = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Name = "Pictures" + ".jpg"
                });
                if (MFd == null) return;
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                streamd = MFd.GetStream();
                Id.Source = ImageSource.FromStream(() =>
                {
                    return MFd.GetStream();
                });
                Imgd.IsVisible = false;
                Id.IsVisible = true;
                PU.IsVisible = false;
                AI.IsRunning = false;
                AI.IsVisible = false;
            }
        }
        private async void Imge_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "This is not support on your device.", "OK");
                return;
            }
            else
            {
                MFe = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Name = "Pictures" + ".jpg"
                });
                if (MFe == null) return;
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                streame = MFe.GetStream();
                Ie.Source = ImageSource.FromStream(() =>
                {
                    return MFe.GetStream();
                });
                Imge.IsVisible = false;
                Ie.IsVisible = true;
                PU.IsVisible = false;
                AI.IsRunning = false;
                AI.IsVisible = false;
            }
        }
        private async void Imgf_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "This is not support on your device.", "OK");
                return;
            }
            else
            {
                MFf = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Name = "Pictures" + ".jpg"
                });
                if (MFf == null) return;
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                streamf = MFf.GetStream();
                If.Source = ImageSource.FromStream(() =>
                {
                    return MFf.GetStream();
                });
                Imgf.IsVisible = false;
                If.IsVisible = true;
                PU.IsVisible = false;
                AI.IsRunning = false;
                AI.IsVisible = false;
            }
        }
        private async void MedCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            Categories Cat = new Categories();
            var SelectedCat = MedCat.SelectedItem.ToString();
            Cat.CategoryName = SelectedCat;
            try
            {
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;
                string url = "https://igs-lf8.conveyor.cloud/api/Medias/Categories";

                HttpClient client = new HttpClient();
                string jsonData = JsonConvert.SerializeObject(Cat);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode.ToString() == "OK")
                {
                    var Catdet = JsonConvert.DeserializeObject<List<CategoryDetailes>>(result);
                    MedItem.ItemsSource = Catdet;
                }
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
        private async void MedLocation_Focused(object sender, FocusEventArgs e)
        {
            var Ques = false;
            if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
            {
                Ques = await DisplayAlert("سؤال ؟", "هل تريد الحصول على الموقع او كتابته ؟", "الحصول على الموقع", "كتابته");
                if (Ques)
                {
                    GetLoation();
                }
                else
                {
                    return;
                }
            }
            else
            {
                Ques = await DisplayAlert("Question ?", "Are you want Geting Location Or writing ?", "Location", "Writing");
                if (Ques)
                {
                    GetLoation();
                }
                else
                {
                    return;
                }
            }
            async void GetLoation()
            {
                try
                {
                    PU.IsVisible = true;
                    AI.IsRunning = true;
                    AI.IsVisible = true;
                    var request = new GeolocationRequest(GeolocationAccuracy.Best);
                    var location = await Geolocation.GetLocationAsync(request);
                    if (location != null)
                    {
                        var latitude = location.Latitude;
                        Medlat.Text = Convert.ToString(latitude);
                        var longitude = location.Longitude;
                        Medlon.Text = Convert.ToString(longitude);
                        var placemarks = await Geocoding.GetPlacemarksAsync(latitude, longitude);
                        var placemark = placemarks?.FirstOrDefault();
                        if (placemark != null)
                        {
                            var streat = "شارع";
                            var countrycode = placemark.CountryCode;
                            var Country = placemark.CountryName;
                            var City = placemark.AdminArea;
                            var Locality = placemark.Locality;
                            var SubAdminArea = placemark.SubAdminArea;
                            var Streetnumber = placemark.SubThoroughfare;
                            var Streetname = placemark.Thoroughfare;
                            MedLocation.Text = Streetnumber + " " +
                                               streat + " " +
                                               Streetname + " - " +
                                               SubAdminArea + " - " +
                                               City + " - " +
                                               Country;
                        }
                        PU.IsVisible = false;
                        AI.IsRunning = false;
                        AI.IsVisible = false;
                    }
                }
                catch (Exception ex)
                {
                    if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                    {
                        await DisplayAlert("خطأ", "يجب تشغيل محدد الموقع لكي يعمل البرنامج", "حسنا");
                        PU.IsVisible = false;
                        AI.IsRunning = false;
                        AI.IsVisible = false;
                        return;
                    }
                    await DisplayAlert("Error", ex.Message, "OK");
                    PU.IsVisible = false;
                    AI.IsRunning = false;
                    AI.IsVisible = false;
                    return;
                }
            }     
        }
    }
}
