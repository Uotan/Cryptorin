using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Cryptorin.Views;
using Cryptorin.Classes;
using Cryptorin.Data;
using System.IO;
using Cryptorin.Classes.SQLiteClasses;
using Xamarin.Essentials;

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
            Themer();
            MyData myData = App.myDB.ReadMyData();
            if (myData != null)
            {
                MainPage = new AppShell();
            }
            else
            {
                MainPage = startLoginPage;
            }
        }

        void Themer()
        {
            

            //OSAppTheme currentTheme = Application.Current.RequestedTheme;

            string themeStart = Preferences.Get("theme", null);


            Application.Current.RequestedThemeChanged += (object sender, AppThemeChangedEventArgs e) =>
            {
                string theme = Preferences.Get("theme", null);
                if (theme == "system")
                {
                    ThemeManager themeManager = new ThemeManager();
                    if (e.RequestedTheme == OSAppTheme.Dark)
                        themeManager.SetDark();
                    else
                        themeManager.SetLight();
                }

            };



            if (themeStart == "dark")
            {
                ThemeManager themeManager = new ThemeManager();
                themeManager.SetDark();
            }
            else if (themeStart == "light")
            {
                ThemeManager themeManager = new ThemeManager();
                themeManager.SetLight();
            }
            else if (themeStart == "system")
            {
                ThemeManager themeManager = new ThemeManager();
                OSAppTheme currentTheme = Application.Current.RequestedTheme;
                if (currentTheme == OSAppTheme.Dark)
                {
                    themeManager.SetDark();
                }
                if (currentTheme == OSAppTheme.Light)
                {
                    themeManager.SetLight();
                }
            }
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
