using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Cryptorin.Data;
using Cryptorin.Classes;
using PasswordGenerator;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Cryptorin.Classes.SQLiteClasses;
using System.Diagnostics;
using Xamarin.Essentials;

namespace Cryptorin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewAuth : ContentPage
    {



        public ViewAuth()
        {
            InitializeComponent();
            checkConnection2server();

        }






        async void checkConnection2server()
        {
            checkConnection checker = new checkConnection();
            bool result = checker.ConnectionAvailable(ServerAddress.srvrAddress);
            if (!result)
            {
                await DisplayAlert("Error", "The connection to the server is not established", "Ok");
            }
        }







        private async void btnSignIn_Clicked(object sender, EventArgs e)
        {

            btnSignIn.IsEnabled = false;
            btnSignUp.IsEnabled = false;

            if (tbLogin.Text == null || tbPassword.Text == null)
            {
                await DisplayAlert("Oh s**t, I'm sorry!", "No login or password entered", "ok");
            }
            else
            {

                classSHA256 classSHA256instance = new classSHA256();
                string hashSalt = classSHA256instance.ComputeSha256Hash(tbLogin.Text + tbPassword.Text);

                Argon argon = new Argon();
                string hashPasswordHex = argon.Argon2id(tbPassword.Text, hashSalt);

                classSignature classSign = new classSignature();


                RSAUtil rSAUtil = new RSAUtil();
                List<string> keys = rSAUtil.CreateKeys();


                fetchedUser fetchedData = classSign.SignIn(tbLogin.Text, hashPasswordHex, keys[1]);


                if (fetchedData != null)
                {
                    WriteLocalData(fetchedData, tbLogin.Text, hashPasswordHex, keys[0]);
                    App.Current.MainPage = new AppShell();
                }
                else
                {
                    await DisplayAlert("Oh s**t, I'm sorry!", "Authorization error.", "ok");
                }

            }

            btnSignIn.IsEnabled = true;
            btnSignUp.IsEnabled = true;




        }






        private async void btnSignUp_Clicked(object sender, EventArgs e)
        {
            tbLogin.Text = null;
            tbPassword.Text = null;
            await Navigation.PushAsync(new ViewRegister());
        }






        void WriteLocalData(fetchedUser _fetcheData, string _login, string _password, string _privateKey)
        {
            App.myDB.DeleteAllData();

            classSignature signInstance = new classSignature();

            string imageBase64 = signInstance.GetImage(_fetcheData.id);

            App.myDB.WriteMyData(_fetcheData.id, _fetcheData.public_name, _privateKey, _login, _password, _fetcheData.key_number, imageBase64,_fetcheData.changes_index);
        }

        private async void toolItmChangeDomain_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Change domain", "Enter new domain (default: https://cryptorin.ru):", keyboard: Keyboard.Email);
            if (result == null || result == "")
            {
                return;
            }
            Preferences.Set("serverAddress", result);
            ServerAddress.srvrAddress = Preferences.Get("serverAddress", null);
        }

        //private void tbLogin_Completed(object sender, EventArgs e)
        //{
        //    var _passentry = tbPassword;
        //    _passentry?.Focus();
        //}
    }
}