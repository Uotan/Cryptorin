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
            classSignature classSign = new classSignature();

            //classSHA256 SHA = new classSHA256();

            Argon argon = new Argon();
            string hashPasswordHex = argon.Argon2id(tbPassword.Text);

            //string Hash = SHA.ComputeSha256Hash(tbPassword.Text);
            publicUserData fetchedData = classSign.SignIn(tbLogin.Text, hashPasswordHex);
            if (fetchedData!=null)
            {
                WriteLocalData(fetchedData,tbLogin.Text, hashPasswordHex);
                App.Current.MainPage = new AppShell();
            }
            else
            {
                await DisplayAlert("Oh shit, I'm sorry!", "Sorry for what?", "ok");
            }
            btnSignIn.IsEnabled = true;
            btnSignUp.IsEnabled = true;
        }

        private async void btnSignUp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewRegister());
        }

        void WriteLocalData(publicUserData _fetcheData, string _login,string _password)
        {
            //Delete all local data
            App.myDB.DeleteAllData();

            //generate hard password to AES encryption
            //var pwd = new Password(16).IncludeLowercase().IncludeUppercase().IncludeNumeric().IncludeSpecial("!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~");
            //string AESkey = pwd.Next();

            //Generate and fetch RSA keys
            classRSA rsa = new classRSA();
            string publicKey = rsa.GetPublicBase64();
            string privateKey = rsa.GetPrivateBase64();

            //fetch the current key number in the database
            classSignature signInstance = new classSignature();

            string numberResult = signInstance.SignInUpdateKeys(_login, _password, publicKey);

            string imageBase64 = signInstance.GetImage(_fetcheData.id);

            App.myDB.WriteMyData(_fetcheData.id, _fetcheData.public_name, privateKey, _login, _password, numberResult,imageBase64);
        }

        private void tbLogin_Completed(object sender, EventArgs e)
        {
            var _passentry = tbPassword;
            _passentry?.Focus();
        }
    }
}