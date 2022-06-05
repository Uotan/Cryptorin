using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Cryptorin.Data;
using Cryptorin.Classes;
//using PasswordGenerator;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Cryptorin.Classes.SQLiteClasses;
using System.Diagnostics;
using Xamarin.Essentials;
using System.Drawing;
using System.IO;

namespace Cryptorin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewAuth : ContentPage
    {

        public ImageSource Source;

        public ViewAuth()
        {
            InitializeComponent();
            
            checkConnection2server();

        }






        async void checkConnection2server()
        {
            CheckConnection checker = new CheckConnection();
            bool result = checker.ConnectionAvailable(ServerAddress.srvrAddress);
            Debug.WriteLine(result);
            if (!result)
            {
                await DisplayAlert("Error", "The connection to the server is not established", "Ok");
                return;
            }
            
            try
            {
                
                var byteArray = new WebClient().DownloadData(ServerAddress.srvrAddress+"/logo.png");
                if (byteArray==null)
                {
                    return;
                }
                Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
                serverLogo.Source = Source;
            }
            catch (Exception)
            {
                return;
            }
        }



        async Task<bool> checkConnet()
        {
            await Task.Delay(50);
            CheckConnection checker = new CheckConnection();
            bool connectionResult = checker.ConnectionAvailable(ServerAddress.srvrAddress);
            if (!connectionResult)
            {
                await DisplayAlert("Error", "The connection to the server is not established", "Ok");
            }

            return connectionResult;
        }



        private async void btnSignIn_Clicked(object sender, EventArgs e)
        {
            if (tbLogin.Text == null || tbPassword.Text == null|| tbLogin.Text==""|| tbPassword.Text == "")
            {
                await DisplayAlert("Oh, I'm sorry!", "No login or password entered", "ok");
            }
            else
            {
                btnSignIn.Text = "Please Wait";
                btnSignIn.IsEnabled = false;
                btnSignUp.IsEnabled = false;

                var connectResult = await checkConnet();

                if (connectResult)
                {
                    var writeResult = await AuthMethod();
                    if (writeResult)
                    {
                        App.Current.MainPage = new AppShell();
                    }
                    else
                    {
                        await DisplayAlert("Oh, I'm sorry!", "Something's wrong", "ok");
                        btnSignIn.IsEnabled = true;
                        btnSignUp.IsEnabled = true;
                        btnSignIn.Text = "Sign In";
                    }
                }
                else
                {
                    btnSignIn.IsEnabled = true;
                    btnSignUp.IsEnabled = true;
                    btnSignIn.Text = "Sign In";
                    return;
                }



            }
        }

        async Task<bool> AuthMethod()
        {
            await Task.Delay(50);
            ClassSHA256 classSHA256instance = new ClassSHA256();
            string hashSaltPassword = classSHA256instance.ComputeSha256Hash(tbPassword.Text);
            string hashSaltLogin = classSHA256instance.ComputeSha256Hash(tbLogin.Text);

            Argon argon = new Argon();
            string hashPasswordHex = argon.Argon2id(tbPassword.Text, hashSaltPassword);
            string hashLoginHex = argon.Argon2id(tbLogin.Text, hashSaltLogin);

            ClassSignature classSign = new ClassSignature();


            RSAUtil rSAUtil = new RSAUtil();
            List<string> keys = rSAUtil.CreateKeys();


            FetchedUser fetchedData = classSign.SignIn(hashLoginHex, hashPasswordHex, keys[1]);

            if (fetchedData != null)
            {
                string code = await DisplayPromptAsync("Come up with a security code", "Enter code:", keyboard: Keyboard.Email);
                
                if (code==null||code=="")
                {
                    return false;
                }
                code = code.Trim();
                string codeArgon = argon.Argon2id(code,code.Length.ToString()+ "#iN6H2V#");
                ClassSHA256 sHA256 = new ClassSHA256();
                string hash_secureCode = sHA256.ComputeSha256Hash(codeArgon);
                hash_secureCode = hash_secureCode.Remove(16);
                //hash_secureCode = hash_secureCode.Remove(32);
                KeyClass.AESkey = hash_secureCode;
                ClassAES aES = new ClassAES(hash_secureCode);


                var EncryptedSecurityCode = aES.Encrypt(hash_secureCode);
                var EncryptedPassword = aES.Encrypt(hashPasswordHex);
                var EncryptedLogin = aES.Encrypt(hashLoginHex);
                Preferences.Set("secretCode", EncryptedSecurityCode);

                string symmetricallyEncryptedKey = aES.Encrypt(keys[0]);
                WriteLocalData(fetchedData, EncryptedLogin, EncryptedPassword, symmetricallyEncryptedKey);
                return true;
                
            }
            else
            {
                return false;
            }
        }





        private async void btnSignUp_Clicked(object sender, EventArgs e)
        {
            tbLogin.Text = null;
            tbPassword.Text = null;
            await Navigation.PushAsync(new ViewRegister());
        }






        void WriteLocalData(FetchedUser _fetcheData, string _login, string _password, string _privateKey)
        {
            App.myDB.DeleteAllData();

            ClassSignature signInstance = new ClassSignature();

            string imageBase64 = signInstance.GetImage(_fetcheData.id);

            App.myDB.WriteMyData(_fetcheData.id, _fetcheData.public_name, _privateKey, _login, _password, _fetcheData.key_number, imageBase64, _fetcheData.changes_index);
        }

        private async void toolItmChangeDomain_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Change domain", "Enter new domain (current: "+ServerAddress.srvrAddress+"):", keyboard: Keyboard.Email);
            if (result == null || result == "")
            {
                return;
            }
            Preferences.Set("serverAddress", result);
            ServerAddress.srvrAddress = Preferences.Get("serverAddress", null);
            checkConnection2server();
        }

        private async void toolItmCodeSource_Clicked(object sender, EventArgs e)
        {
            Uri uri = new Uri("https://github.com/Uotan/Cryptorin");
            await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
    }
}