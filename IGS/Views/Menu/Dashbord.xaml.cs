using IGS.Resx;
using IGS.Views.Dashbord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Markup;
using Xamarin.Forms.Xaml;

namespace IGS.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashbord : ContentPage
    {
        public Dashbord()
        {
            InitializeComponent();
        }
        private async void Btngac_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Categoriesview());
        }

        private async void Btngai_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Itemsview());
        }

        private async void Btngau_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Usersview());
        }
    }
}