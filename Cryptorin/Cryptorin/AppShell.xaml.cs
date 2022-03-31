using Cryptorin.Views;
using Cryptorin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cryptorin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {

        //ImageSource imgSourcePhoto;
        
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ViewChat),typeof(ViewChat));

            
            //var byteArray = new WebClient().DownloadData("https://cryptorin.ru/images/filename.jpg");
            //imgSourcePhoto = ImageSource.FromStream(() => new MemoryStream(byteArray));




            Device.StartTimer(new TimeSpan(0, 0, 5), () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //tampering check

                });
                return true;
            });

        }

        private void mnItmQuit_Clicked(object sender, EventArgs e)
        {
            
            App.Current.MainPage = new NavigationPage(new ViewAuth());
            
        }
    }
}