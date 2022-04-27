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

        RSAUtil rSAUtil = new RSAUtil();
        List<string> keys;

        public ViewAuth()
        {
            InitializeComponent();
            checkConnection2server();
            keys = rSAUtil.CreateKeys();

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

            var data = "Поддерживает RSACryptoServiceProvider размеры ключей от 384 до 16384 бит приращения 8 бит, если установлен расширенный поставщик шифрования Майкрософт. Он поддерживает размеры ключей от 384 до 512 бит приращения 8 бит, если установлен базовый поставщик шифрования Майкрософт.Допустимые размеры ключей зависят от поставщика служб шифрования(CSP), используемого экземпляром RSACryptoServiceProvider.Windows поставщики служб конфигурации обеспечивают размер ключа от 384 до 16384 бит для версий Windows до Windows 8.1 и размер ключа от 512 до 16384 бит для Windows 8.1.Дополнительные сведения см. в описании функции CryptGenKey в документации по Windows.Класс RSACryptoServiceProvider не позволяет изменять размеры ключей KeySize с помощью свойства.Любое значение, записанное в это свойство, не сможет обновить свойство без ошибок. Чтобы изменить размер ключа, используйте одну из перегрузок конструктора.";

            string cipher = rSAUtil.Encrypt(keys[1], data);
            string decyptedMessage = rSAUtil.Decrypt(keys[0], cipher);

            await DisplayAlert("final", decyptedMessage, "ok");

            //btnSignIn.IsEnabled = false;
            //btnSignUp.IsEnabled = false;

            //if (tbLogin.Text==null||tbPassword.Text==null)
            //{
            //    await DisplayAlert("Oh shit, I'm sorry!", "No login or password entered", "ok");
            //}
            //else
            //{

            //    classSHA256 classSHA256instance = new classSHA256();
            //    string hashSalt = classSHA256instance.ComputeSha256Hash(tbLogin.Text + tbPassword.Text);

            //    Argon argon = new Argon();
            //    string hashPasswordHex = argon.Argon2id(tbPassword.Text, hashSalt);

            //    classSignature classSign = new classSignature();


            //    classRSA rsa = new classRSA();
            //    string publicKey = rsa.GetPublicBase64();
            //    string privateKey = rsa.GetPrivateBase64();


            //    fetchedUser fetchedData = classSign.SignIn(tbLogin.Text, hashPasswordHex, publicKey);


            //    if (fetchedData != null)
            //    {
            //        WriteLocalData(fetchedData, tbLogin.Text, hashPasswordHex, privateKey);
            //        App.Current.MainPage = new AppShell();
            //    }
            //    else
            //    {
            //        await DisplayAlert("Oh shit, I'm sorry!", "Sorry for what?", "ok");
            //    }
                
            //}

            //btnSignIn.IsEnabled = true;
            //btnSignUp.IsEnabled = true;
        }






        private async void btnSignUp_Clicked(object sender, EventArgs e)
        {
            tbLogin.Text = null;
            tbPassword.Text = null;
            await Navigation.PushAsync(new ViewRegister());
        }






        void WriteLocalData(fetchedUser _fetcheData, string _login,string _password,string _privateKey)
        {
            //Delete all local data
            App.myDB.DeleteAllData();

            classSignature signInstance = new classSignature();

            string imageBase64 = signInstance.GetImage(_fetcheData.id);

            App.myDB.WriteMyData(_fetcheData.id, _fetcheData.public_name, _privateKey, _login, _password, _fetcheData.key_number, imageBase64);
        }

        //private void tbLogin_Completed(object sender, EventArgs e)
        //{
        //    var _passentry = tbPassword;
        //    _passentry?.Focus();
        //}
    }
}