using IGS.Appdata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Logout : ContentPage
    {
        public Logout()
        {
            var Db = App.Dbi().Conn;
            var UA = Db.Table<AuthUser>().Where(u => u.UserAuthentication == true).FirstOrDefault();
            if (UA != null)
            {
                Db.Table<AuthUser>().Delete(x => x.Id == UA.Id);
                App.Current.MainPage = new IGS.MainPage();
            }
            InitializeComponent();
        }
    }
}