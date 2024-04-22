using IGS.Models;
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

namespace IGS.Views.Dashbord
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Itemsview : ContentPage
    {
        public Itemsview()
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
        private async void Add_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CDText.Text))
            {
                if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                {
                    await DisplayAlert("خطأ", "الصنف مطلوب", "حسناً");
                    return;
                }
                await DisplayAlert("Error", "Item is Required", "OK");
                return;
            }
            if (picker.SelectedItem == null)
            {
                if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                {
                    await DisplayAlert("خطأ", "الفئه مطلوبه", "حسناً");
                    return;
                }
                await DisplayAlert("Error", "Category is Required", "OK");
                return;
            }
            try
            {
                PU.IsVisible = true;
                AI.IsRunning = true;
                AI.IsVisible = true;

                CategoryDetailes CD = new CategoryDetailes();
                CD.DetailesName = CDText.Text;
                CD.CategoryName = picker.SelectedItem.ToString();
                string url = "https://igs-lf8.conveyor.cloud/api/CategoryDetailes";

                HttpClient client = new HttpClient();
                string jsonData = JsonConvert.SerializeObject(CD);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.StatusCode.ToString() == "Created")
                {
                    AI.IsRunning = false;
                    AI.IsVisible = false;
                    PU.IsVisible = false;
                    if (Lang == "ar-EG" || Lang == "ar-SA" || Lang == "ar-AE")
                    {
                        await DisplayAlert("نجاح", "تمت اضافة الصنف بنجاح", "حسناً");
                    }
                    else
                    {
                        await DisplayAlert("Success", "Your Item is Saved Successfully ...", "Ok");
                    }
                    await Navigation.PushAsync(new Itemopration());
                }
            }
            catch (Exception ex)
            {
                AI.IsRunning = false;
                AI.IsVisible = false;
                PU.IsVisible = false;
                await DisplayAlert("Error", "The Server Is Dowen Now Please Try Again Lettar ...", "Ok");
                return;
            }
        }
        private async void Show_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Itemopration());
        }
    }
}