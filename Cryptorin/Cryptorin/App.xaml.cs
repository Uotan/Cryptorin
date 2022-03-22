using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Cryptorin.Views;
using Cryptorin.Classes;

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

            Application.Current.RequestedThemeChanged += (object sender, AppThemeChangedEventArgs e) =>
            {
                ThemeManager themeManager = new ThemeManager();
                if (e.RequestedTheme == OSAppTheme.Dark)
                    themeManager.SetDark();
                else
                    themeManager.SetLight();
            };

            //MainPage = new AppShell();
            MainPage = new viewAuth();

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
