using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Cryptorin.Views;

namespace Cryptorin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //NavigationPage mainPage = new NavigationPage(new viewAuth())
            //{
            //    BarBackgroundColor = Color.FromHex("#a7c5c7"),
            //    BarTextColor = Color.White
            //};
            MainPage = new AppShell();

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
