using IGS.Resx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGS.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class About : ContentPage
    {
        public About()
        {
            InitializeComponent();
            CRA.Text = @"©"+" "+"2020 - 2021"+" "+ AppResources.DB +"\n"+ AppResources.ARR;
        }

        private void FB_Clicked(object sender, EventArgs e)
        {

        }
    }
}