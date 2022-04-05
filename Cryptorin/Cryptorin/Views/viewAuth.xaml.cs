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

namespace Cryptorin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewAuth : ContentPage
    {
        public ViewAuth()
        {
            InitializeComponent();
        }



        private async void btnSignIn_Clicked(object sender, EventArgs e)
        {
            classSignature classSign = new classSignature();
            classSHA256 SHA = new classSHA256();

            publicUserData fetchedData = classSign.SignIn(tbLogin.Text,SHA.ComputeSha256Hash(tbPassword.Text));
            if (fetchedData!=null)
            {
                await DisplayAlert("Yeaaah!","","ok");
                WriteLocalData(fetchedData,tbLogin.Text,tbPassword.Text);
                App.Current.MainPage = new AppShell();
            }
            else
            {
                await DisplayAlert("Oh shit, I'm sorry!", "Sorry for what?", "ok");
            }
        }

        private async void btnSignUp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewRegister());
        }

        void WriteLocalData(publicUserData _fetcheData, string _login,string _password)
        {
            App.myDB.DeleteAllData();
            var pwd = new Password(16).IncludeLowercase().IncludeUppercase().IncludeNumeric().IncludeSpecial("!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~");
            string AESkey = pwd.Next();


            classRSA rsa = new classRSA();
            string publicKey = Convert.ToBase64String(rsa.publicKeyBytes);
            string privateKey = Convert.ToBase64String(rsa.privateKeyBytes);


            var random = new Random();
            var color = String.Format("#{0:X6}", random.Next(0x1000000));
            
            App.myDB.WriteMyData(_fetcheData.id,_fetcheData.public_name,AESkey, privateKey,tbLogin.Text,tbPassword.Text);
        }
        
    }
}