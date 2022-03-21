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

        private void Button_Clicked(object sender, EventArgs e)
        {
            Random random = new Random();
            int ran1 = random.Next(0, 255);
            int ran2 = random.Next(0, 255);
            int ran3 = random.Next(0, 255);
            Color colorLabel = Color.FromRgb(ran1, ran2, ran3);
            App.Current.Resources["labelColor"] = colorLabel;
        }
    }
}