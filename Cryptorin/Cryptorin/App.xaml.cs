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

            NavigationPage startLoginPage = new NavigationPage(new viewAuth())
            {
                //BarBackgroundColor = Color.FromHex("#a7c5c7"),
                //BarTextColor = Color.White
            };
            OSAppTheme currentTheme = Application.Current.RequestedTheme;
            Application.Current.RequestedThemeChanged += (object sender, AppThemeChangedEventArgs e) =>
            {
                ThemeManager themeManager = new ThemeManager();
                if (e.RequestedTheme == OSAppTheme.Dark)
                    themeManager.SetDark();
                else
                    themeManager.SetLight();
            };
            if (currentTheme == OSAppTheme.Dark)
            {
                ThemeManager themeManager = new ThemeManager();
                themeManager.SetDark();
            }
            if (currentTheme == OSAppTheme.Light)
            {
                ThemeManager themeManager = new ThemeManager();
                themeManager.SetLight();
            }

            //MainPage = new AppShell();
            MainPage = startLoginPage;

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            //Application.Current.Quit();
        }

        protected override void OnResume()
        {
        }
    }
}
