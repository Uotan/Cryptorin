using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Cryptorin.Views;
using Cryptorin.Classes;
using Cryptorin.Data;
using System.IO;

namespace Cryptorin
{
    public partial class App : Application
    {
        static controllerSQLite myDataBase;
        public static controllerSQLite myDB
        {
            get
            {
                if (myDataBase == null)
                {
                    myDataBase = new controllerSQLite(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "myData.db"));
                    //File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NotesDatabase.db"));
                }
                return myDataBase;
            }
        }
        public App()
        {
            InitializeComponent();

            NavigationPage startLoginPage = new NavigationPage(new ViewAuth());
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
