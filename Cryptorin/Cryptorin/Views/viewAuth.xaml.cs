using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Cryptorin.Data;
using Cryptorin.Classes;

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
                MyData myData = new MyData();
                myData.id = 1;
                myData.public_name = "qawe";
                myData.aes_key = "ads";
                myData.private_key = "awe";
                myData.login = "awe";
                myData.password = "awe";
                myData.hex_color = "awe";
                myData.key_number = 1;
                myData.image = "sdafs";

                App.myDB.SaveMyDataAsync(myData);
                await DisplayAlert("Yeaaah!","","ok");
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
    }
}