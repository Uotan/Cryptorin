using Cryptorin.Views;
using Cryptorin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Cryptorin.Data;
using Cryptorin.Classes;
using Cryptorin.Classes.SQLiteClasses;
using System.Diagnostics;

namespace Cryptorin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        bool timerAlive = false;
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ViewChat),typeof(ViewChat));

            timerAlive = !timerAlive;

            CheckConnection();

            CheckAnotherEntry();

            EnterSecurityCode();


            Debug.WriteLine("App Shell Init END");
            //Device.StartTimer(new TimeSpan(0, 0, 5), () =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        checkConnection2server();
            //    });
            //    return true;
            //});

        }


        async void EnterSecurityCode()
        {
            string code = await DisplayPromptAsync("Enter the security code", "Enter code:", keyboard: Keyboard.Email);

            if (code==null||code == "")
            {
                await DisplayAlert("Oh, I'm sorry!", "Invalid security code", "ok");
                return;
            }
            code = code.Trim();
            classSHA256 sHA256 = new classSHA256();
            string hash_secureCode = sHA256.ComputeSha256Hash(code);
            //hash_secureCode = hash_secureCode.Remove(16);
            hash_secureCode = hash_secureCode.Remove(32);

            classAES aES = new classAES(hash_secureCode);

            string secretCodeFromMemory = Preferences.Get("secretCode",null);

            string decryptedSecreCode = aES.Decrypt(secretCodeFromMemory);

            if (decryptedSecreCode!=null)
            {
                decryptedSecreCode = decryptedSecreCode.Trim();
            }

            if (decryptedSecreCode == hash_secureCode)
            {
                keyClass.AESkey = hash_secureCode;
                keyClass.isUnlock = true;
            }
            else
            {
                await DisplayAlert("Oh, I'm sorry!", "Invalid security code", "ok");
            }
        }

        private async void mnItmQuit_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Are you sure?", "This will destroy all data locally and in the database!", "Yes", "No");
            if (answer)
            {
                //delete all data methods
                App.myDB.DeleteAllData();
                App.Current.MainPage = new NavigationPage(new ViewAuth());
            }
            
            
        }

        private async void mnItmSource_Clicked(object sender, EventArgs e)
        {
            Uri uri = new Uri("https://github.com/Uotan/Cryptorin");
            await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }

        async void CheckConnection()
        {

            while (timerAlive)
            {
                checkConnection checker = new checkConnection();
                bool result = checker.ConnectionAvailable(ServerAddress.srvrAddress);
                if (!result)
                {
                    await DisplayAlert("Error", "The connection to the server is not established", "Ok");
                }
                await Task.Delay(10000);
            }
        }

        async void CheckAnotherEntry()
        {
            MyData mydata = new MyData();
            mydata = App.myDB.ReadMyData();
            while (timerAlive)
            {
                classSignature signature = new classSignature();
                var result = signature.GetUserKeyNumber(mydata.id);
                if (result==null)
                {
                    return;
                }
                if (result!=mydata.key_number)
                {
                    otherEntryController.myKeyChanged = true;
                    await Task.Delay(1000);
                    //App.myDB.DeleteAllMessages();
                    //mydata.key_number = result;
                    //App.myDB.UpdateMyData(mydata);
                    //mydata.key_number = result;
                    //App.myDB.UpdateMyData(mydata);
                    await DisplayAlert("Attention", "Logged in from another device - change your password and update keys!", "Ok");
                    mydata = App.myDB.ReadMyData();
                }
                await Task.Delay(3000);
                otherEntryController.myKeyChanged = false;
            }
        }
    }
}