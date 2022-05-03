using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Cryptorin.Data;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.IO;
using System.Net;
using Cryptorin.Classes;

namespace Cryptorin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewRegister : ContentPage
    {
        
        FileResult file;
        public ViewRegister()
        {
            InitializeComponent();
        }





        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                file = await FilePicker.PickAsync(new PickOptions { FileTypes = FilePickerFileType.Images, PickerTitle = "Please pick the image" });
                if (file == null)
                {
                    return;
                }
                imagePicker.Source = file.FullPath;
            }
            catch (Exception ex)
            {
                await DisplayAlert("error", ex.Message, "close");
            }
        }







        private async void btnSignUp_Clicked(object sender, EventArgs e)
        {
            checkConnection checker = new checkConnection();
            bool connectionResult = checker.ConnectionAvailable(ServerAddress.srvrAddress);
            if (!connectionResult)
            {
                await DisplayAlert("Error", "The connection to the server is not established", "Ok");
                return;
            }
            string base64ImageRepresentation = null;
            if (file!=null)
            {
                byte[] imageArray = File.ReadAllBytes(file.FullPath);
                base64ImageRepresentation = Convert.ToBase64String(imageArray);
            }

            //string regexpass = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$";
            //if (Regex.IsMatch(tbPassw.Text, regexpass)&& Regex.IsMatch(tbRepPassw.Text, regexpass)&&tbPassw.Text==tbRepPassw.Text)
            //{
            //    await DisplayAlert("Done", "Registration is completed!", "Ok");
            //    await Navigation.PopAsync();
            //}

            try
            {
                
                if (tbPassw.Text == tbRepPassw.Text && tbPassw.Text!="" && tbPublicName.Text!=""&&tbLogin.Text!="" && tbPassw.Text != null && tbPublicName.Text != null && tbLogin.Text != null)
                {
                    var urlEncodedPublicName = WebUtility.UrlEncode(tbPublicName.Text);

                    classSignature signInstance = new classSignature();
                    if (signInstance.CheckLoginExists(tbLogin.Text) == "ok")
                    {

                        classSHA256 classSHA256instance = new classSHA256();
                        string hashSalt = classSHA256instance.ComputeSha256Hash(tbLogin.Text + tbPassw.Text);

                        Argon argon = new Argon();
                        string hashPasswordHex = argon.Argon2id(tbPassw.Text, hashSalt);

                        string result = signInstance.SignUp(urlEncodedPublicName, tbLogin.Text, hashPasswordHex, base64ImageRepresentation);
                        if (result == "created")
                        {
                            await DisplayAlert("Done", "Registration is completed!", "Ok");
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await DisplayAlert("Error", "error(((", "Ok");
                        }
                        
                    }
                    else
                    {
                        await DisplayAlert("Error", "This login already exists", "Ok");
                    }

                }
                else
                {
                    await DisplayAlert("Error", "Incorrectly entered data", "Ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error",ex.Message,"Ok");
            }
            
        }
    }
}