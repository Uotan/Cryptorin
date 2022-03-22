using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cryptorin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class viewAuth : ContentPage
    {
        public viewAuth()
        {
            InitializeComponent();
            using (WebClient client = new WebClient())
            {
                var response = client.DownloadString("https://cryptorin.ru/index.php");
                testEcho.Text = response;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new viewRegister();
            //await Navigation.PushModalAsync(new viewRegister());
        }
    }
}